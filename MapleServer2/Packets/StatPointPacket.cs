using System.Collections.Generic;
using System.Linq;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class StatPointPacket
    {
        public static Packet WriteTotalStatPoints(Player character)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.STAT_POINT);
            // sends back some 41 character length packet with mode 00
            // this represents the total number of stat points gained and how each stat point was obtained (ie quest, trophy, exploration, prestige)
            // sent when FieldEnterHandler is called?
            pWriter.WriteByte(0);
            pWriter.WriteInt(character.StatPointDistribution.TotalStatPoints);

            return pWriter;
        }

        public static Packet WriteStatPointDistribution(Player character)
        {
            // sends back a packet that updates or loads the character's current stat distribution
            PacketWriter pWriter = PacketWriter.Of(SendOp.STAT_POINT);

            // write mode
            pWriter.WriteByte(1);
            // write number of attribute points the character has
            pWriter.WriteInt(character.StatPointDistribution.TotalStatPoints); //TODO: FIGURE OUT HOW TO SAVE TOTAL NUMBER OF ATTRIBUTE POINTS 

            // write the number of types of stat points that have points allocated
            pWriter.WriteInt(character.StatPointDistribution.GetStatTypeCount());

            // run through each entry in the character's AllocatedStats dictionary
            foreach (var StatType in character.StatPointDistribution.AllocatedStats.Keys.ToList())
            {
                // write the Stat Type (ex. Strength), then int value of points allocated
                pWriter.WriteByte(StatType);
                pWriter.WriteInt(character.StatPointDistribution.AllocatedStats[StatType]);
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