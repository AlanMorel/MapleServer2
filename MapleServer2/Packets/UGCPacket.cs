using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class UgcPacket
{
    private enum UgcMode : byte
    {
        CreateUgc = 0x02,
        SetItemUrl = 0x04,
        ProfilePicture = 0x0B,
        UpdateUgc = 0x0D,
        SetEndpoint = 0x11,
        Mode12 = 0x12,
    }

    public static PacketWriter SetEndpoint(string uploadEndpoint, string resourceEndpoint, string locale = "na")
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.UGC);
        pWriter.Write(UgcMode.SetEndpoint); // Function
        pWriter.WriteUnicodeString(uploadEndpoint); // Serves some random irrq.aspx
        pWriter.WriteUnicodeString(resourceEndpoint); // Serves resources
        pWriter.WriteUnicodeString(locale); // locale
        pWriter.WriteByte(2);

        return pWriter;
    }

    public static PacketWriter Unknown0()
    {
        // SO MANY CASES...
        return null;
    }

    public static PacketWriter CreateUgc(bool success, UGC ugc)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.UGC);
        pWriter.Write(UgcMode.CreateUgc);
        pWriter.WriteBool(success);
        if (!success)
        {
            return pWriter;
        }

        pWriter.WriteLong(ugc.Uid);
        pWriter.WriteUnicodeString(ugc.Guid.ToString());

        return pWriter;
    }

    public static PacketWriter SetItemUrl(UGC ugc)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.UGC);
        pWriter.Write(UgcMode.SetItemUrl);
        pWriter.WriteBool(ugc is not null);
        if (ugc is null)
        {
            return pWriter;
        }

        pWriter.WriteLong(ugc.Uid);
        pWriter.WriteUnicodeString(ugc.Url);

        return pWriter;
    }

    public static PacketWriter Unknown7()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.UGC);
        pWriter.WriteByte(0x07);
        pWriter.WriteLong();
        pWriter.WriteByte(); // condition
        // If byte == 1
        SharedSubUgc(pWriter);
        // EndIf
        // ???

        return pWriter;
    }

    public static PacketWriter Unknown8()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.UGC);
        pWriter.WriteByte(0x08);
        SharedSubUgc2(pWriter);

        return pWriter;
    }

    public static PacketWriter Unknown9()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.UGC);
        pWriter.WriteByte(0x09);
        pWriter.WriteLong();
        pWriter.WriteInt(); // counter for loop
        for (int i = 0; i < 0; i++)
        {
            pWriter.WriteLong();
        }

        return pWriter;
    }

    public static PacketWriter SetProfilePictureUrl(int objectId, long characterId, string url)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.UGC);
        pWriter.Write(UgcMode.ProfilePicture);
        pWriter.WriteInt(objectId);
        pWriter.WriteLong(characterId);
        pWriter.WriteUnicodeString(url);

        return pWriter;
    }

    // not sure about the name
    public static PacketWriter UpdateUgcItem(IFieldObject<Player> fieldPlayer, Item item)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.UGC);
        pWriter.Write(UgcMode.UpdateUgc);
        pWriter.WriteInt(fieldPlayer.ObjectId);
        pWriter.WriteLong(item.Uid);
        pWriter.WriteInt(item.Id);
        pWriter.WriteInt(item.Amount);
        pWriter.WriteUnicodeString(item.UGC.Name);
        pWriter.WriteByte(1); // unknown
        pWriter.WriteLong(item.UGC.SalePrice);
        pWriter.WriteBool(false); // unknown
        pWriter.WriteUgcTemplate(item.UGC);

        return pWriter;
    }

    public static PacketWriter Unknown13To15()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.UGC);
        pWriter.WriteByte(0x0D); // Also 0x0E, 0x0F
        pWriter.WriteInt();
        // sub1
        pWriter.WriteLong();
        pWriter.WriteInt();
        pWriter.WriteInt();
        pWriter.WriteUnicodeString("StrW");
        pWriter.WriteByte();
        pWriter.WriteLong();
        pWriter.WriteBool(false);
        // sub2
        pWriter.WriteUgcTemplate(null);

        return pWriter;
    }

    public static PacketWriter Unknown16()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.UGC);
        pWriter.WriteByte(0x10);
        pWriter.WriteInt();
        // sub1
        pWriter.WriteLong();
        pWriter.WriteLong();
        pWriter.WriteInt();
        pWriter.WriteUnicodeString("StrW");
        // sub2
        pWriter.WriteUgcTemplate(null);

        return pWriter;
    }

    public static PacketWriter Unknown17()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.UGC);
        pWriter.WriteByte(0x11);
        pWriter.WriteUnicodeString("WstrA");
        pWriter.WriteUnicodeString("WstrA");
        pWriter.WriteUnicodeString("WstrA");
        pWriter.WriteByte();

        return pWriter;
    }

    public static PacketWriter Mode12()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.UGC);
        pWriter.Write(UgcMode.Mode12);

        int counter1 = 0;
        pWriter.WriteInt(counter1);
        for (int i = 0; i < counter1; i++)
        {
            bool flagA = false;
            pWriter.WriteBool(flagA);
            if (flagA)
            {
                pWriter.WriteLong();
                pWriter.WriteUnicodeString();
                pWriter.WriteByte();
                pWriter.WriteInt();
                pWriter.WriteLong();
                pWriter.WriteLong();
                pWriter.WriteUnicodeString();
                pWriter.WriteUnicodeString();
                pWriter.WriteUnicodeString();
            }
        }

        int counter2 = 0;
        pWriter.WriteInt(counter2);
        for (int i = 0; i < counter2; i++)
        {
            bool flagB = false;
            pWriter.WriteLong();
            pWriter.WriteBool(flagB);
            if (flagB)
            {
                pWriter.WriteByte();
                pWriter.WriteInt();
                pWriter.WriteLong();
                pWriter.WriteLong();
                pWriter.WriteUnicodeString();
                pWriter.WriteUnicodeString();
                pWriter.WriteLong();
                pWriter.WriteUnicodeString();
                pWriter.WriteByte();
                pWriter.WriteByte();
                pWriter.WriteLong();
                pWriter.WriteByte();
                pWriter.WriteUnicodeString();
            }
        }

        int counter3 = 0;
        pWriter.WriteInt(counter3);
        for (int i = 0; i < counter3; i++)
        {
            SharedSubUgc2(pWriter);
        }

        return pWriter;
    }

    public static PacketWriter Unknown20()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.UGC);
        pWriter.WriteByte(0x14);
        pWriter.WriteLong();
        pWriter.WriteInt(); // some count for loop
        for (int i = 0; i < 0; i++)
        {
            pWriter.WriteLong();
            pWriter.WriteInt();
            pWriter.WriteLong();
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteLong();
        }

        return pWriter;
    }

    public static PacketWriter Unknown21()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.UGC);
        pWriter.WriteByte(0x15);
        pWriter.WriteInt(); // some count for loop
        for (int i = 0; i < 0; i++)
        {
            pWriter.WriteLong();
            pWriter.WriteInt();
        }

        return pWriter;
    }

    public static PacketWriter Unknown22()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.UGC);
        pWriter.WriteByte(0x16);
        pWriter.WriteInt();

        return pWriter;
    }

    public static void WriteUgcTemplate(this PacketWriter pWriter, UGC ugc)
    {
        pWriter.WriteLong(ugc?.Uid ?? 0);
        pWriter.WriteUnicodeString(ugc?.Guid.ToString() ?? ""); // UUID (filename)
        pWriter.WriteUnicodeString(ugc?.Name ?? ""); // Name (itemname)
        pWriter.WriteByte(1);
        pWriter.WriteInt(1);
        pWriter.WriteLong(ugc?.AccountId ?? 0); // AccountId
        pWriter.WriteLong(ugc?.CharacterId ?? 0); // CharacterId
        pWriter.WriteUnicodeString(ugc?.CharacterName ?? ""); // CharacterName
        pWriter.WriteLong(ugc?.CreationTime ?? 0); // CreationTime
        pWriter.WriteUnicodeString(ugc?.Url ?? ""); // URL (no domain)
        pWriter.WriteByte();
    }

    private static void SharedSubUgc(PacketWriter pWriter)
    {
        pWriter.WriteByte();
        pWriter.WriteUnicodeString("WstrA");
        // unknown call to invalid memory using packet
    }

    private static void SharedSubUgc2(PacketWriter pWriter)
    {
        pWriter.WriteLong();
        pWriter.WriteInt(); // counter for loop
        for (int i = 0; i < 0; i++)
        {
            pWriter.WriteLong();
            pWriter.WriteUnicodeString("StrW");
            // unknown call to invalid memory using packet
            pWriter.WriteByte();
        }
    }
}
