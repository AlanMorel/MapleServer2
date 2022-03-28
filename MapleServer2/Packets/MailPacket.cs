using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets.Helpers;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class MailPacket
{
    private enum MailPacketMode : byte
    {
        Open = 0x0,
        Send = 0x1,
        Read = 0x2,
        Collect = 0xA,
        UpdateReadTime = 0xB,
        Delete = 0xD,
        Notify = 0xE,
        ExpireNotification = 0xF,
        StartOpen = 0x10,
        EndOpen = 0x11,
        Error = 0x14
    }

    public static PacketWriter Open(List<Mail> box)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Mail);
        pWriter.Write(MailPacketMode.Open);
        pWriter.WriteInt(box.Count);
        foreach (Mail mail in box)
        {
            WriteMail(pWriter, mail);
        }
        return pWriter;
    }

    public static PacketWriter Send(Mail mail)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Mail);
        pWriter.Write(MailPacketMode.Send);
        pWriter.WriteLong(mail.Id);
        return pWriter;
    }

    public static PacketWriter Read(Mail mail)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Mail);
        pWriter.Write(MailPacketMode.Read);
        pWriter.WriteLong(mail.Id);
        pWriter.WriteLong(mail.ReadTimestamp);
        return pWriter;
    }

    public static PacketWriter Collect(Mail mail)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Mail);
        pWriter.Write(MailPacketMode.Collect);
        pWriter.WriteLong(mail.Id);
        pWriter.WriteByte(1);
        pWriter.WriteByte();
        pWriter.WriteLong(TimeInfo.Now());
        return pWriter;
    }

    public static PacketWriter UpdateReadTime(Mail mail)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Mail);
        pWriter.Write(MailPacketMode.UpdateReadTime);
        pWriter.WriteLong(mail.Id);
        pWriter.WriteLong(mail.ReadTimestamp);
        return pWriter;
    }

    public static PacketWriter Delete(Mail mail)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Mail);
        pWriter.Write(MailPacketMode.Delete);
        pWriter.WriteLong(mail.Id);
        return pWriter;
    }

    public static PacketWriter Notify(int unreadCount, bool alert = false)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Mail);
        pWriter.Write(MailPacketMode.Notify);
        pWriter.WriteInt(unreadCount);
        pWriter.WriteBool(alert);
        pWriter.WriteInt(); // Unknown maybe repeat of count?
        return pWriter;
    }

    public static PacketWriter ExpireNotification()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Mail);
        pWriter.Write(MailPacketMode.ExpireNotification);
        return pWriter;
    }

    public static PacketWriter StartOpen()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Mail);
        pWriter.Write(MailPacketMode.StartOpen);
        return pWriter;
    }

    public static PacketWriter EndOpen()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Mail);
        pWriter.Write(MailPacketMode.EndOpen);
        return pWriter;
    }

    public static PacketWriter Error(byte errorCode)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Mail);
        pWriter.Write(MailPacketMode.Error);
        pWriter.WriteByte(0x1);
        pWriter.WriteByte(errorCode);
        return pWriter;
    }

    public static PacketWriter WriteMail(PacketWriter pWriter, Mail mail)
    {
        pWriter.Write(mail.Type);
        pWriter.WriteLong(mail.Id);
        pWriter.WriteLong(mail.SenderCharacterId);
        pWriter.WriteUnicodeString(mail.SenderName);
        pWriter.WriteUnicodeString(mail.Title);
        pWriter.WriteUnicodeString(mail.Body);
        pWriter.WriteUnicodeString(mail.AdditionalParameter1);
        pWriter.WriteUnicodeString(mail.AdditionalParameter2);

        pWriter.WriteByte((byte) mail.Items.Count);
        foreach (Item item in mail.Items)
        {
            pWriter.WriteInt(item.Id);
            pWriter.WriteLong(item.Uid);
            pWriter.WriteByte();
            pWriter.WriteInt(item.Rarity);
            pWriter.WriteInt(item.Amount);
            pWriter.WriteLong();
            pWriter.WriteInt();
            pWriter.WriteLong();
            pWriter.WriteItem(item);
        }

        pWriter.WriteLong(mail.Mesos);
        pWriter.WriteLong(); // last purchase timestamp?
        pWriter.WriteLong(mail.Merets);
        pWriter.WriteLong();
        pWriter.WriteLong(); // red meret
        pWriter.WriteLong();

        bool unk = false;
        pWriter.WriteBool(unk);
        if (unk)
        {
            pWriter.WriteByte();
            pWriter.WriteByte();
            pWriter.WriteLong();
            pWriter.WriteLong();
        }
        pWriter.WriteLong(mail.ReadTimestamp);
        pWriter.WriteLong(mail.ExpiryTimestamp);
        pWriter.WriteLong(mail.SentTimestamp);
        pWriter.WriteUnicodeString();
        return pWriter;
    }
}
