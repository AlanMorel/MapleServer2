using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets.Helpers;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
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
            StartOpen = 0x10,
            EndOpen = 0x11,
            Error = 0x14,
        }

        public static Packet Open(List<Mail> box)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.MAIL);
            pWriter.WriteEnum(MailPacketMode.Open);
            pWriter.WriteInt(box.Count);
            foreach (Mail mail in box)
            {
                WriteMail(pWriter, mail);
            }
            return pWriter;
        }

        public static Packet Send(Mail mail)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.MAIL);
            pWriter.WriteEnum(MailPacketMode.Send);
            pWriter.WriteLong(mail.Id);
            return pWriter;
        }

        public static Packet Read(Mail mail)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.MAIL);
            pWriter.WriteEnum(MailPacketMode.Read);
            pWriter.WriteLong(mail.Id);
            pWriter.WriteLong(mail.ReadTimestamp);
            return pWriter;
        }

        public static Packet Collect(Mail mail)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.MAIL);
            pWriter.WriteEnum(MailPacketMode.Collect);
            pWriter.WriteLong(mail.Id);
            pWriter.WriteByte(1);
            pWriter.WriteByte();
            pWriter.WriteLong(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            return pWriter;
        }

        public static Packet UpdateReadTime(Mail mail)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.MAIL);
            pWriter.WriteEnum(MailPacketMode.UpdateReadTime);
            pWriter.WriteLong(mail.Id);
            pWriter.WriteLong(mail.ReadTimestamp);
            return pWriter;
        }

        public static Packet Delete(Mail mail)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.MAIL);
            pWriter.WriteEnum(MailPacketMode.Delete);
            pWriter.WriteLong(mail.Id);
            return pWriter;
        }

        public static Packet Notify(int unreadCount, bool alert = false)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.MAIL);
            pWriter.WriteEnum(MailPacketMode.Notify);
            pWriter.WriteInt(unreadCount);
            pWriter.WriteBool(alert);
            pWriter.WriteInt(); // Unknown maybe repeat of count?
            return pWriter;
        }

        public static Packet StartOpen()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.MAIL);
            pWriter.WriteEnum(MailPacketMode.StartOpen);
            return pWriter;
        }

        public static Packet EndOpen()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.MAIL);
            pWriter.WriteEnum(MailPacketMode.EndOpen);
            return pWriter;
        }

        public static Packet Error(byte errorCode)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.MAIL);
            pWriter.WriteEnum(MailPacketMode.Error);
            pWriter.WriteByte(0x1);
            pWriter.WriteByte(errorCode);
            return pWriter;
        }

        public static PacketWriter WriteMail(PacketWriter pWriter, Mail mail)
        {
            pWriter.WriteEnum(mail.Type);
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
            pWriter.WriteLong();
            pWriter.WriteLong();
            pWriter.WriteLong();
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
            pWriter.WriteLong(mail.ReadTimestamp > 0 ? mail.ReadTimestamp + Environment.TickCount : 0);
            pWriter.WriteLong(mail.ExpiryTimestamp);
            pWriter.WriteLong(mail.SentTimestamp);
            pWriter.WriteUnicodeString("");
            return pWriter;
        }
    }
}
