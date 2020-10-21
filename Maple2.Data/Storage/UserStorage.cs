using Maple2.Data.Converter;
using Maple2.Data.Types;
using Maple2.Data.Types.Items;
using Maple2.Sql.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Maple2.Data.Storage {
    public partial class UserStorage {
        private readonly DbContextOptions options;
        private readonly IModelConverter<Account, Maple2.Sql.Model.Account> accountConverter;
        private readonly IModelConverter<Character, Maple2.Sql.Model.Character> characterConverter;
        private readonly IModelConverter<Item, Maple2.Sql.Model.Item> itemConverter;
        private readonly IModelConverter<ProgressState, Maple2.Sql.Model.CharacterProgress> progressConverter;
        private readonly IModelConverter<SkillTab, Maple2.Sql.Model.SkillTab> skillTabConverter;
        private readonly IModelConverter<Quest, Maple2.Sql.Model.Quest> questConverter;
        private readonly ILogger logger;

        public UserStorage(DbContextOptions options,
                IModelConverter<Account, Maple2.Sql.Model.Account> accountConverter,
                IModelConverter<Character, Maple2.Sql.Model.Character> characterConverter,
                IModelConverter<Item, Maple2.Sql.Model.Item> itemConverter,
                IModelConverter<ProgressState, Maple2.Sql.Model.CharacterProgress> progressConverter,
                IModelConverter<SkillTab, Maple2.Sql.Model.SkillTab> skillTabConverter,
                IModelConverter<Quest, Maple2.Sql.Model.Quest> questConverter,
                ILogger<UserStorage> logger) {
            this.options = options;
            this.accountConverter = accountConverter;
            this.characterConverter = characterConverter;
            this.itemConverter = itemConverter;
            this.progressConverter = progressConverter;
            this.skillTabConverter = skillTabConverter;
            this.questConverter = questConverter;
            this.logger = logger;
        }

        public Request Context()  {
            return new Request(this, new UserContext(options), logger);
        }

        public partial class Request : DatabaseRequest<UserContext> {
            private readonly UserStorage storage;
            private readonly ItemStorageOperations<UserContext> itemOperations;

            public Request(UserStorage storage, UserContext context, ILogger logger) : base(context, logger) {
                this.storage = storage;
                itemOperations = new ItemStorageOperations<UserContext>(context, storage.itemConverter, logger);
            }
        }
    }
}