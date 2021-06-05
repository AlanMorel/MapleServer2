using System;
using System.Collections.Generic;

namespace MapleServer2.Types
{
    public class Mailbox
    {
        public List<Mail> Mails { get; private set; } = new List<Mail>();

        public Mailbox() { }

        public Mailbox(List<Mail> mails)
        {
            Mails = mails;
        }

        public void AddOrUpdate(Mail mail)
        {
            int index = Mails.FindIndex(x => x.Uid == mail.Uid);

            if (index > -1)
            {
                Mails[index] = mail;
            }
            else
            {
                Mails.Insert(0, mail); // Adds to front of mailbox
            }
        }

        public long Read(int id)
        {
            long timestamp = 0;
            int index = Mails.FindIndex(x => x.Uid == id);

            if (index > -1)
            {
                Mails[index].Read(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
                timestamp = Mails[index].ReadTimestamp;
            }

            return timestamp;
        }

        public List<Item> Collect(int id)
        {
            List<Item> items = null;
            int index = Mails.FindIndex(x => x.Uid == id);

            if (index > -1)
            {
                items = Mails[index].Items;
                Mails[index].Collect(null);
            }

            return items;

        }

        public int GetUnreadCount()
        {
            int unread = 0;
            foreach (Mail mail in Mails)
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

            foreach (Mail mail in Mails)
            {
                if (mail.Type == 1)
                {
                    if (currentTime >= mail.SentTimestamp + 2592000) // 2592000 = 30 days
                    {
                        Mails.Remove(mail);
                    }
                }
                else if (mail.Type == 101)
                {
                    if (currentTime >= mail.SentTimestamp + 864000000) // 864000000 = 10000 days
                    {
                        Mails.Remove(mail);
                    }
                }
            }
        }
    }
}
