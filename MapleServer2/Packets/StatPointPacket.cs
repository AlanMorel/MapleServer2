using System.Collections.Generic;
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
            pWriter.WriteInt(18);


            return pWriter;
        }

        public static Packet WriteStatPointDistribution(Player character)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.STAT_POINT);
            // sends back a packet that updates or loads the character's current stat distribution
            pWriter.WriteByte(1);
            pWriter.WriteInt(18); // Number of attribute points the character has

            // write mode
            // write number of attribute points the character has
            // write the number of types of stat points that have points allocated
                // loop through dictionary and count any stat that is > 0
            // get dictionary of current stat point allocations
            // run through each entry in the dictionary to construct the distribution packet
            // for Stat in StatDistribution
                // write byte for Crit Rate index, value 17
                // write int for Crit Rate points allocated
                // write byte for Strength index, value 00
                // write int for Strength points allocated
                // write byte for Dex index, value 01
                // write int for Dex points allocated
                // write byte for Int index, value 02
                // write int for Int points allocated
                // write byte for Luck index, value 03
                // write int for Luck points allocated
                // write byte for Health index, value 04
                // write int for Health points allocated


            return pWriter;
        }

        public static void AddStatPoint(Player character, byte statType)
        {
            //call StatDistribution.addPoint(statType)
            character.StatPointDistribution.addPoint(statType);
            //call WriteStatPointDistribution - it takes the character's StatDistribution and converts it to a packet
        }

        public static void ResetStatPoints(Player character)
        {
            //call StatDistribution.resetPoints
            //call WriteStatPointDistribution

        }
    }
}