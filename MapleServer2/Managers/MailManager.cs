using MapleServer2.Database;
using MapleServer2.Types;

namespace MapleServer2.Managers;

public class MailManager
{
    private readonly Dictionary<long, Mail> MailList;

    public MailManager()
    {
        MailList = new();
        List<Mail> list = DatabaseManager.Mails.FindAll();
        foreach (Mail mail in list)
        {
            if (mail.ExpiryTimestamp < TimeInfo.Now())
            {
                RemoveMail(mail);
                DatabaseManager.Mails.Delete(mail.Id);
                continue;
            }

            AddMail(mail);
        }
    }

    public void AddMail(Mail mail)
    {
        MailList.Add(mail.Id, mail);
    }

    public void RemoveMail(Mail mail)
    {
        foreach (Item item in mail.Items)
        {
            DatabaseManager.Items.Delete(item.Uid);
        }

        MailList.Remove(mail.Id);
    }

    public List<Mail> GetMails(long characterId)
    {
        return MailList.Values.Where(b => b.RecipientCharacterId == characterId).ToList();
    }
}
