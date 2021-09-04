using Maple2Storage.Tools;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.PacketHandlers.Game.Helpers;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class SkillHandler : GamePacketHandler
    {
        private static readonly Random rand = RandomProvider.Get();

        public override RecvOp OpCode => RecvOp.SKILL;

        public SkillHandler(ILogger<SkillHandler> logger) : base(logger) { }

        private enum SkillHandlerMode : byte
        {
            Cast = 0x0,     // Start of a skill
            Damage = 0x1,   // Damaging part of a skill. one is sent per hit
            HoldCast = 0x2,    // Cast continues skills
            Mode3 = 0x3,
            Mode4 = 0x4,
        }

        private enum DamagingMode : byte
        {
            TypeOfDamage = 0x0,
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
                case SkillHandlerMode.HoldCast:
                    HandleHoldCast(session, packet);
                    break;
                case SkillHandlerMode.Mode3:
                    HandleMode3(packet);
                    break;
                case SkillHandlerMode.Mode4:
                    HandleMode4(packet);
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
                case DamagingMode.TypeOfDamage:
                    HandleTypeOfDamage(packet);
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
            int unkValue = packet.ReadInt();
            int skillId = packet.ReadInt();
            short skillLevel = packet.ReadShort();
            packet.ReadByte();
            CoordF coords = packet.Read<CoordF>();
            CoordF coords1 = packet.Read<CoordF>();
            CoordF rotation = packet.Read<CoordF>();
            packet.ReadInt();
            int unkValue2 = packet.ReadInt();

            SkillCast skillCast = session.FieldPlayer.Value.Cast(skillId, skillLevel, skillSN, unkValue);
            if (skillCast != null)
            {
                session.FieldManager.BroadcastPacket(SkillUsePacket.SkillUse(skillCast, coords, rotation));
                session.Send(StatPacket.SetStats(session.FieldPlayer));
            }
        }

        private static void HandleHoldCast(GameSession session, PacketReader packet)
        {

        }

        private static void HandleMode3(PacketReader packet)
        {
            packet.ReadLong();
            packet.ReadInt();
        }

        private static void HandleMode4(PacketReader packet)
        {
            packet.ReadLong();
        }

        private static void HandleTypeOfDamage(PacketReader packet)
        {
            long skillSN = packet.ReadLong();
            packet.ReadByte();
            CoordF coords = packet.Read<CoordF>();
            CoordF coords2 = packet.Read<CoordF>();
            int count = packet.ReadByte();
            packet.ReadInt();
            for (int i = 0; i < count; i++)
            {
                packet.ReadInt(); // increment +1 every count
                packet.ReadInt();
                packet.ReadInt();
                packet.ReadShort(); // increment +1 every count
            }
        }

        private static void HandleDamage(GameSession session, PacketReader packet)
        {
            List<(IFieldObject<Mob>, DamageHandler)> mobs = new List<(IFieldObject<Mob>, DamageHandler)>();
            long skillSN = packet.ReadLong();
            int unkValue = packet.ReadInt();
            int playerObjectId = packet.ReadInt();
            CoordF coords = packet.Read<CoordF>();
            CoordF coords2 = packet.Read<CoordF>();
            CoordF coords3 = packet.Read<CoordF>();
            packet.ReadByte();
            byte count = packet.ReadByte();
            packet.ReadInt();

            bool isCrit = DamageHandler.RollCrit(session.Player.Stats[PlayerStatId.CritRate].Current);
            for (int i = 0; i < count; i++)
            {
                int entity = packet.ReadInt();
                packet.ReadByte();

                IFieldObject<Mob> mob = session.FieldManager.State.Mobs.GetValueOrDefault(entity);
                if (mob == null)
                {
                    continue;
                }

                DamageHandler damage = DamageHandler.CalculateDamage(session.FieldPlayer.Value.SkillCast, session.FieldPlayer.Value, mob.Value, isCrit);

                mob.Value.Damage(damage.Damage);
                session.Send(StatPacket.UpdateMobStats(mob));

                if (mob.Value.IsDead)
                {
                    HandleMobKill(session, mob);
                }

                mobs.Add((mob, damage));

                // TODO: Check if the skill is a debuff for an entity
                SkillCast skillCast = session.FieldPlayer.Value.SkillCast;
                if (skillCast.IsDebuffElement() || skillCast.IsDebuffToEntity() || skillCast.IsDebuffElement())
                {
                    Status status = new Status(session.FieldPlayer.Value.SkillCast, mob.ObjectId, session.FieldPlayer.ObjectId, 1);
                    StatusHandler.Handle(session, status);
                }
            }
            // TODO: Verify if its the player or an ally
            if (session.FieldPlayer.Value.SkillCast.IsHeal())
            {
                Status status = new Status(session.FieldPlayer.Value.SkillCast, session.FieldPlayer.ObjectId, session.FieldPlayer.ObjectId, 1);
                StatusHandler.Handle(session, status);

                // TODO: Heal based on stats
                session.FieldManager.BroadcastPacket(SkillDamagePacket.Heal(status, 50));
                session.FieldPlayer.Value.Stats.Increase(PlayerStatId.Hp, 50);
                session.Send(StatPacket.UpdateStats(session.FieldPlayer, PlayerStatId.Hp));
            }
            else
            {
                session.FieldManager.BroadcastPacket(SkillDamagePacket.Damage(session.FieldPlayer.Value.SkillCast.SkillSN, unkValue, coords, session.FieldPlayer, mobs));
            }
        }

        private static void HandleRegionSkills(GameSession session, PacketReader packet)
        {
            long skillSN = packet.ReadLong();
            byte mode = packet.ReadByte();
            int unknown = packet.ReadInt();
            int unknown2 = packet.ReadInt();
            CoordF coord = packet.Read<CoordF>();
            CoordF coord1 = packet.Read<CoordF>();
            SkillCast skillCast = SkillUsePacket.SkillCastMap[skillSN];
            // TODO: Verify rest of skills to proc correctly.
            // Send status correctly when Region attacks are proc.
            if (skillCast?.GetConditionSkill() == null)
            {
                return;
            }
            foreach (SkillAttack skill in skillCast.GetConditionSkill())
            {
                if (!skill.Splash)
                {
                    continue;
                }
                RegionSkillHandler.Handle(session, session.FieldPlayer.ObjectId, coord, skillCast);
            }
        }

        private static void HandleMobKill(GameSession session, IFieldObject<Mob> mob)
        {
            // TODO: Add trophy + item drops
            // Drop Money
            bool dropMeso = rand.Next(2) == 0;
            if (dropMeso)
            {
                // TODO: Calculate meso drop rate
                Item meso = new Item(90000001, rand.Next(2, 800));
                session.FieldManager.AddResource(meso, mob, session.FieldPlayer);
            }
            // Drop Meret
            bool dropMeret = rand.Next(40) == 0;
            if (dropMeret)
            {
                Item meret = new Item(90000004, 20);
                session.FieldManager.AddResource(meret, mob, session.FieldPlayer);
            }
            // Drop SP
            bool dropSP = rand.Next(6) == 0;
            if (dropSP)
            {
                Item spBall = new Item(90000009, 20);
                session.FieldManager.AddResource(spBall, mob, session.FieldPlayer);
            }
            // Drop EP
            bool dropEP = rand.Next(10) == 0;
            if (dropEP)
            {
                Item epBall = new Item(90000010, 20);
                session.FieldManager.AddResource(epBall, mob, session.FieldPlayer);
            }
            // Drop Items
            // Send achieves (?)
            // Gain Mob EXP
            session.Player.Levels.GainExp(mob.Value.Experience);
            // Send achieves (2)
            // Quest Check
            QuestHelper.UpdateQuest(session, mob.Value.Id.ToString(), "npc");
        }
    }
}
