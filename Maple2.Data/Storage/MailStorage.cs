using System;
using System.Collections.Generic;
using System.Linq;
using Maple2.Data.Converter;
using Maple2.Data.Types;
using Maple2.Data.Types.Items;
using Maple2.Data.Utils;
using Maple2.Sql.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace Maple2.Data.Storage {
    public class MailStorage {
        private readonly DbContextOptions options;
        private readonly IModelConverter<Item, Maple2.Sql.Model.Item> itemConverter;
        private readonly IModelConverter<MailInfo, Maple2.Sql.Model.Mail> mailConverter;
        private readonly ILogger logger;

        public MailStorage(DbContextOptions options,
                IModelConverter<Item, Maple2.Sql.Model.Item> itemConverter,
                IModelConverter<MailInfo, Maple2.Sql.Model.Mail> mailConverter,
                ILogger<MarketStorage> logger) {
            this.options = options;
            this.itemConverter = itemConverter;
            this.mailConverter = mailConverter;
            this.logger = logger;
        }

        public Request Context() {
            return new Request(this, new MailContext(options), logger);
        }

        public class Request : DatabaseRequest<MailContext> {
            private readonly MailStorage storage;
            private readonly ItemStorageOperations<MailContext> itemOperations;

            public Request(MailStorage storage, MailContext context, ILogger logger) : base(context, logger) {
                this.storage = storage;
                itemOperations = new ItemStorageOperations<MailContext>(context, storage.itemConverter, logger);
            }

            /* Read */
            public MailInfo GetMail(long mailId, long requesterId = -1) {
                IQueryable<Maple2.Sql.Model.Mail> mailQuery = context.Mail.AsQueryable()
                    .Where(dbMail => dbMail.Id == mailId);

                if (requesterId != -1) {
                    mailQuery = mailQuery.Where(dbMail => dbMail.RecipientId == requesterId);
                }

                Maple2.Sql.Model.Mail model = mailQuery.SingleOrDefault();
                if (model == null) {
                    return null;
                }

                MailInfo mail = storage.mailConverter.FromModel(model);
                mail.Items = itemOperations.GetItems(mail.Id);
                SetSenderName(mail);

                return mail;
            }

            public ICollection<MailInfo> GetAllMail(long characterId) {
                ICollection<MailInfo> mails = context.Mail.AsQueryable()
                    .Where(dbMail => dbMail.RecipientId == characterId)
                    .AsEnumerable()
                    .Select(storage.mailConverter.FromModel)
                    .ToList();

                foreach (MailInfo mail in mails) {
                    mail.Items = itemOperations.GetItems(mail.Id);
                    SetSenderName(mail);
                }

                return mails;
            }

            public long CharacterNameToId(string name) {
                return context.Character.AsQueryable()
                    .Where(character => character.Name == name)
                    .Select(character => character.Id)
                    .SingleOrDefault();
            }

            private void SetSenderName(MailInfo mail) {
                mail.SenderName = context.Character.AsQueryable()
                    .Where(character => character.Id == mail.SenderId)
                    .Select(character => character.Name)
                    .SingleOrDefault() ?? string.Empty;
            }

            /* Write */
            public long CreateMail(MailInfo mail) {
                Maple2.Sql.Model.Mail dbMail = storage.mailConverter.ToModel(mail);
                if (mail.Items.Count == 0) {
                    context.Mail.Add(dbMail);
                    return context.TrySaveChanges() ? dbMail.Id : -1;
                }

                // Use transaction when saving items as well
                using IDbContextTransaction transaction = context.Database.BeginTransaction();
                context.Mail.Add(dbMail);
                if (!context.TrySaveChanges()) {
                    return -1;
                }

                itemOperations.StageSyncItems(dbMail.Id, mail.Items);
                transaction.Commit();
                return dbMail.Id;
            }

            public bool ReadMail(long mailId, long requesterId = -1) {
                Maple2.Sql.Model.Mail model = SafeGetMail(mailId, requesterId);
                if (model == null || model.ReadTime > model.CreationTime) {
                    return false;
                }

                model.ReadTime = DateTime.UtcNow;
                return context.TrySaveChanges();
            }

            public bool DeleteMail(long mailId, long requesterId = -1) {
                Maple2.Sql.Model.Mail model = SafeGetMail(mailId, requesterId);
                if (model == null) {
                    return false;
                }

                context.Mail.Remove(model);
                return context.TrySaveChanges();
            }

            private Maple2.Sql.Model.Mail SafeGetMail(long mailId, long requesterId) {
                Maple2.Sql.Model.Mail model = context.Mail.Find(mailId);
                if (model == null) {
                    return null;
                }

                // requesterId prevents you from accessing other people's mail
                if (requesterId != -1 && model.RecipientId != requesterId) {
                    return null;
                }

                return model;
            }
        }
    }
}