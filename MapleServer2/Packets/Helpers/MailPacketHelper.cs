using MaplePacketLib2.Tools;
using MapleServer2.Types;

namespace MapleServer2.Packets.Helpers
{
    public static class MailPacketHelper
    {
        public static PacketWriter WriteSystem(PacketWriter pWriter, Mail mail)
        {
            pWriter.WriteEnum(mail.Type);
            pWriter.WriteLong(mail.Id);
            pWriter.WriteLong(mail.RecipientCharacterId);
            pWriter.WriteInt(0);
            pWriter.WriteUnicodeString(mail.SenderName);
            pWriter.WriteUnicodeString(mail.Title);
            pWriter.WriteUnicodeString(mail.Body);

            if (mail.Items == null)
            {
                pWriter.WriteByte(0); // Number of items if 0 don't write any item data
            }
            else
            {
                pWriter.WriteByte((byte) mail.Items.Count); // Number of items if 0 don't write any item data

                foreach (Item item in mail.Items)
                {
                    pWriter.WriteInt(item.Id);
                    pWriter.WriteLong(item.Uid);
                    pWriter.WriteInt(256); // Unknown 00 01 00 00 => 256
                    pWriter.WriteInt(256); // Unknown 00 01 00 00 => 256
                    pWriter.WriteZero(21);
                    pWriter.WriteInt(item.Amount); // Amount
                    pWriter.WriteInt(0); // Unknown
                    pWriter.WriteInt(-1); // Unknown
                    pWriter.WriteLong(item.CreationTime); // Item creation time
                    pWriter.WriteZero(52); // Unknown 52 zero bytes
                    pWriter.WriteInt(-1); // Unknown
                    pWriter.WriteZero(102); // Unknown 102 zero bytes
                    pWriter.WriteInt(1); // Unknown
                    pWriter.WriteZero(14); // Unknown 14 zero bytes
                    pWriter.WriteInt(14); // Unknown
                    pWriter.WriteInt(1); // Unknown
                    pWriter.WriteZero(6); // Unknown 6 zero bytes
                    pWriter.WriteByte(1); // Unknown
                    pWriter.WriteByte(1); // Unknown
                    pWriter.WriteLong(item.OwnerCharacterId); // Item owner character id
                    pWriter.WriteUnicodeString(item.OwnerCharacterName); // Item owner name
                    pWriter.WriteZero(20); // Unknown 20 zero bytes
                }
            }

            pWriter.WriteInt(0);
            pWriter.WriteLong(0);
            pWriter.WriteInt(0);
            pWriter.WriteInt(0);
            pWriter.WriteZero(21);
            pWriter.WriteInt(0);
            pWriter.WriteInt(0);
            pWriter.WriteLong(mail.ReadTimestamp > 0 ? mail.ReadTimestamp + Environment.TickCount : 0); // Read timestamp
            pWriter.WriteLong(mail.SentTimestamp + 864000000); // Time left = sentTime + 10000 days
            pWriter.WriteLong(mail.SentTimestamp); // Sent timestamp
            pWriter.WriteShort(0);

            return pWriter;
        }

        public static PacketWriter WriteRegular(PacketWriter pWriter, Mail mail, List<Item> items = null)
        {
            pWriter.WriteEnum(mail.Type);
            pWriter.WriteLong(mail.Id);
            pWriter.WriteLong(mail.SenderCharacterId);
            pWriter.WriteUnicodeString(mail.SenderName);
            pWriter.WriteUnicodeString(mail.Title);
            pWriter.WriteUnicodeString(mail.Body);
            pWriter.WriteUnicodeString(mail.BlackMarketItemKey);
            pWriter.WriteUnicodeString(mail.BlackMarketMesoKey);

            pWriter.WriteByte((byte) items.Count);
            foreach (Item item in items)
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

            //pWriter.WriteLong(mail.ReadTimestamp > 0 ? mail.ReadTimestamp + Environment.TickCount : 0); // Read timestamp
            //pWriter.WriteLong(mail.SentTimestamp + 2592000); // Time left = sentTime + 30 days
            //pWriter.WriteLong(mail.SentTimestamp + Environment.TickCount); // Sent timestamp
            //pWriter.WriteShort(0);

            return pWriter;
        }
    }
}
