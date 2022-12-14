using Maple2.PathEngine.Types;
using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Managers.Actors;
using MapleServer2.Network;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;
using MapleServer2.Types;
using Org.BouncyCastle.Asn1.X509;

namespace MapleServer2.PacketHandlers.Game;

public class SkillHandler : GamePacketHandler<SkillHandler>
{
    public override RecvOp OpCode => RecvOp.Skill;

    private enum Mode : byte
    {
        Cast = 0x0,
        Damage = 0x1,
        Sync = 0x2,
        SyncTick = 0x3,
        Cancel = 0x4
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        Mode mode = (Mode) packet.ReadByte();
        switch (mode)
        {
            case Mode.Cast:
                HandleCast(session, packet);
                break;
            case Mode.Damage:
                HandleDamageMode(session, packet);
                break;
            case Mode.Sync:
                HandleSyncSkills(session, packet);
                break;
            case Mode.SyncTick:
                HandleSyncTick(packet);
                break;
            case Mode.Cancel:
                HandleCancelSkill(session, packet);
                break;
            default:
                LogUnknownMode(mode);
                break;
        }
    }

    private static void HandleCast(GameSession session, PacketReader packet)
    {
        long skillSN = packet.ReadLong();
        int serverTick = packet.ReadInt();
        int skillId = packet.ReadInt();
        short skillLevel = packet.ReadShort();
        byte attackPoint = packet.ReadByte();
        CoordF position = packet.Read<CoordF>();
        CoordF direction = packet.Read<CoordF>();
        CoordF rotation = packet.Read<CoordF>();
        packet.ReadFloat();
        int clientTick = packet.ReadInt();
        packet.ReadBool();
        packet.ReadLong();
        bool flag = packet.ReadBool();
        if (flag)
        {
            packet.ReadInt();
            string unkString = packet.ReadUnicodeString();
        }

        if (session.Player.FieldPlayer is null)
        {
            return;
        }

        IFieldActor<Player> fieldPlayer = session.Player.FieldPlayer;
        SkillCast skillCast = new(skillId, skillLevel, skillSN, serverTick, fieldPlayer, clientTick, attackPoint)
        {
            Position = position,
            Direction = direction,
            Rotation = rotation,
            LookDirection = fieldPlayer.LookDirection
        };

        /* HOW TO HANDLE ADDITIONAL EFFECTS BY MAYGI:
            when handling an additional effect:
            + loop through all SkillMotion items in the skill
            ++ loop through all SkillAttacks in the attacks in the SkillMotion
            +++ grab a list of ConditionSkills from each SkillAttack
            +++ check for a cube magic path id on each SkillAttack and handle it if it exists
            ++++ if a cube magic path exists, you can grab a ConditionSkill reference from any index of the list, it doesn't matter in this case
            ++++ handle magic path move processing for square based abilities
            +++ loop through all ConditionSkills
            ++++ loop through all SkillData on each ConditionSkill and check the SkillInfo on each Condition Skill ID
            +++++ check if each SkillInfo has an additional skill
            ++++++ if an additional skill exists, check the proc and requirements (proc is a chance, requirement is a buff ID), and determine whether or not to proc additional effects for said proc)
            + also handle Splash Skills which trigger region effects
         */

        // TODO: Check BeginCondition
        fieldPlayer.TaskScheduler.QueueBufferedTask(() => fieldPlayer.Cast(skillCast));
    }

    private static void HandleSyncSkills(GameSession session, PacketReader packet)
    {
        long skillSn = packet.ReadLong();
        int skillId = packet.ReadInt();
        short skillLevel = packet.ReadShort();
        byte motionPoint = packet.ReadByte();
        CoordF position = packet.Read<CoordF>();
        CoordF unkCoords = packet.Read<CoordF>();
        CoordF rotation = packet.Read<CoordF>();
        CoordF unknown = packet.Read<CoordF>();
        bool toggle = packet.ReadBool();
        packet.ReadInt();
        packet.ReadByte();

        if (session.Player.FieldPlayer is null)
        {
            return;
        }

        SkillCast? skillCast = session.Player.FieldPlayer.SkillCast;
        if (skillCast is null)
        {
            return;
        }

        session.FieldManager.BroadcastPacket(SkillSyncPacket.Sync(skillCast, session.Player.FieldPlayer, position, rotation, toggle), session);
    }

