using Maple2.Data.Types;
using Maple2.Data.Types.Items;
using Maple2.Data.Utils;

namespace Maple2.Data.Converter {
    public class MailConverter : IModelConverter<MailInfo, Maple2.Sql.Model.Mail> {
        private readonly IModelConverter<Item, Maple2.Sql.Model.Item> itemConverter;

        public MailConverter(IModelConverter<Item, Maple2.Sql.Model.Item> itemConverter) {
            this.itemConverter = itemConverter;
        }

        public Maple2.Sql.Model.Mail ToModel(MailInfo value, Maple2.Sql.Model.Mail mail) {
            if (value == null) return null;

            mail ??= new Maple2.Sql.Model.Mail();
            mail.Id = value.Id;
            mail.SenderId = value.SenderId;
            mail.RecipientId = value.RecipientId;
            mail.ExpiryTime = value.ExpiryTime.FromEpochSeconds();
            mail.ReadTime = value.ReadTime.FromEpochSeconds(); // TODO: maybe dont set this
            mail.Type = value.Type;
            mail.Title = value.Title;
            mail.Content = value.Content;

            return mail;
        }

        public MailInfo FromModel(Maple2.Sql.Model.Mail value) {
            if (value == null) return null;

            var mail = new MailInfo();
            mail.Id = value.Id;
            mail.SenderId = value.SenderId;
            mail.RecipientId = value.RecipientId;
            mail.CreationTime = value.CreationTime.ToEpochSeconds();
            mail.ExpiryTime = value.ExpiryTime.ToEpochSeconds();
            mail.ReadTime = value.ReadTime.ToEpochSeconds();
            mail.Type = value.Type;
            mail.Title = value.Title;
            mail.Content = value.Content;

            return mail;
        }
    }
}