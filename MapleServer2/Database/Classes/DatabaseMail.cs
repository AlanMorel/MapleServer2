using MapleServer2.Enums;
using MapleServer2.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes;

public class DatabaseMail : DatabaseTable
{
    public DatabaseMail() : base("mails") { }

    public List<Mail> FindAll()
    {
        List<Mail> mailList = new();
        IEnumerable<dynamic> results = QueryFactory.Query(TableName).Get();
        foreach (dynamic result in results)
        {
            mailList.Add(ReadMail(result));
        }
        return mailList;
    }

    public long Insert(Mail mail)
    {
        return QueryFactory.Query(TableName).InsertGetId<long>(new
        {
            type = mail.Type,
            recipient_character_id = mail.RecipientCharacterId,
            sender_character_id = mail.SenderCharacterId,
            sender_name = mail.SenderName,
            title = mail.Title,
            body = mail.Body,
            read_timestamp = mail.ReadTimestamp,
            sent_timestamp = mail.SentTimestamp,
            expiry_timestamp = mail.ExpiryTimestamp,
            additional_parameter1 = mail.AdditionalParameter1,
            additional_parameter2 = mail.AdditionalParameter2,
            mesos = mail.Mesos
        });
    }

    public virtual void Update(Mail mail)
    {
        QueryFactory.Query(TableName).Where("id", mail.Id).Update(new
        {
            read_timestamp = mail.ReadTimestamp,
            mesos = mail.Mesos
        });
    }

    public bool Delete(long id)
    {
        return QueryFactory.Query(TableName).Where("id", id).Delete() == 1;
    }

    private static Mail ReadMail(dynamic data)
    {
        return new()
        {
            Id = data.id,
            Type = (MailType) data.type,
            RecipientCharacterId = data.recipient_character_id,
            SenderCharacterId = data.sender_character_id,
            SenderName = data.sender_name,
            Title = data.title,
            Body = data.body,
            SentTimestamp = data.sent_timestamp,
            ExpiryTimestamp = data.expiry_timestamp,
            ReadTimestamp = data.read_timestamp,
            AdditionalParameter1 = data.additional_parameter1,
            AdditionalParameter2 = data.additional_parameter2,
            Items = DatabaseManager.Items.FindAllByMailId(data.id),
            Mesos = data.mesos
        };
    }
}
