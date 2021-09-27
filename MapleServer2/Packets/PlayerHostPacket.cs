﻿using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class PlayerHostPacket
    {
        private enum PlayerHostPacketMode : byte
        {
            OpenHongbao = 0x0,
            HongbaoGiftNotice = 0x2,
            StartMinigame = 0x3,
            MinigameRewardNotice = 0x4,
            MinigameRewardReceive = 0x5,
            AdBalloonWindow = 0x6,
            AdBalloonPlace = 0x7,
        }
        public static Packet OpenHongbao(Player player, HongBao hongBao)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PLAYER_HOST);
            pWriter.WriteEnum(PlayerHostPacketMode.OpenHongbao);
            pWriter.WriteInt(hongBao.ItemId);
            pWriter.WriteInt(hongBao.Id);
            pWriter.WriteInt(hongBao.RewardId);
            pWriter.WriteInt(hongBao.RewardAmount);
            pWriter.WriteInt(hongBao.MaxReceivers);
            pWriter.WriteUnicodeString(player.Name);
            return pWriter;
        }

        public static Packet HongbaoGiftNotice(Player receiver, HongBao hongBao, int dividedRewardAmount)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PLAYER_HOST);
            pWriter.WriteEnum(PlayerHostPacketMode.HongbaoGiftNotice);
            pWriter.WriteBool(hongBao.Active);
            if (hongBao.Active)
            {
                pWriter.WriteInt(hongBao.ItemId);
                pWriter.WriteInt(hongBao.RewardId);
                pWriter.WriteInt(dividedRewardAmount);
                pWriter.WriteUnicodeString(hongBao.Giver.Name);
                pWriter.WriteUnicodeString(receiver.Name);
            }
            return pWriter;
        }

        public static Packet StartMinigame(Player player, int minigameId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PLAYER_HOST);
            pWriter.WriteEnum(PlayerHostPacketMode.StartMinigame);
            pWriter.WriteUnicodeString(player.Name);
            pWriter.WriteInt(minigameId);
            return pWriter;
        }

        public static Packet MinigameRewardNotice(int minigameId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PLAYER_HOST);
            pWriter.WriteEnum(PlayerHostPacketMode.StartMinigame);
            pWriter.WriteInt(minigameId);
            pWriter.WriteInt(); // amount of players in map
            return pWriter;
        }

        public static Packet MinigameRewardReceive(int minigameId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PLAYER_HOST);
            pWriter.WriteEnum(PlayerHostPacketMode.StartMinigame);
            pWriter.WriteInt(minigameId);
            pWriter.WriteInt(); // amount of players in map
            return pWriter;
        }

        public static Packet AdBalloonWindow(AdBalloon balloon)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PLAYER_HOST);
            pWriter.WriteEnum(PlayerHostPacketMode.AdBalloonWindow);
            pWriter.WriteLong(balloon.Owner.AccountId);
            pWriter.WriteLong(balloon.Owner.CharacterId);
            pWriter.WriteUnicodeString(balloon.Owner.Name);
            pWriter.WriteUnicodeString(balloon.Owner.Name);
            pWriter.WriteShort(balloon.Owner.Levels.Level);
            pWriter.WriteInt();
            pWriter.WriteUnicodeString(balloon.Title);
            pWriter.WriteUnicodeString(balloon.Description);
            pWriter.WriteBool(balloon.PublicHouse);
            pWriter.WriteLong(balloon.CreationTimestamp);
            pWriter.WriteLong(balloon.ExpirationTimestamp);
            pWriter.WriteLong();
            return pWriter;
        }

        public static Packet AdBalloonPlace()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PLAYER_HOST);
            pWriter.WriteEnum(PlayerHostPacketMode.AdBalloonPlace);
            pWriter.WriteInt();
            return pWriter;
        }
    }
}
