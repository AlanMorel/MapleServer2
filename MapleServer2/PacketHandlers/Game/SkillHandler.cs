using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class SkillHandler : GamePacketHandler
{
    private static readonly Random Rand = Random.Shared;

    public override RecvOp OpCode => RecvOp.Skill;

    private enum SkillHandlerMode : byte
    {
        Cast = 0x0,
        Damage = 0x1,
        Sync = 0x2,
        SyncTick = 0x3,
        Cancel = 0x4
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        SkillHandlerMode mode = (SkillHandlerMode) packet.ReadByte();
        switch (mode)
        {
            case SkillHandlerMode.Cast:
                HandleCast(session, packet);
                break;
            case SkillHandlerMode.Damage:
                HandleDamageMode(session, packet);
                break;
            case SkillHandlerMode.Sync:
                HandleSyncSkills(session, packet);
                break;
            case SkillHandlerMode.SyncTick:
                HandleSyncTick(packet);
                break;
            case SkillHandlerMode.Cancel:
                HandleCancelSkill(session, packet);
                break;
            default:
                IPacketHandler<GameSession>.LogUnknownMode(mode);
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

        IFieldActor<Player> fieldPlayer = session.Player.FieldPlayer;
        SkillCast skillCast = new(skillId, skillLevel, skillSN, serverTick, fieldPlayer.ObjectId, clientTick, attackPoint)
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
        fieldPlayer.Cast(skillCast);
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

        SkillCast skillCast = session.Player.FieldPlayer.SkillCast;
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

        SkillCast skillCast = session.Player.FieldPlayer.SkillCast;
        if (skillCast is null || skillCast.SkillSn != skillSn)
        {
            return;
        }

        session.FieldManager.BroadcastPacket(SkillCancelPacket.SkillCancel(skillSn, session.Player.FieldPlayer.ObjectId), session);
    }

    #region HandleDamage

    private enum DamagingMode : byte
    {
        SyncDamage = 0x0,
        Damage = 0x1,
        RegionSkill = 0x2
    }

    private static void HandleDamageMode(GameSession session, PacketReader packet)
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
                IPacketHandler<GameSession>.LogUnknownMode(mode);
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

        SkillCast skillCast = session.Player.FieldPlayer.SkillCast;
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
        packet.ReadInt();

        IFieldActor<Player> fieldPlayer = session.Player.FieldPlayer;

        SkillCast skillCast = fieldPlayer.SkillCast;
        if (skillCast is null || skillCast.SkillSn != skillSn)
        {
            return;
        }

        // TODO: Verify if its the player or an ally
        if (skillCast.IsRecovery())
        {
            Status status = new(skillCast, fieldPlayer.ObjectId, fieldPlayer.ObjectId, 1);
            StatusHandler.Handle(session, status);

            // TODO: Heal based on stats
            fieldPlayer.Heal(session, status, 50);
            return;
        }

        bool isCrit = DamageHandler.RollCrit(session.Player.Stats[StatAttribute.CritRate].Total);
        List<(int targetId, byte damageType, double damage)> damages = new();
        for (int i = 0; i < count; i++)
        {
            int entityId = packet.ReadInt();
            packet.ReadByte();

            if (entityId == playerObjectId)
            {
                damages.Add(new(playerObjectId, 0, 0));
                continue;
            }

            IFieldActor<NpcMetadata> mob = session.FieldManager.State.Mobs.GetValueOrDefault(entityId);
            if (mob == null)
            {
                continue;
            }
            skillCast.Target = mob;

            DamageHandler damage = DamageHandler.CalculateDamage(skillCast, fieldPlayer, mob, isCrit);

            mob.Damage(damage, session);

            damages.Add(new(damage.Target.ObjectId, (byte) (isCrit ? 1 : 0), damage.Damage));

            // TODO: Check if the skill is a debuff for an entity
            if (!skillCast.IsDebuffElement() && !skillCast.IsDebuffToEntity() && !skillCast.IsDebuffElement())
            {
                continue;
            }

            Status status = new(skillCast, mob.ObjectId, fieldPlayer.ObjectId, 1);
            StatusHandler.Handle(session, status);
        }

        session.FieldManager.BroadcastPacket(SkillDamagePacket.Damage(skillCast, attackCounter, position, rotation, damages));
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

        SkillCast parentSkill = session.Player.FieldPlayer.SkillCast;

        if (parentSkill is null || parentSkill.SkillSn != skillSn)
        {
            return;
        }

        SkillAttack skillAttack = parentSkill.GetSkillMotions().FirstOrDefault()?.SkillAttacks.FirstOrDefault();
        if (skillAttack is null)
        {
            return;
        }

        if (skillAttack.CubeMagicPathId == 0 && skillAttack.MagicPathId == 0)
        {
            return;
        }

        SkillCondition skillCondition = skillAttack.SkillConditions.FirstOrDefault(x => x.IsSplash);
        if (skillCondition is null)
        {
            return;
        }

        SkillCast skillCast = new(skillCondition.SkillId, skillCondition.SkillLevel, GuidGenerator.Long(), session.ServerTick, parentSkill)
        {
            SkillAttack = skillAttack,
            Duration = skillCondition.FireCount * 1000,
            Interval = skillCondition.Interval
        };
        RegionSkillHandler.HandleEffect(session.FieldManager, skillCast, attackIndex);
    }

    #endregion
}
