using System.Collections.Generic;
using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class SkillHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.SKILL;

        public SkillHandler(ILogger<SkillHandler> logger) : base(logger) { }

        private enum SkillHandlerMode : byte
        {
            FirstSent = 0x0,  // Start of a skill
            Damage = 0x1,     // Damaging part of a skill. one is sent per hit
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
                case SkillHandlerMode.FirstSent:
                    HandleFirstSent(session, packet);
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

        private static void HandleFirstSent(GameSession session, PacketReader packet)
        {
            long skillUid = packet.ReadLong();
            int value = packet.ReadInt();
            int skillId = packet.ReadInt();
            short skillLevel = packet.ReadShort();
            packet.ReadByte();
            CoordF coords = packet.Read<CoordF>();
            packet.ReadShort();
            session.Send(SkillUsePacket.SkillUse(session.FieldPlayer, value, skillUid, coords));
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
            long skillUid = packet.ReadLong();
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
            List<IFieldObject<Mob>> mobs = new List<IFieldObject<Mob>>();
            long skillUid = packet.ReadLong();
            int someValue = packet.ReadInt();
            int playerObjectId = packet.ReadInt();
            CoordF coords = packet.Read<CoordF>();
            CoordF coords2 = packet.Read<CoordF>();
            CoordF coords3 = packet.Read<CoordF>();
            packet.ReadByte();
            byte count = packet.ReadByte();
            packet.ReadInt();
            for (int i = 0; i < count; i++)
            {
                mobs.Add(session.FieldManager.State.Mobs.GetValueOrDefault(packet.ReadInt()));
                packet.ReadByte();
                session.Send(StatPacket.UpdateMobStats(mobs[i]));
            }

            session.Send(SkillDamagePacket.ApplyDamage(session.FieldPlayer, skillUid, someValue, coords, mobs));
        }

        private static void HandleTypeOfDamage2(PacketReader packet)
        {
            long skillUid = packet.ReadLong();
            packet.ReadByte();
            packet.ReadInt();
            packet.ReadInt();
            packet.Read<CoordF>();
            packet.Read<CoordF>();
        }
    }
}
