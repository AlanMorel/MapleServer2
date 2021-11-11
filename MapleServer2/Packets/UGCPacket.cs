﻿using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class UgcPacket
{
    public enum UgcMode : byte
    {
        CreateUGC = 0x02,
        SetItemURL = 0x04,
        ProfilePicture = 0x0B,
        UpdateUGC = 0x0D,
        SetEndpoint = 0x11,
    }

    public static PacketWriter SetEndpoint(string unknownEndpoint, string resourceEndpoint, string locale = "na")
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.UGC);
        pWriter.Write(UgcMode.SetEndpoint); // Function
        pWriter.WriteUnicodeString(unknownEndpoint); // Serves some random irrq.aspx
        pWriter.WriteUnicodeString(resourceEndpoint); // Serves resources
        pWriter.WriteUnicodeString(locale); // locale?
        pWriter.WriteByte(2);

        return pWriter;
    }

    public static PacketWriter Unknown0()
    {
        // SO MANY CASES...
        return null;
    }

    public static PacketWriter CreateUGC(bool success, UGC ugc)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.UGC);
        pWriter.Write(UgcMode.CreateUGC);
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
        pWriter.Write(UgcMode.SetItemURL);
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
        SharedSubUGC(pWriter);
        // EndIf
        // ???

        return pWriter;
    }

    public static PacketWriter Unknown8()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.UGC);
        pWriter.WriteByte(0x08);
        SharedSubUGC2(pWriter);

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

    public static PacketWriter SetProfilePictureURL(int objectId, long characterId, string url)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.UGC);
        pWriter.Write(UgcMode.ProfilePicture);
        pWriter.WriteInt(objectId);
        pWriter.WriteLong(characterId);
        pWriter.WriteUnicodeString(url);

        return pWriter;
    }

    // not sure about the name
    public static PacketWriter UpdateUGCItem(IFieldObject<Player> fieldPlayer, Item item)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.UGC);
        pWriter.Write(UgcMode.UpdateUGC);
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

    public static PacketWriter Unknown18()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.UGC);
        pWriter.WriteByte(0x12);
        // sub1
        pWriter.WriteInt();
        pWriter.WriteByte(); // condition
        // If condition
        pWriter.WriteLong();
        pWriter.WriteUnicodeString("StrW");
        // unknown call to invalid memory using packet
        pWriter.WriteByte();
        pWriter.WriteInt();
        pWriter.WriteLong();
        pWriter.WriteLong();
        pWriter.WriteUnicodeString("WstrA");
        // unknown call to invalid memory using packet
        pWriter.WriteUnicodeString("StrW");
        // unknown call to invalid memory using packet
        pWriter.WriteUnicodeString("StrW");
        // EndIf

        // One some random condition jump to this block
        pWriter.WriteInt(); // counter for loop
        for (int i = 0; i < 0; i++)
        {
            pWriter.WriteLong();
            pWriter.WriteByte();
            // If some condition (can't read)
            SharedSubUGC(pWriter);
            // EndIf
        }
        pWriter.WriteInt(); // counter for loop
        for (int i = 0; i < 0; i++)
        {
            SharedSubUGC2(pWriter);
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

    public static PacketWriter WriteUgcTemplate(this PacketWriter pWriter, UGC ugc)
    {
        pWriter.WriteLong(ugc.Uid);
        pWriter.WriteUnicodeString(ugc.Guid.ToString()); // UUID (filename)
        pWriter.WriteUnicodeString(ugc.Name); // Name (itemname)
        pWriter.WriteByte(1);
        pWriter.WriteInt(1);
        pWriter.WriteLong(ugc.AccountId); // AccountId
        pWriter.WriteLong(ugc.CharacterId); // CharacterId
        pWriter.WriteUnicodeString(ugc.CharacterName); // CharacterName
        pWriter.WriteLong(ugc.CreationTime); // CreationTime
        pWriter.WriteUnicodeString(ugc.Url); // URL (no domain)
        pWriter.WriteByte();

        return pWriter;
    }

    private static void SharedSubUGC(PacketWriter pWriter)
    {
        pWriter.WriteByte();
        pWriter.WriteUnicodeString("WstrA");
        // unknown call to invalid memory using packet
    }

    private static void SharedSubUGC2(PacketWriter pWriter)
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
