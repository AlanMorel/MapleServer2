using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Enums;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public class ResponseLoadUGCMapPacket
    {
        public static Packet LoadUGCMap(bool isHome, Home home = null)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.LOAD_UGC_MAP);
            pWriter.WriteLong();
            pWriter.WriteBool(isHome);

            if (isHome)
            {
                pWriter.WriteLong(home.AccountId);
                pWriter.WriteUnicodeString(home.Name);
                pWriter.WriteUnicodeString(home.Description);
                pWriter.WriteByte();
                pWriter.WriteInt(home.ArchitectScoreCurrent);
                pWriter.WriteInt(home.ArchitectScoreTotal);
                pWriter.WriteInt();
                pWriter.WriteInt();
                pWriter.WriteByte();
                pWriter.WriteByte(home.Size);
                pWriter.WriteByte(home.Height);
                pWriter.WriteByte(home.Background);
                pWriter.WriteByte(home.Lighting);
                pWriter.WriteByte(home.Camera);
                pWriter.WriteByte((byte) home.Permissions.Count);
                for (int i = 0; i < 9; i++) // permissions
                {
                    if (home.Permissions.ContainsKey((HomePermission) i))
                    {
                        pWriter.WriteBool(true);
                        pWriter.WriteByte(home.Permissions[(HomePermission) i]);
                    }
                    else
                    {
                        pWriter.WriteBool(false);
                    }
                }
                pWriter.WriteShort();
            }

            return pWriter;
        }
    }
}
