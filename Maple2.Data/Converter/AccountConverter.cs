using Maple2.Data.Types;

namespace Maple2.Data.Converter {
    public class AccountConverter : IModelConverter<Account, Maple2.Sql.Model.Account> {
        public Maple2.Sql.Model.Account ToModel(Account value, Maple2.Sql.Model.Account account) {
            if (value == null) return null;

            account ??= new Maple2.Sql.Model.Account();
            account.Id = value.Id;
            account.LastModified = value.LastModified;
            account.Merets = value.Merets;
            account.MaxCharacters = value.MaxCharacters;
            account.PrestigeLevel = value.PrestigeLevel;
            account.PrestigeExperience = value.PrestigeExperience;
            account.PremiumTime = value.PremiumTime;

            return account;
        }

        public Account FromModel(Maple2.Sql.Model.Account value) {
            if (value == null) return null;

            var account = new Account();
            account.Id = value.Id;
            account.LastModified = value.LastModified;
            account.Merets = value.Merets;
            account.MaxCharacters = value.MaxCharacters;
            account.PrestigeLevel = value.PrestigeLevel;
            account.PrestigeExperience = value.PrestigeExperience;
            account.PremiumTime = value.PremiumTime;

            return account;
        }
    }
}