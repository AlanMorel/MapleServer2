using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class ServerEnterPacket
{
    public static PacketWriter Enter(GameSession session)
    {
        Player player = session.Player;
        Account account = player.Account;
        Wallet wallet = player.Wallet;

        PacketWriter pWriter = PacketWriter.Of(SendOp.ServerEnter);
        pWriter.WriteInt(player.FieldPlayer.ObjectId);
        pWriter.WriteLong(player.CharacterId);
        pWriter.WriteShort(player.ChannelId);
        pWriter.WriteLong(player.Levels.Exp);
        pWriter.WriteLong(player.Levels.RestExp);
        pWriter.WriteLong(wallet.Meso.Amount);

        pWriter.WriteLong(); // Total Merets
        pWriter.WriteLong(account.Meret.Amount); // Merets
        pWriter.WriteLong(account.GameMeret.Amount); // Game Merets
        pWriter.WriteLong(account.EventMeret.Amount); // Event Merets

        pWriter.WriteLong(); // merets?

        pWriter.WriteLong(wallet.ValorToken.Amount);
        pWriter.WriteLong(wallet.Treva.Amount);
        pWriter.WriteLong(wallet.Rue.Amount);
        pWriter.WriteLong(wallet.HaviFruit.Amount);
        pWriter.WriteLong(); // reverse coin
        pWriter.WriteLong(); // mentor
        pWriter.WriteLong(); // mentee
        pWriter.WriteLong(); // star point
        pWriter.WriteLong(account.MesoToken.Amount);
        pWriter.WriteUnicodeString(player.ProfileUrl); // Profile Url
        pWriter.WriteByte();
        pWriter.WriteByte();

        // Unlocked Maps (World Map)
        List<int> unlockedMaps = player.UnlockedMaps;
        pWriter.WriteShort((short) unlockedMaps.Count);
        foreach (int mapId in unlockedMaps)
        {
            pWriter.WriteInt(mapId);
        }

        // Unlocked Taxis
        List<int> unlockedTaxis = player.UnlockedTaxis;
        pWriter.WriteShort((short) unlockedTaxis.Count);
        foreach (int mapId in unlockedTaxis)
        {
            pWriter.WriteInt(mapId);
        }

        pWriter.WriteLong();
        pWriter.WriteUnicodeString();
        pWriter.WriteUnicodeString("https://github.com/AlanMorel/MapleServer2");
        pWriter.WriteUnicodeString();
        pWriter.WriteUnicodeString();
        pWriter.WriteUnicodeString();

        return pWriter;
    }
}