    private static void HandleSyncTick(PacketReader packet)
    {
        long skillSN = packet.ReadLong();
        int serverTick = packet.ReadInt();
    }

    private static void HandleCancelSkill(GameSession session, PacketReader packet)
    {
        long skillSn = packet.ReadLong();

        if (session.Player.FieldPlayer is null)
        {
            return;
        }

        SkillCast? skillCast = session.Player.FieldPlayer?.SkillCast;
        if (skillCast is null || skillCast.SkillSn != skillSn)
        {
            return;
        }

        session.Player.FieldPlayer?.TaskScheduler.QueueBufferedTask(() =>
        {
            session.Player.FieldPlayer.SkillTriggerHandler.FireEvents(null, null, EffectEvent.OnSkillCastEnd, skillCast.SkillId);

            session.FieldManager.BroadcastPacket(SkillCancelPacket.SkillCancel(skillSn, session.Player.FieldPlayer.ObjectId), session);
        });
    }

    #region HandleDamage

    private enum DamagingMode : byte
    {
        SyncDamage = 0x0,
        Damage = 0x1,
        RegionSkill = 0x2
    }

    private void HandleDamageMode(GameSession session, PacketReader packet)
    {
        DamagingMode mode = (DamagingMode) packet.ReadByte();
        switch (mode)
        {
            case DamagingMode.SyncDamage:
                HandleSyncDamage(session, packet);
                break;
            case DamagingMode.Damage:
                HandleDamage(session, packet);
                break;
            case DamagingMode.RegionSkill:
                HandleRegionSkills(session, packet);
                break;
            default:
                LogUnknownMode(mode);
                break;
        }
    }

    private static void HandleSyncDamage(GameSession session, PacketReader packet)
    {
        long skillSn = packet.ReadLong();
        byte attackPoint = packet.ReadByte();
        CoordF position = packet.Read<CoordF>();
        CoordF rotation = packet.Read<CoordF>();
        byte count = packet.ReadByte();
        packet.ReadInt();

        List<int> atkCount = new();
        List<int> sourceId = new();
        List<int> targetId = new();
        List<short> animation = new();
        // TODO: Handle multiple projectiles
        for (int i = 0; i < count; i++)
        {
            atkCount.Add(packet.ReadInt());
            sourceId.Add(packet.ReadInt());
            targetId.Add(packet.ReadInt());
            animation.Add(packet.ReadShort());
        }

        if (session.Player.FieldPlayer is null)
        {
            return;
        }

        SkillCast? skillCast = session.Player.FieldPlayer.SkillCast;
        if (skillCast is null)
        {
            return;
        }

        session.FieldManager.BroadcastPacket(SkillDamagePacket.SyncDamage(skillCast, position, rotation, session.Player.FieldPlayer, sourceId, count, atkCount,
            targetId, animation));
    }

    private static void HandleDamage(GameSession session, PacketReader packet)
    {
        long skillSn = packet.ReadLong();
        int attackCounter = packet.ReadInt();
        int playerObjectId = packet.ReadInt();
        CoordF position = packet.Read<CoordF>();
        CoordF impactPos = packet.Read<CoordF>();
        CoordF rotation = packet.Read<CoordF>();
        int attackPoint = packet.ReadByte();
        byte count = packet.ReadByte();
        int unknownInt = packet.ReadInt();

        int tick = Environment.TickCount;

        int entityId = packet.ReadInt();
        packet.ReadByte();

        session.Player.FieldPlayer?.TaskScheduler.QueueBufferedTask(() => HandleDamage(session, skillSn, count, attackPoint, entityId, playerObjectId, attackCounter, position, rotation));
    }

