using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Database.Types;

namespace MapleServer2.Packets;

public static class BannerListPacket
{
    public static PacketWriter SetBanner(List<Banner> banners)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.BANNER_LIST);
        pWriter.WriteShort((short) banners.Count);
        foreach (Banner banner in banners)
        {
            pWriter.WriteInt(banner.Id);
            pWriter.WriteUnicodeString(banner.Name);
            pWriter.WriteUnicodeString(banner.Type.ToString());
            if (banner.Type == BannerType.left || banner.Type == BannerType.right)
            {
                pWriter.WriteUnicodeString(banner.SubType.ToString());
            }
            else
            {
                pWriter.WriteUnicodeString();
            }
            pWriter.WriteUnicodeString();
            pWriter.WriteUnicodeString(banner.ImageUrl);
            pWriter.Write(banner.Language);
            pWriter.WriteLong(banner.BeginTime);
            pWriter.WriteLong(banner.EndTime);
        }

        return pWriter;
    }
}
