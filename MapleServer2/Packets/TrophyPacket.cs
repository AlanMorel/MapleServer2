using System.Collections.Generic;
using System.Linq;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class TrophyPacket
    {
        private enum TrophyPacketMode : byte
        {
            TableStart = 0x0,
            TableContent = 0x1,
            Update = 0x2
        }

        public enum GradeStatus : byte
        {
            NotFinalGrade = 0x0,
            FinalGrade = 0x3
        }

        public static Packet WriteTableStart()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.TROPHY);
            pWriter.WriteEnum(TrophyPacketMode.TableStart);

            return pWriter;
        }

        // packet from WriteTableStart() must be sent immediately before sending these packets
        public static Packet WriteTableContent(List<Trophy> trophies)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.TROPHY);
            pWriter.WriteEnum(TrophyPacketMode.TableContent);
            pWriter.WriteInt(trophies.Count);

            foreach (Trophy trophy in trophies)
            {
                pWriter.WriteInt(trophy.Id);
                pWriter.WriteInt(1);            // unknown 'SS' ?
                WriteIndividualTrophy(pWriter, trophy);
            }

            return pWriter;
        }

        public static Packet WriteUpdate(Trophy trophy)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.TROPHY);
            pWriter.WriteEnum(TrophyPacketMode.Update);
            pWriter.WriteInt(trophy.Id);
            WriteIndividualTrophy(pWriter, trophy);

            return pWriter;
        }

        private static void WriteIndividualTrophy(PacketWriter pWriter, Trophy trophy)
        {
            int tCount = trophy.Timestamps.Count;

            pWriter.WriteEnum(trophy.GetGradeStatus());
            pWriter.WriteInt(1);
            pWriter.WriteInt(trophy.NextGrade);
            pWriter.WriteInt(trophy.MaxGrade);
            pWriter.WriteByte(0);
            pWriter.WriteLong(trophy.Counter);
            pWriter.WriteInt(tCount);
            for (int t = 0; t < tCount; t++)
            {
                pWriter.WriteInt(t + 1);
                pWriter.WriteLong(trophy.Timestamps.ElementAt(t));
            }
        }
    }
}
