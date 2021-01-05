using System.Collections.Generic;
using System.Linq;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class StatPointPacket
    {
        private enum StatPointPacketMode : byte
        {
            TotalPoints = 0x0,
            StatDistribution = 0x1
        }

        public static Packet WriteTotalStatPoints(Player character)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.STAT_POINT);
            // sends back 41 character length packet which represents the total number of stat points gained and
            // how each stat point was obtained (ie quest, trophy, exploration, prestige)
            pWriter.WriteByte((byte) StatPointPacketMode.TotalPoints);
            pWriter.WriteInt(character.StatPointDistribution.TotalStatPoints);

            return pWriter;
        }

        public static Packet WriteStatPointDistribution(Player character)
        {
            // sends back a packet that updates or loads the character's current stat distribution
            PacketWriter pWriter = PacketWriter.Of(SendOp.STAT_POINT);

            pWriter.WriteByte((byte) StatPointPacketMode.StatDistribution);
            // write number of attribute points the character has
            pWriter.WriteInt(character.StatPointDistribution.TotalStatPoints); //TODO: FIGURE OUT HOW TO SAVE TOTAL NUMBER OF ATTRIBUTE POINTS 

            // write the number of types of stat points that have points allocated
            pWriter.WriteInt(character.StatPointDistribution.GetStatTypeCount());

            foreach (byte statType in character.StatPointDistribution.AllocatedStats.Keys.ToList())
            {
                // write the Stat Type (ex. Strength), then int value of points allocated
                pWriter.WriteByte(statType);
                pWriter.WriteInt(character.StatPointDistribution.AllocatedStats[statType]);
            }

            return pWriter;
        }

        public static void AddStatPoint(Player character, byte statType)
        {
            character.StatPointDistribution.AddPoint(statType);
        }

        public static void ResetStatPoints(Player character)
        {
            character.StatPointDistribution.ResetPoints();
        }
    }
}
