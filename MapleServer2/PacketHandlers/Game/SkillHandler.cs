using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
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
                    HandleMode3(session, packet);
                    break;
                case SkillHandlerMode.Mode4:
                    HandleMode4(session, packet);
                    break;
                default:
                    IPacketHandler<GameSession>.LogUnknownMode(mode);
                    break;
            }
        }

        private void HandleFirstSent(GameSession session, PacketReader packet)
        {
            long skillCount = packet.ReadLong();
            int value = packet.ReadInt();
            int skillId = packet.ReadInt();
            short skillLevel = packet.ReadShort();
            packet.ReadByte();
            CoordF coords = packet.Read<CoordF>();
            packet.ReadShort();
            session.Send(SkillUsePacket.SkillUse(value, skillCount, skillId, skillLevel, coords));
        }

        private void HandleDamage(GameSession session, PacketReader packet)
        {
            DamagingMode mode = (DamagingMode) packet.ReadByte();
            switch (mode)
            {
                case DamagingMode.TypeOfDamage:
                    HandleTypeOfDamage(session, packet);
                    break;
                case DamagingMode.AoeDamage:
                    HandleAoeDamage(session, packet);
                    break;
                case DamagingMode.TypeOfDamage2:
                    HandleTypeOfDamage2(session, packet);
                    break;
                default:
                    IPacketHandler<GameSession>.LogUnknownMode(mode);
                    break;
            }
        }

        private void HandleMode3(GameSession session, PacketReader packet)
        {
            packet.ReadLong();
            packet.ReadInt();
        }

        private void HandleMode4(GameSession session, PacketReader packet)
        {
            packet.ReadLong();
        }

        private void HandleTypeOfDamage(GameSession session, PacketReader packet)
        {
            long skillCount = packet.ReadLong();
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

        private void HandleAoeDamage(GameSession session, PacketReader packet)
        {
            long skillCount = packet.ReadLong();
            int someValue = packet.ReadInt();
            int playerObjectId = packet.ReadInt();
            CoordF coords = packet.Read<CoordF>();
            CoordF coords2 = packet.Read<CoordF>();
            CoordF coords3 = packet.Read<CoordF>();
            packet.ReadByte();
            byte count = packet.ReadByte();
            packet.ReadInt();
            /*for (int i = 0; i < count3; i++)
            {
                EntityNpc.Add(session.FieldNpc);
                packet.ReadByte();
            }
            if (EntityNpc.Count > 0)
            {
                logger.LogInformation($"entities: "+ EntityNpc.Count);
            }*/
            int objectId = packet.ReadInt();
            packet.ReadByte();

            // Hardcoded SkillId(bow), SkillLeve(1)
            session.Send(SkillDamagePacket.SkillDamage(session.FieldPlayer, skillCount, someValue, 600001, 1, coords, objectId));
        }

        private void HandleTypeOfDamage2(GameSession session, PacketReader packet)
        {
            long skillCount = packet.ReadLong();
            packet.ReadByte();
            packet.ReadInt();
            packet.ReadInt();
            packet.Read<CoordF>();
            packet.Read<CoordF>();
        }
    }
}
