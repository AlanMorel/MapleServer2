using MapleServer2.Database;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;

namespace MapleServer2.Types
{
    public class Mail
    {
        public long Id { get; set; }
        public MailType Type { get; set; }
        public long RecipientCharacterId { get; set; }
        public long SenderCharacterId { get; set; }
        public string SenderName { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public long ReadTimestamp { get; set; }
        public long SentTimestamp { get; set; }
        public long ExpiryTimestamp { get; set; }
        public long Mesos { get; set; }
        public List<Item> Items = new List<Item>();
        public string AdditionalParameter1 = "";
        public string AdditionalParameter2 = "";

        public Mail() { }

        public Mail(MailType type, long recipientCharacterId, long senderCharacterId, string senderName, string title, string body, string addParameter1, string addParameter2, List<Item> items, long mesos)
        {
            Type = type;
            RecipientCharacterId = recipientCharacterId;
            SenderCharacterId = senderCharacterId;
            SenderName = senderName;
            Title = title;
            Body = body;
            SentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            ExpiryTimestamp = SentTimestamp + 2592000; // 30 days TODO: Change to grab from Constant.xml
            Items = items;
            Mesos = mesos;
            AdditionalParameter1 = addParameter1;
            AdditionalParameter2 = addParameter2;
            Id = DatabaseManager.Mails.Insert(this);
            foreach (Item item in items)
            {
                item.MailId = Id;
                DatabaseManager.Items.Update(item);
            }
        }

        public Mail(long id, MailType type, long recipientCharacterId, long senderCharacterId, string senderName, string title, string body, long sentTimestamp, long expiryTimestamp, long readTimestamp, string addParameter1,
            string addParameter2, List<Item> items, long mesos)
        {
            Id = id;
            Type = type;
            RecipientCharacterId = recipientCharacterId;
            SenderCharacterId = senderCharacterId;
            SenderName = senderName;
            Title = title;
            Body = body;
            SentTimestamp = sentTimestamp;
            ReadTimestamp = readTimestamp;
            ExpiryTimestamp = expiryTimestamp;
            AdditionalParameter1 = addParameter1;
            AdditionalParameter2 = addParameter2;
            Items = items;
            Mesos = mesos;
        }

        public enum MailType : byte
        {
            Player = 1,
            System = 101,
            BlackMarketSale = 102,
            BlackMarketListingCancel = 104
        }

        public void Read(GameSession session)
        {
            ReadTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            DatabaseManager.Mails.Update(this);
            session.Send(MailPacket.Read(this));
        }

        public void Delete(GameSession session)
        {
            session.Player.Mailbox.Remove(this);
            session.Send(MailPacket.Delete(this));
            DatabaseManager.Mails.Delete(Id);
        }
    }
}
