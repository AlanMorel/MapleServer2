using Maple2Storage.Enums;
using Maple2Storage.Tools;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.PacketHandlers.Game.Helpers;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class SkillHandler : GamePacketHandler
{
    private static readonly Random Rand = RandomProvider.Get();

    public override RecvOp OpCode => RecvOp.SKILL;

    private enum SkillHandlerMode : byte
    {
        Cast = 0x0,
        Damage = 0x1,
        Sync = 0x2,
        SyncTick = 0x3,
        Cancel = 0x4
    }

    private enum DamagingMode : byte
    {
        SyncDamage = 0x0,
        Damage = 0x1,
        RegionSkill = 0x2
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
                HandleCancelSkill(packet);
                break;
            default:
                IPacketHandler<GameSession>.LogUnknownMode(mode);
                break;
        }
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

        SkillCast skillCast = new(skillId, skillLevel, skillSN, serverTick, session.Player.FieldPlayer.ObjectId, clientTick, attackPoint)
        {
            Position = position,
            Direction = direction,
            Rotation = rotation
        };
        session.Player.FieldPlayer.Cast(skillCast);
    }

    private static void HandleSyncSkills(GameSession session, PacketReader packet)
    {
        long skillSN = packet.ReadLong();
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

        session.FieldManager.BroadcastPacket(SkillSyncPacket.Sync(skillSN, session.Player.FieldPlayer, position, rotation, toggle), session);
    }

    private static void HandleSyncTick(PacketReader packet)
    {
        long skillSN = packet.ReadLong();
        int serverTick = packet.ReadInt();
    }

    private static void HandleCancelSkill(PacketReader packet)
    {
        long skillSN = packet.ReadLong();
    }

    private static void HandleSyncDamage(GameSession session, PacketReader packet)
    {
        long skillSN = packet.ReadLong();
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

        session.FieldManager.BroadcastPacket(SkillDamagePacket.SyncDamage(skillSN, position, rotation, session.Player.FieldPlayer, sourceId, count, atkCount, targetId, animation));
    }

    private static void HandleDamage(GameSession session, PacketReader packet)
    {
        long skillSN = packet.ReadLong();
        int attackCounter = packet.ReadInt();
        int playerObjectId = packet.ReadInt();
        CoordF position = packet.Read<CoordF>();
        CoordF impactPos = packet.Read<CoordF>();
        CoordF rotation = packet.Read<CoordF>();
        int attackPoint = packet.ReadByte();
        byte count = packet.ReadByte();
        packet.ReadInt();

        IFieldActor<Player> fieldPlayer = session.Player.FieldPlayer;

        bool isCrit = DamageHandler.RollCrit(session.Player.Stats[StatId.CritRate].Total);

        // TODO: Check if skillSN matches server's current skill for the player
        // TODO: Verify if its the player or an ally
        if (fieldPlayer.SkillCast.IsHeal())
        {
            Status status = new(fieldPlayer.SkillCast, fieldPlayer.ObjectId, fieldPlayer.ObjectId, 1);
            StatusHandler.Handle(session, status);

            // TODO: Heal based on stats
            session.FieldManager.BroadcastPacket(SkillDamagePacket.Heal(status, 50));
            fieldPlayer.Stats[StatId.Hp].Increase(50);
            session.Send(StatPacket.UpdateStats(fieldPlayer, StatId.Hp));
        }
        else
        {
            List<DamageHandler> damages = new();
            for (int i = 0; i < count; i++)
            {
                int entityId = packet.ReadInt();
                packet.ReadByte();

                IFieldActor<NpcMetadata> mob = session.FieldManager.State.Mobs.GetValueOrDefault(entityId);
                if (mob == null)
                {
                    continue;
                }

                DamageHandler damage = DamageHandler.CalculateDamage(fieldPlayer.SkillCast, fieldPlayer, mob, isCrit);

                mob.Damage(damage);
                // TODO: Move logic to Damage()
                session.FieldManager.BroadcastPacket(StatPacket.UpdateMobStats(mob));
                if (mob.IsDead)
                {
                    HandleMobKill(session, mob);
                }

                damages.Add(damage);

                // TODO: Check if the skill is a debuff for an entity
                SkillCast skillCast = fieldPlayer.SkillCast;
                if (skillCast.IsDebuffElement() || skillCast.IsDebuffToEntity() || skillCast.IsDebuffElement())
                {
                    Status status = new(fieldPlayer.SkillCast, mob.ObjectId, fieldPlayer.ObjectId, 1);
                    StatusHandler.Handle(session, status);
                }
            }

            session.FieldManager.BroadcastPacket(SkillDamagePacket.Damage(skillSN, attackCounter, position, rotation, fieldPlayer, damages));
        }
    }

    private static void HandleRegionSkills(GameSession session, PacketReader packet)
    {
        long skillSN = packet.ReadLong();
        byte mode = packet.ReadByte();
        int unknown = packet.ReadInt();
        int unknown2 = packet.ReadInt();
        CoordF position = packet.Read<CoordF>();
        CoordF rotation = packet.Read<CoordF>();

        // TODO: Verify rest of skills to proc correctly.
        // Send status correctly when Region attacks are proc.
        SkillCast parentSkill = SkillUsePacket.SkillCastMap[skillSN];

        if (parentSkill.GetConditionSkill() == null)
        {
            return;
        }

        foreach (SkillCondition conditionSkill in parentSkill.GetConditionSkill())
        {
            if (!conditionSkill.Splash)
            {
                continue;
            }

            SkillCast skillCast = new(conditionSkill.Id, conditionSkill.Level, GuidGenerator.Long(), session.ServerTick, parentSkill);
            RegionSkillHandler.Handle(session, GuidGenerator.Int(), session.Player.FieldPlayer.Coord, skillCast);
        }
    }

    private static void HandleMobKill(GameSession session, IFieldObject<NpcMetadata> mob)
    {
        // TODO: Add trophy + item drops
        // Drop Money
        bool dropMeso = Rand.Next(2) == 0;
        if (dropMeso)
        {
            // TODO: Calculate meso drop rate
            Item meso = new(90000001, Rand.Next(2, 800));
            session.FieldManager.AddResource(meso, mob, session.Player.FieldPlayer);
        }
        // Drop Meret
        bool dropMeret = Rand.Next(40) == 0;
        if (dropMeret)
        {
            Item meret = new(90000004, 20);
            session.FieldManager.AddResource(meret, mob, session.Player.FieldPlayer);
        }
        // Drop SP
        bool dropSP = Rand.Next(6) == 0;
        if (dropSP)
        {
            Item spBall = new(90000009, 20);
            session.FieldManager.AddResource(spBall, mob, session.Player.FieldPlayer);
        }
        // Drop EP
        bool dropEP = Rand.Next(10) == 0;
        if (dropEP)
        {
            Item epBall = new(90000010, 20);
            session.FieldManager.AddResource(epBall, mob, session.Player.FieldPlayer);
        }
        // Drop Items
        // Send achieves (?)
        // Gain Mob EXP
        session.Player.Levels.GainExp(mob.Value.Experience);
        // Send achieves (2)

        string mapId = session.Player.MapId.ToString();
        // Prepend zero if map id is equal to 7 digits
        if (mapId.Length == 7)
        {
            mapId = $"0{mapId}";
        }

        // Quest Check
        QuestHelper.UpdateQuest(session, mob.Value.Id.ToString(), "npc", mapId);
    }
}
