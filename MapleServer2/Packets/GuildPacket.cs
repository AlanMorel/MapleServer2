using System;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class GuildPacket
    {
        private enum GuildPacketMode : byte
        {
            UpdateGuild = 0x0,
            Create = 0x1,
            CheckInConfirm = 0xF,
            Create2 = 0x12,
            GuildNoticeChange = 0x1A,
            Create3 = 0x20,
            Unk1 = 0x24,
            UpdateGuildExp = 0x31,
            UpdateGuildFunds = 0x32,
            UpdatePlayerContribution = 0x33,
            GuildNoticeConfirm = 0x3E,
            Invite = 0x4B,
            GuildWindow = 0x54,
            UpdateGuildFunds2 = 0x59, //unk
            List = 0x5A,
            UpdatePlayerDonation = 0x6E,
        }

        public static Packet UpdateGuild(GameSession session, string guildName)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteByte(0x0);
            pWriter.WriteLong(1); // guild ID
            pWriter.WriteUnicodeString(guildName);
            pWriter.WriteUnicodeString(""); // guildMark Url
            pWriter.WriteByte(0x3C); // guild.Capacity
            pWriter.WriteUnicodeString("");
            pWriter.WriteUnicodeString(""); // guild.Notice
            pWriter.WriteLong(session.Player.AccountId); // guild.Leader.AccountId
            pWriter.WriteLong(session.Player.CharacterId); // guild.Leader.CharacterId
            pWriter.WriteUnicodeString(session.Player.Name); // guild.Leader.Player.Name
            pWriter.WriteLong(1609108139); // guild creation timestamp
            pWriter.WriteByte(0x0);
            pWriter.WriteInt(1000);
            pWriter.WriteInt(0);
            pWriter.WriteInt(0);
            pWriter.WriteInt(0);
            pWriter.WriteInt(0);
            pWriter.WriteInt(0);
            pWriter.WriteInt(0);
            pWriter.WriteInt(0);
            pWriter.WriteByte(0x1);
            pWriter.WriteInt(0);
            pWriter.WriteInt(0); // guild.Exp
            pWriter.WriteInt(0); // guild.Funds
            pWriter.WriteByte(0x0);
            pWriter.WriteInt(0);
            pWriter.WriteByte(0x0); // guild.Members.Count and also loop for each member ?
            pWriter.WriteByte(0x6); // loop for guild ranks ?
            pWriter.WriteByte(0x0);
            pWriter.WriteUnicodeString("Leader");
            pWriter.WriteInt(4095);
            pWriter.WriteByte(0x1);
            pWriter.WriteUnicodeString("Jr.Leader");
            pWriter.WriteInt(1);
            pWriter.WriteByte(0x2);
            pWriter.WriteUnicodeString("Member");
            pWriter.WriteInt(1);
            pWriter.WriteByte(0x3);
            pWriter.WriteUnicodeString("NewMember");
            pWriter.WriteInt(1);
            pWriter.WriteByte(0x4);
            pWriter.WriteUnicodeString("Member2");
            pWriter.WriteInt(1);
            pWriter.WriteByte(0x5);
            pWriter.WriteUnicodeString("NewMember2");
            pWriter.WriteInt(1);
            pWriter.WriteByte(0x9); // loop for all guild skills
            pWriter.WriteInt(1); // guild skill ID
            pWriter.WriteInt(1); // guild skill level
            pWriter.WriteLong(0);
            pWriter.WriteInt(2);
            pWriter.WriteInt(1);
            pWriter.WriteLong(0);
            pWriter.WriteInt(3);
            pWriter.WriteInt(1);
            pWriter.WriteLong(0);
            pWriter.WriteInt(4);
            pWriter.WriteInt(1);
            pWriter.WriteLong(0);
            pWriter.WriteInt(10001);
            pWriter.WriteInt(1);
            pWriter.WriteLong(0);
            pWriter.WriteInt(10002);
            pWriter.WriteInt(1);
            pWriter.WriteLong(0);
            pWriter.WriteInt(10003);
            pWriter.WriteInt(1);
            pWriter.WriteLong(0);
            pWriter.WriteInt(10004);
            pWriter.WriteInt(1);
            pWriter.WriteLong(0);
            pWriter.WriteInt(10005);
            pWriter.WriteInt(1);
            pWriter.WriteLong(0);
            pWriter.WriteByte(0x4); // another loop. unk
            pWriter.WriteInt(1);
            pWriter.WriteInt(0);
            pWriter.WriteInt(2);
            pWriter.WriteInt(0);
            pWriter.WriteInt(3);
            pWriter.WriteInt(0);
            pWriter.WriteInt(4);
            pWriter.WriteInt(0);
            pWriter.WriteInt(1);
            pWriter.WriteInt(1);
            pWriter.WriteInt(0);
            pWriter.WriteByte(0x0);
            pWriter.WriteByte(0x1);
            pWriter.WriteShort(0);
            pWriter.WriteInt(0);
            pWriter.WriteInt(0);
            pWriter.WriteUnicodeString("");
            pWriter.WriteLong(0);
            pWriter.WriteLong(0);
            pWriter.WriteInt(0);
            pWriter.WriteInt(0);
            pWriter.WriteInt(0);
            pWriter.WriteInt(0);
            pWriter.WriteInt(0);
            pWriter.WriteInt(0);
            pWriter.WriteInt(0);

            return pWriter;
        }

        public static Packet Create(string guildName)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteMode(GuildPacketMode.Create);
            pWriter.WriteByte(0x0);
            pWriter.WriteUnicodeString(guildName);
            return pWriter;
        }

        public static Packet CheckInConfirm()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteMode(GuildPacketMode.CheckInConfirm);
            return pWriter;
        }

        public static Packet Create2(Player player, string guildName)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteMode(GuildPacketMode.Create2);
            pWriter.WriteShort();
            pWriter.WriteUnicodeString(player.Name);
            pWriter.WriteByte(); //unk
            pWriter.WriteShort(0x3);
            pWriter.WriteLong(0x24161B5EC67A0400); //unk guildId?
            pWriter.WriteLong(player.AccountId);
            pWriter.WriteLong(player.CharacterId);
            pWriter.WriteUnicodeString(player.Name);
            pWriter.WriteByte(); //unk
            pWriter.WriteInt(player.JobGroupId);
            pWriter.WriteInt(player.JobId);
            pWriter.WriteShort(player.Level);
            pWriter.WriteInt(); //unk
            pWriter.WriteInt(player.MapId);
            pWriter.WriteShort(); //player.Channel
            pWriter.WriteUnicodeString(player.ProfileUrl);
            pWriter.WriteInt(); //player home mapID
            pWriter.WriteInt(); // unk
            pWriter.WriteInt(); // unk
            pWriter.WriteLong(); //player home mapID expiration time
            pWriter.WriteInt(); //combat trophy count
            pWriter.WriteInt(); //adventure trophy count
            pWriter.WriteInt(); //lifestyle trophy count
            pWriter.WriteShort(); //unk, filler?
            pWriter.WriteLong(); //timestamp
            pWriter.WriteLong(); //timestamp
            pWriter.WriteLong(); //timestamp
            pWriter.WriteLong(); //unk
            pWriter.WriteLong(); //unk
            pWriter.WriteLong(); //unk
            pWriter.WriteLong(); //unk
            pWriter.WriteByte(); //unk
            return pWriter;
        }

        public static Packet GuildNoticeChange(Player player, string notice)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteMode(GuildPacketMode.GuildNoticeChange);
            pWriter.WriteUnicodeString(player.Name);
            pWriter.WriteByte(0x1);
            pWriter.WriteUnicodeString(notice);
            return pWriter;
        }

        public static Packet Create3(Player player, string guildName)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteMode(GuildPacketMode.Create3);
            pWriter.WriteUnicodeString(player.Name);
            pWriter.WriteLong(player.AccountId);
            pWriter.WriteLong(player.CharacterId);
            pWriter.WriteUnicodeString(player.Name);
            pWriter.WriteByte(); //unk
            pWriter.WriteInt(player.JobGroupId);
            pWriter.WriteInt(player.JobId);
            pWriter.WriteShort(player.Level);
            pWriter.WriteInt(); //unk
            pWriter.WriteInt(player.MapId);
            pWriter.WriteShort(); // player.Channel
            pWriter.WriteUnicodeString(player.ProfileUrl);
            pWriter.WriteInt(); // player home mapID
            pWriter.WriteInt(); // unk
            pWriter.WriteInt(); // unk
            pWriter.WriteLong(); // player home mapID expiration time
            pWriter.WriteInt(); // combat trophy count
            pWriter.WriteInt(); // adventure trophy count
            pWriter.WriteInt(); // lifestyle trophy count
            return pWriter;
        }

        public static Packet Unk1(Player player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteMode(GuildPacketMode.Unk1);
            pWriter.WriteUnicodeString(player.Name);
            pWriter.WriteInt();
            pWriter.WriteInt();
            return pWriter;
        }

        public static Packet UpdateGuildExp()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteMode(GuildPacketMode.UpdateGuildExp);
            pWriter.WriteInt(50000); // guildExp
            return pWriter;
        }

        public static Packet UpdateGuildFunds()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteMode(GuildPacketMode.UpdateGuildFunds);
            pWriter.WriteInt(10000000); // guildFunds
            return pWriter;
        }

        public static Packet UpdatePlayerContribution(Player player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteMode(GuildPacketMode.UpdatePlayerContribution);
            pWriter.WriteUnicodeString(player.Name);
            pWriter.WriteInt();
            pWriter.WriteInt(20); // Contribution today
            pWriter.WriteInt(100); // Total contribution
            return pWriter;
        }

        public static Packet GuildNoticeConfirm(string notice)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteMode(GuildPacketMode.GuildNoticeConfirm);
            pWriter.WriteByte(0x1);
            pWriter.WriteUnicodeString(notice);
            return pWriter;
        }

        public static Packet Invite(Player player, string guildName)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteMode(GuildPacketMode.Invite);
            pWriter.WriteUnicodeString(player.Name);
            pWriter.WriteUnicodeString(guildName);
            return pWriter;
        }

        public static Packet GuildWindow()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteMode(GuildPacketMode.GuildWindow);
            pWriter.WriteInt();
            return pWriter;
        }

        public static Packet UpdateGuildFunds2()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteMode(GuildPacketMode.UpdateGuildFunds2);
            pWriter.WriteInt(80); //guildExp gain
            pWriter.WriteInt(10000); // guildFunds gain
            return pWriter;
        }

        public static Packet List()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteMode(GuildPacketMode.List);
            pWriter.WriteByte(0x1);
            pWriter.WriteLong();
            pWriter.WriteLong();
            pWriter.WriteLong();
            pWriter.WriteShort();
            pWriter.WriteUnicodeString(""); //GuildMark url
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteInt(); //guild Member Total
            pWriter.WriteInt(); //guild Capacity
            return pWriter;
        }

        public static Packet UpdatePlayerDonation()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUILD);
            pWriter.WriteMode(GuildPacketMode.UpdatePlayerDonation);
            pWriter.WriteInt(0x3); // total amount of donations today
            pWriter.WriteLong(DateTimeOffset.Now.ToUnixTimeSeconds() + Environment.TickCount); // timestamp now
            return pWriter;
        }
    }
}
