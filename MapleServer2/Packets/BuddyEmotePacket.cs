using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class BuddyEmotePacket
{
    private enum Mode : byte
    {
        SendRequest = 0x0,
        ConfirmSendRequest = 0x1,
        LearnEmote = 0x2,
        AcceptEmote = 0x3,
        DeclineEmote = 0x4,
        StartEmote = 0x5,
        StopEmote = 0x6
    }

    public static PacketWriter SendRequest(int buddyEmoteId, Player sender)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.BuddyEmote);
        pWriter.Write(Mode.SendRequest);
        pWriter.WriteInt(buddyEmoteId);
        pWriter.WriteLong(sender.CharacterId);
        pWriter.WriteUnicodeString(sender.Name);
        return pWriter;
    }

    public static PacketWriter ConfirmSendRequest(Player player)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.BuddyEmote);
        pWriter.Write(Mode.ConfirmSendRequest);
        pWriter.WriteLong(player.CharacterId);
        return pWriter;
    }

    public static PacketWriter LearnEmote()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.BuddyEmote);
        pWriter.Write(Mode.LearnEmote);
        pWriter.WriteInt(); // emoteID
        pWriter.WriteInt(1); // quantity
        pWriter.WriteLong();
        return pWriter;
    }

    public static PacketWriter SendAccept(int buddyEmoteId, Player player)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.BuddyEmote);
        pWriter.Write(Mode.AcceptEmote);
        pWriter.WriteInt(buddyEmoteId);
        pWriter.WriteLong(player.CharacterId);
        return pWriter;
    }

    public static PacketWriter DeclineEmote(int buddyEmoteId, Player player)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.BuddyEmote);
        pWriter.Write(Mode.DeclineEmote);
        pWriter.WriteInt(buddyEmoteId);
        pWriter.WriteLong(player.CharacterId);
        return pWriter;
    }

    public static PacketWriter StartEmote(int buddyEmoteId, Player player1, Player player2, CoordF coords, int rotation)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.BuddyEmote);
        pWriter.Write(Mode.StartEmote);
        pWriter.WriteInt(buddyEmoteId);
        pWriter.WriteLong(player1.CharacterId);
        pWriter.WriteLong(player2.CharacterId);
        pWriter.Write(coords);
        pWriter.WriteLong();
        pWriter.Write(rotation);

        pWriter.WriteInt();
        return pWriter;
    }

    public static PacketWriter StopEmote(int buddyEmoteId, Player player)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.BuddyEmote);
        pWriter.Write(Mode.StopEmote);
        pWriter.WriteInt(buddyEmoteId);
        pWriter.WriteLong(player.CharacterId);
        return pWriter;
    }
}
