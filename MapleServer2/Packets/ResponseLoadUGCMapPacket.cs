using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Enums;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public class ResponseLoadUGCMapPacket
    {
        public static Packet LoadUGCMap(bool isHouse, Home home = null)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.LOAD_UGC_MAP);
            pWriter.WriteLong();
            pWriter.WriteBool(isHouse);

            if (isHouse)
            {
                pWriter.WriteLong(home.AccountId);
                pWriter.WriteUnicodeString(home.Name);
                pWriter.WriteUnicodeString(home.Description); // description
                pWriter.WriteByte();
                pWriter.WriteInt(); //archiect score
                pWriter.WriteInt(); //total score
                pWriter.WriteInt();
                pWriter.WriteInt();
                pWriter.WriteByte();
                pWriter.WriteByte(home.Size); // house size
                pWriter.WriteByte(home.Height); // height
                pWriter.WriteByte(home.Background); //background option
                pWriter.WriteByte(home.Lighting); //lighthing option
                pWriter.WriteByte(home.Camera); //camera option
                pWriter.WriteByte(9);
                for (int i = 0; i < 9; i++)
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
