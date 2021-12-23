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

        PacketWriter pWriter = PacketWriter.Of(SendOp.SERVER_ENTER);
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

        pWriter.WriteLong();

        pWriter.WriteLong(wallet.ValorToken.Amount);
        pWriter.WriteLong(wallet.Treva.Amount);
        pWriter.WriteLong(wallet.Rue.Amount);
        pWriter.WriteLong(wallet.HaviFruit.Amount);
        pWriter.WriteLong();
        pWriter.WriteLong();
        pWriter.WriteLong();
        pWriter.WriteLong();
        pWriter.WriteLong(account.MesoToken.Amount);
        pWriter.WriteUnicodeString(player.ProfileUrl); // Profile Url
        pWriter.WriteByte();
        pWriter.WriteByte();
        // REQUIRED OR CRASH

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
        pWriter.WriteUnicodeString("http://nxcache.nexon.net/maplestory2/maplenews/index.html");
        pWriter.WriteUnicodeString();
        pWriter.WriteUnicodeString(@"^https?://test-nxcache\.nexon\.net ^https?://nxcache\.nexon\.net");
        pWriter.WriteUnicodeString();


        return pWriter;
    }

    public static PacketWriter Confirm()
    {
        return PacketWriter.Of(SendOp.FINALIZE_SERVER_ENTER);
    }
}
