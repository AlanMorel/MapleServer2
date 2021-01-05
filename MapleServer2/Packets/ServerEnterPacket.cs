using System.Collections.Generic;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Servers.Game;

namespace MapleServer2.Packets {
    public static class ServerEnterPacket {
        public static Packet Enter(GameSession session) {
            var pWriter = PacketWriter.Of(SendOp.SERVER_ENTER);
            pWriter.WriteInt(session.FieldPlayer.ObjectId);
            pWriter.WriteLong(session.Player.CharacterId);
            pWriter.WriteShort(1); // channel
            pWriter.WriteLong(session.Player.Experience);
            pWriter.WriteLong(session.Player.RestExperience);
            pWriter.WriteLong(session.Player.Mesos);

            pWriter.WriteLong(session.Player.Merets); // Merets
            pWriter.WriteLong(); // Merets
            // These Merets are added up. If set, previous are ignored.

            pWriter.WriteLong(); // Game Merets
            pWriter.WriteLong(); // Event Merets

            pWriter.WriteLong();

            pWriter.WriteLong(session.Player.ValorToken);
            pWriter.WriteLong(session.Player.Treva);
            pWriter.WriteLong(session.Player.Rue);
            pWriter.WriteLong(session.Player.HaviFruit);
            pWriter.WriteLong();
            pWriter.WriteLong();
            pWriter.WriteLong();
            pWriter.WriteLong();
            pWriter.WriteLong(session.Player.MesoToken);
            pWriter.WriteUnicodeString(""); // Profile Url
            pWriter.WriteByte();
            pWriter.WriteByte();
            // REQUIRED OR CRASH
            // Unlocked Hidden Maps (Not on WorldMap)
            List<int> unlockedHiddenMaps = new List<int> { 52000065 };
            pWriter.WriteShort((short)unlockedHiddenMaps.Count);
            foreach (int mapId in unlockedHiddenMaps) {
                pWriter.WriteInt(mapId);
            }
            // Unlocked Maps (On WorldMap)
            List<int> unlockedMaps = new List<int> { 2000062 };
            pWriter.WriteShort((short)unlockedMaps.Count);
            foreach (int mapId in unlockedMaps) {
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

        public static Packet Confirm() {
            return PacketWriter.Of(SendOp.FINALIZE_SERVER_ENTER);
        }
    }
}
