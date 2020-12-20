using System.Collections.Generic;
using MapleServer2.Tools;

namespace MapleServer2.Types
{
    public class Mail
    {
        public byte Type { get; private set; } // Mail type (101 = system reward, 102 = blackmarket, 1 = regular?)
        public int Uid { get; private set; }
        public long CharacterId { get; private set; }
        public string SenderName { get; private set; }
        public string Title { get; private set; }
        public string Body { get; private set; }
        public long ReadTimestamp {get; private set;}
        public long SentTimestamp { get; private set; }
        public List<Item> Items { get; private set; }

        public Mail(byte type, long characterId, string senderName, string title, string body, long readTimestamp, long sentTimestamp, List<Item> items)
        {
            Type = type;
            Uid = GuidGenerator.Int();
            CharacterId = characterId;
            SenderName = senderName;
            Title = title;
            Body = body;
            ReadTimestamp = readTimestamp;
            SentTimestamp = sentTimestamp;
            Items = items;
        }

        public void Read(long readTimestamp)
        {
            ReadTimestamp = readTimestamp;
        }

        public void Collect(List<Item> items)
        {
            Items = items;
        }
    }
}