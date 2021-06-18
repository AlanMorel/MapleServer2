using System;
using System.Collections.Generic;
using Maple2Storage.Types;
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
        static readonly Random rand = new Random();

        public override RecvOp OpCode => RecvOp.SKILL;

        public SkillHandler(ILogger<SkillHandler> logger) : base(logger) { }

        private enum SkillHandlerMode : byte
        {
            Cast = 0x0,     // Start of a skill
            Damage = 0x1,   // Damaging part of a skill. one is sent per hit
            Mode3 = 0x3,
            Mode4 = 0x4
        }

        private enum DamagingMode : byte
        {
            TypeOfDamage = 0x0,
            AoeDamage = 0x1,
            TypeOfDamage2 = 0x2
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
                    HandleDamage(session, packet);
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

        private static void HandleCast(GameSession session, PacketReader packet)
        {
            long skillSN = packet.ReadLong();
            int unkValue = packet.ReadInt();
            int skillId = packet.ReadInt();
            short skillLevel = packet.ReadShort();
            packet.ReadByte();
            CoordF coords = packet.Read<CoordF>();
            packet.ReadShort();

            SkillCast skillCast = session.Player.Cast(skillId, skillLevel, skillSN, unkValue);
            if (skillCast != null)
            {
                session.Send(SkillUsePacket.SkillUse(skillCast, coords));
                session.Send(StatPacket.SetStats(session.FieldPlayer));
            }
        }

        private static void HandleDamage(GameSession session, PacketReader packet)
        {
            DamagingMode mode = (DamagingMode) packet.ReadByte();
            switch (mode)
            {
                case DamagingMode.TypeOfDamage:
                    HandleTypeOfDamage(packet);
                    break;
                case DamagingMode.AoeDamage:
                    HandleAoeDamage(session, packet);
                    break;
                case DamagingMode.TypeOfDamage2:
                    HandleTypeOfDamage2(packet);
                    break;
                default:
                    IPacketHandler<GameSession>.LogUnknownMode(mode);
                    break;
            }
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
                packet.ReadLong();
                packet.ReadInt();
                packet.ReadByte();
                if (packet.ReadBool())
                {
                    packet.ReadLong();
                }
            }
        }

        private static void HandleAoeDamage(GameSession session, PacketReader packet)
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
                if (mob != null)
                {
                    DamageHandler damage = DamageHandler.CalculateDamage(session.FieldPlayer.Value.SkillCast, session.FieldPlayer.Value, mob.Value, isCrit);

                    mob.Value.Damage(damage.Damage);
                    session.Send(StatPacket.UpdateMobStats(mob));

                    if (mob.Value.IsDead)
                    {
                        HandleMobKill(session, mob);
                    }

                    if (mob.Value.Id == 29000128) // Temp fix for tutorial barrier
                    {
                        session.Send("4F 00 03 E8 03 00 00 00 00 00 00 00 00 00 00 00 00 80 3F".ToByteArray());
                        session.Send("4F 00 03 D0 07 00 00 00 00 00 00 00 00 00 00 00 00 80 3F".ToByteArray());
                        session.Send("4F 00 08 01 04 01 00 00".ToByteArray());
                    }

                    mobs.Add((mob, damage));
                }
            }
            session.Send(SkillDamagePacket.ApplyDamage(skillSN, unkValue, coords, session.FieldPlayer, mobs));
        }

        private static void HandleTypeOfDamage2(PacketReader packet)
        {
            long skillSN = packet.ReadLong();
            byte mode = packet.ReadByte();
            int unk1 = packet.ReadInt();
            int unk2 = packet.ReadInt();
            CoordF coord = packet.Read<CoordF>();
            CoordF coord1 = packet.Read<CoordF>();
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
