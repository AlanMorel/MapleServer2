using MapleServer2.Database;
using MapleServer2.Types;

namespace MapleServer2.Managers
{
    public class MailManager
    {
        private readonly Dictionary<long, Mail> MailList;

        public MailManager()
        {
            MailList = new Dictionary<long, Mail>();
            List<Mail> list = DatabaseManager.Mails.FindAll();
            foreach (Mail mail in list)
            {
                AddMail(mail);
            }
        }

        public void AddMail(Mail mail) => MailList.Add(mail.Id, mail);

        public void RemoveMail(Mail mail) => MailList.Remove(mail.Id);

        public List<Mail> GetMails(long characterId) => MailList.Values.Where(b => b.RecipientCharacterId == characterId).ToList();
    }
}
