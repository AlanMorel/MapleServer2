using System;
using System.Collections.Generic;

namespace MapleServer2.Types
{
    public class Mailbox
    {
        public List<Mail> Box { get; private set; }

        public Mailbox()
        {
            Box = new List<Mail>();
        }

        public void AddOrUpdate(Mail mail)
        {
            int index = Box.FindIndex(x => x.Uid == mail.Uid);

            if (index > -1)
            {
                Box[index] = mail;
            }
            else
            {
                Box.Insert(0, mail); // Adds to front of mailbox
            }
        }

        public long Read(int id)
        {
            long timestamp = 0;
            int index = Box.FindIndex(x => x.Uid == id);

            if (index > -1)
            {
                Box[index].Read(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
                timestamp = Box[index].ReadTimestamp;
            }

            return timestamp;
        }

        public List<Item> Collect(int id)
        {
            List<Item> items = null;
            int index = Box.FindIndex(x => x.Uid == id);

            if (index > -1)
            {
                items = Box[index].Items;
                Box[index].Collect(null);
            }

            return items;

        }

        public int GetUnreadCount()
        {
            int unread = 0;
            foreach (Mail mail in Box)
            {
                if (mail.ReadTimestamp == 0)
                {
                    unread++;
                }
            }

            return unread;
        }

        public void ClearExpired()
        {
            long currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            foreach (Mail mail in Box)
            {
                if (mail.Type == 1)
                {
                    if (currentTime >= mail.SentTimestamp + 2592000) // 2592000 = 30 days
                    {
                        Box.Remove(mail);
                    }
                }
                else if (mail.Type == 101)
                {
                    if (currentTime >= mail.SentTimestamp + 864000000) // 864000000 = 10000 days
                    {
                        Box.Remove(mail);
                    }
                }
            }
        }
    }
}