    private static void HandleDamage(GameSession session, long skillSn, byte count, int attackPoint, int entityId, int playerObjectId, int attackCounter, CoordF position, CoordF rotation)
    {
        IFieldActor<Player>? fieldPlayer = session.Player.FieldPlayer;

        if (fieldPlayer is null)
        {
            return;
        }

        SkillCast? skillCast = fieldPlayer?.SkillCast;
        if (skillCast is null || skillCast.SkillSn != skillSn)
        {
            return;
        }

        IFieldActor? caster = skillCast.Caster;
        IFieldActor? target = session.FieldManager.State.Mobs.GetValueOrDefault(entityId);
        target = target ?? session.FieldManager.State.Players.GetValueOrDefault(entityId);

        if (target is null)
        {
            return;
        }

        HandleDamage(skillCast, target, count, attackPoint, attackCounter, position, rotation);
    }

    public static void HandleDamage(SkillCast skillCast, IFieldActor target, int count, int attackPoint, int attackCounter, CoordF position, CoordF rotation)
    {

        IFieldActor? caster = skillCast.Caster;
        GameSession? session = (caster as Character)?.Value?.Session;

        List<DamageHandler> damages = new();

        for (int i = 0; i < count; i++)
        {
            skillCast.Target = target;
            skillCast.AttackPoint = (byte) attackPoint;

            foreach (SkillMotion motion in skillCast.GetSkillMotions())
            {
                SkillAttack? attack = motion.SkillAttacks?[attackPoint];

                if (attack is null)
                {
                    continue;
                }

                skillCast.SkillAttack = attack;

                if (caster == target && attack.RangeProperty.ApplyTarget != ApplyTarget.Ally)
                {
                    continue;
                }

                ConditionSkillTarget castInfo = new(caster, target, caster, caster, EffectEventOrigin.Caster);
                bool hitCrit = false;
                bool hitMissed = false;

                if (skillCast.GetDamageRate() != 0)
                {
                    DamageHandler damage = DamageHandler.CalculateDamage(skillCast, caster, target);

                    target.Damage(damage, session);

                    damages.Add(damage);

                    hitCrit = damage.HitType == Enums.HitType.Critical;
                    hitMissed = damage.HitType == Enums.HitType.Miss;
                }

                caster.TaskScheduler.QueueBufferedTask(() => caster?.SkillTriggerHandler.FireTriggerSkills(attack.SkillConditions, skillCast, castInfo));

                target.SkillTriggerHandler.OnAttacked(caster, skillCast.SkillId, !hitMissed, hitCrit, hitMissed, false);
            }
        }

        session?.FieldManager.BroadcastPacket(SkillDamagePacket.Damage(skillCast, attackCounter, position, rotation, damages));
    }

    private static void HandleRegionSkills(GameSession session, PacketReader packet)
    {
        long skillSn = packet.ReadLong();
        byte mode = packet.ReadByte();
        int unknown = packet.ReadInt();
        int attackIndex = packet.ReadInt();
        CoordF position = packet.Read<CoordF>();
        CoordF rotation = packet.Read<CoordF>();
        // What are these values used? Check client vs server?

        // TODO: Verify rest of skills to proc correctly.
        // TODO: Send status correctly when Region attacks are proc.

        SkillCast? parentSkill = session.Player.FieldPlayer?.SkillCast;

        if (parentSkill is null || parentSkill.SkillSn != skillSn)
        {
            return;
        }

        SkillAttack? skillAttack = parentSkill.GetSkillMotions().FirstOrDefault()?.SkillAttacks[attackIndex];
        if (skillAttack is null)
        {
            return;
        }

        if (skillAttack.CubeMagicPathId == 0 && skillAttack.MagicPathId == 0)
        {
            return;
        }

        SkillCast skillCast = new(parentSkill.SkillId, parentSkill.SkillLevel, GuidGenerator.Long(), session.ServerTick, parentSkill)
        {
            Owner = parentSkill.Caster,
            Caster = parentSkill.Caster,
            SkillAttack = skillAttack,
            Rotation = parentSkill.Rotation,
            Direction = parentSkill.Direction,
            LookDirection = parentSkill.LookDirection
        };
        ConditionSkillTarget castInfo = new(parentSkill.Caster, parentSkill.Caster, parentSkill.Caster, parentSkill.Caster, EffectEventOrigin.Caster);

        session.Player.FieldPlayer?.TaskScheduler.QueueBufferedTask(() =>
            parentSkill.Caster?.SkillTriggerHandler.FireTriggerSkills(skillAttack.SkillConditions, skillCast, castInfo)
        );
    }

    #endregion
}
