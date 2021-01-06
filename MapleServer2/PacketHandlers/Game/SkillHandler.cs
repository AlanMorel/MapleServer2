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
            short skillLevel;
            long skillCount;
            int skillId;
            int value;
            int playerObjectId;
            CoordF coords;
            CoordF coords2;
            CoordF coords3;

            SkillHandlerMode mode = (SkillHandlerMode)packet.ReadByte();
            switch (mode)
            {
                case SkillHandlerMode.FirstSent:
                    skillCount = packet.ReadLong();
                    value = packet.ReadInt();
                    skillId = packet.ReadInt();
                    skillLevel = packet.ReadShort();
                    packet.ReadByte();
                    coords = packet.Read<CoordF>();
                    packet.ReadShort();
                    session.Send(SkillUsePacket.SkillUse(value, skillCount, skillId, skillLevel, coords));
                    break;
                case SkillHandlerMode.Damage:
                    DamagingMode type = (DamagingMode)packet.ReadByte();
                    switch (type)
                    {
                        case DamagingMode.TypeOfDamage:
                            skillCount = packet.ReadLong();
                            packet.ReadByte();
                            coords = packet.Read<CoordF>();
                            coords2 = packet.Read<CoordF>();
                            int count2 = packet.ReadByte();
                            packet.ReadInt();
                            for (int i = 0; i < count2; i++)
                            {
                                packet.ReadLong();
                                packet.ReadInt();
                                packet.ReadByte();
                                if (packet.ReadBool())
                                {
                                    packet.ReadLong();
                                }
                            }
                            break;
                        case DamagingMode.AoeDamage:
                            skillCount = packet.ReadLong();
                            int someValue = packet.ReadInt();
                            playerObjectId = packet.ReadInt();
                            coords = packet.Read<CoordF>();
                            coords2 = packet.Read<CoordF>();
                            coords3 = packet.Read<CoordF>();
                            packet.ReadByte();
                            byte count3 = packet.ReadByte();
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
                            session.Send(SkillUsePacket.SkillDamage(session.FieldPlayer, skillCount, someValue, 600001, 1, coords, objectId));
                            break;
                        case DamagingMode.TypeOfDamage2:
                            skillCount = packet.ReadLong();
                            packet.ReadByte();
                            packet.ReadInt();
                            packet.ReadInt();
                            packet.Read<CoordF>();
                            packet.Read<CoordF>();
                            break;
                        default:
                            break;
                    }
                    break;
                case SkillHandlerMode.Mode3:
                    packet.ReadLong();
                    packet.ReadInt();
                    break;
                case SkillHandlerMode.Mode4:
                    packet.ReadLong();
                    break;
                default:
                    IPacketHandler<GameSession>.LogUnknownMode(mode);
                    break;
            }
        }
    }
}
