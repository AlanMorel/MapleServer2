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
            int index = Box.FindIndex(x => x.Uid == id);
            Box[index].Read(DateTimeOffset.UtcNow.ToUnixTimeSeconds());

            return Box[index].ReadTimestamp;
        }

        public List<Item> Collect(int id)
        {
            int index = Box.FindIndex(x => x.Uid == id);
            List<Item> items = Box[index].Items;
            Box[index].Collect(null);

            return items;
        }

        public int GetUnread()
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
    }
}