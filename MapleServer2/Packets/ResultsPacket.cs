﻿using System.Collections.Generic;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class ResultsPacket
    {
        private enum ResultsPacketMode : byte
        {
            Timed = 0x0,
            Rounds = 0x1,
            Untimed = 0x3,
        }

        public static Packet Timed(bool success, List<Item> itemRewards, bool bonus, List<Item> itemRewardsBonus = null)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESULTS);
            pWriter.WriteEnum(ResultsPacketMode.Timed);
            pWriter.WriteBool(success);
            pWriter.WriteInt(); // dungeonID
            pWriter.WriteByte();
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteByte();
            pWriter.WriteByte();
            pWriter.WriteByte();
            pWriter.WriteByte();
            pWriter.WriteInt(3); // stats loop
            pWriter.WriteByte(1); // 1 = mesos
            pWriter.WriteInt(); // mesos gained
            pWriter.WriteByte(2); // 2 = exp
            pWriter.WriteInt(); // exp gained
            pWriter.WriteByte(3); // 3 = time
            pWriter.WriteInt(); // clear time
            pWriter.WriteInt(itemRewards.Count);

            foreach (Item item in itemRewards)
            {
                pWriter.WriteInt(item.Id);
                pWriter.WriteInt(item.Amount);
                pWriter.WriteInt(item.Rarity);
            }

            pWriter.WriteByte(0x0);
            pWriter.WriteByte(0x0);
            pWriter.WriteByte(0x0);

            if (bonus)
            {
                pWriter.WriteInt();

                foreach (Item item in itemRewardsBonus)
                {
                    pWriter.WriteInt(item.Id);
                    pWriter.WriteInt(item.Amount);
                    pWriter.WriteInt(item.Rarity);
                }
            }

            pWriter.WriteInt(0x0);
            return pWriter;
        }

        public static Packet Rounds(int roundsCleared, int totalRounds, int expGain, List<Item> itemRewards)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESULTS);
            pWriter.WriteEnum(ResultsPacketMode.Rounds);
            pWriter.WriteInt(roundsCleared);
            pWriter.WriteInt(totalRounds);
            pWriter.WriteInt(expGain);

            foreach (Item item in itemRewards)
            {
                pWriter.WriteInt(item.Id);
                pWriter.WriteInt(item.Amount);
                pWriter.WriteInt(item.Rarity);
            }

            pWriter.WriteInt();
            return pWriter;
        }

        public static Packet Untimed(bool success)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESULTS);
            pWriter.WriteEnum(ResultsPacketMode.Untimed);
            pWriter.WriteBool(success);
            pWriter.WriteInt(); // dungeonID
            pWriter.WriteInt();
            pWriter.WriteInt();
            return pWriter;
        }
    }
}
