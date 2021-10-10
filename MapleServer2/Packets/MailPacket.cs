using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets.Helpers;
using MapleServer2.Servers.Game;
using MapleServer2.Types;
using static MapleServer2.Types.Mail;

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

        public static Packet Test()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.MAIL);
            pWriter.WriteEnum(MailPacketMode.Open);
            pWriter.WriteHexString("01 00 00 00 65 A8 F9 E9 03 00 00 00 00 00 00 00 00 00 00 00 00 32 00 3C 00 6D 00 73 00 32 00 3E 00 3C 00 76 00 20 00 6B 00 65 00 79 00 3D 00 22 00 73 00 5F 00 63 00 6F 00 75 00 70 00 6C 00 65 00 5F 00 65 00 66 00 66 00 65 00 63 00 74 00 5F 00 6D 00 61 00 69 00 6C 00 5F 00 73 00 65 00 6E 00 64 00 65 00 72 00 22 00 20 00 2F 00 3E 00 3C 00 2F 00 6D 00 73 00 32 00 3E 00 3A 00 3C 00 6D 00 73 00 32 00 3E 00 3C 00 76 00 20 00 6B 00 65 00 79 00 3D 00 22 00 73 00 5F 00 63 00 6F 00 75 00 70 00 6C 00 65 00 5F 00 65 00 66 00 66 00 65 00 63 00 74 00 5F 00 6D 00 61 00 69 00 6C 00 5F 00 74 00 69 00 74 00 6C 00 65 00 5F 00 72 00 65 00 63 00 65 00 69 00 76 00 65 00 72 00 22 00 20 00 2F 00 3E 00 3C 00 2F 00 6D 00 73 00 32 00 3E 00 3C 00 3C 00 6D 00 73 00 32 00 3E 00 3C 00 76 00 20 00 6B 00 65 00 79 00 3D 00 22 00 73 00 5F 00 63 00 6F 00 75 00 70 00 6C 00 65 00 5F 00 65 00 66 00 66 00 65 00 63 00 74 00 5F 00 6D 00 61 00 69 00 6C 00 5F 00 63 00 6F 00 6E 00 74 00 65 00 6E 00 74 00 5F 00 72 00 65 00 63 00 65 00 69 00 76 00 65 00 72 00 22 00 20 00 2F 00 3E 00 3C 00 2F 00 6D 00 73 00 32 00 3E 00 00 00 20 00 3C 00 6D 00 73 00 32 00 3E 00 3C 00 76 00 20 00 73 00 74 00 72 00 3D 00 22 00 41 00 64 00 6F 00 6E 00 69 00 73 00 22 00 20 00 3E 00 3C 00 2F 00 76 00 3E 00 3C 00 2F 00 6D 00 73 00 32 00 3E 00 01 83 52 38 04 67 27 3D 65 09 99 BF 29 00 04 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 00 00 00 00 FF FF FF FF 0C 5A BE 5E 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 07 08 00 37 00 30 00 38 00 30 00 30 00 30 00 30 00 33 00 08 00 00 00 00 00 00 00 00 00 00 00 00 00 01 01 C7 98 41 71 DC 15 30 28 09 00 4D 00 65 00 72 00 65 00 76 00 79 00 6C 00 6E 00 6E 00 00 00 B0 2E 7E A9 23 13 38 28 06 00 41 00 64 00 6F 00 6E 00 69 00 73 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 F7 20 86 61 00 00 00 00 F7 93 5E 61 00 00 00 00 00 00");
            return pWriter;
        }

        public static Packet Send(Mail mail)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.MAIL);
            pWriter.WriteEnum(MailPacketMode.Send);
            pWriter.WriteLong(mail.Id);
            return pWriter;
        }

        public static Packet Notify(int unreadCount)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.MAIL);
            pWriter.WriteEnum(MailPacketMode.Notify);
            pWriter.WriteInt(unreadCount);
            pWriter.WriteByte(); // Unknown
            pWriter.WriteInt(); // Unknown maybe repeat of count?
            return pWriter;
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

        public static Packet CollectedAmount(long id, long timestamp)
        {
            // Not sure what the purpose of this packet is, perhaps if collect fails?
            PacketWriter pWriter = PacketWriter.Of(SendOp.MAIL);

            pWriter.WriteByte(10); // Mode for collect success/failure? collect mode = 11
            pWriter.WriteLong(id);
            pWriter.WriteShort(1); // Successfully collected? 01 00
            pWriter.WriteLong(timestamp + Environment.TickCount);

            return pWriter;
        }

        public static Packet CollectResponse(long id, long timestamp)
        {
            // Collect response packet
            PacketWriter pWriter = PacketWriter.Of(SendOp.MAIL);

            pWriter.WriteByte(11); // Mode for collect
            pWriter.WriteLong(id);
            pWriter.WriteLong(timestamp + Environment.TickCount); // Collect timestamp

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

            pWriter.WriteLong();
            pWriter.WriteLong();
            pWriter.WriteLong();
            pWriter.WriteLong();
            pWriter.WriteLong();
            pWriter.WriteLong();

            bool idk2 = false;
            pWriter.WriteBool(idk2);
            if (idk2)
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
