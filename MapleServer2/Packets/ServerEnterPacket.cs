using System.Collections.Generic;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Servers.Game;

namespace MapleServer2.Packets
{
    public static class ServerEnterPacket
    {
        public static Packet Enter(GameSession session)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.SERVER_ENTER);
            pWriter.WriteInt(session.FieldPlayer.ObjectId);
            pWriter.WriteLong(session.Player.CharacterId);
            pWriter.WriteShort(1); // channel
            pWriter.WriteLong(session.Player.Exp.PlayerExp);
            pWriter.WriteLong(session.Player.Exp.RestExp);
            pWriter.WriteLong(session.Player.Wallet.Meso.Amount);

            pWriter.WriteLong(); // Total Merets
            pWriter.WriteLong(session.Player.Wallet.Meret.Amount); // Merets
            pWriter.WriteLong(session.Player.Wallet.GameMeret.Amount); // Game Merets
            pWriter.WriteLong(session.Player.Wallet.EventMeret.Amount); // Event Merets

            pWriter.WriteLong();

            pWriter.WriteLong(session.Player.Wallet.ValorToken.Amount);
            pWriter.WriteLong(session.Player.Wallet.Treva.Amount);
            pWriter.WriteLong(session.Player.Wallet.Rue.Amount);
            pWriter.WriteLong(session.Player.Wallet.HaviFruit.Amount);
            pWriter.WriteLong();
            pWriter.WriteLong();
            pWriter.WriteLong();
            pWriter.WriteLong();
            pWriter.WriteLong(session.Player.Wallet.MesoToken.Amount);
            pWriter.WriteUnicodeString(""); // Profile Url
            pWriter.WriteByte();
            pWriter.WriteByte();
            // REQUIRED OR CRASH
            // Unlocked Hidden Maps (Not on WorldMap)
            List<int> unlockedHiddenMaps = new List<int> { 52000065 };
            pWriter.WriteShort((short) unlockedHiddenMaps.Count);
            foreach (int mapId in unlockedHiddenMaps)
            {
                pWriter.WriteInt(mapId);
            }
            // Unlocked Maps (On WorldMap)
            List<int> unlockedMaps = new List<int> { 2000062 };
            pWriter.WriteShort((short) unlockedMaps.Count);
            foreach (int mapId in unlockedMaps)
            {
                pWriter.WriteInt(mapId);
            }
            pWriter.WriteLong();
            pWriter.WriteUnicodeString("");
            pWriter.WriteUnicodeString("http://nxcache.nexon.net/maplestory2/maplenews/index.html");
            pWriter.WriteUnicodeString("");
            pWriter.WriteUnicodeString(@"^https?://test-nxcache\.nexon\.net ^https?://nxcache\.nexon\.net");
            pWriter.WriteUnicodeString("");


            return pWriter;
        }

        public static Packet Confirm()
        {
            return PacketWriter.Of(SendOp.FINALIZE_SERVER_ENTER);
        }
    }
}
