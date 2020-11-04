using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Maple2.Data.Types;
using Maple2.Data.Utils;
using Microsoft.EntityFrameworkCore.Storage;

namespace Maple2.Data.Storage {
    public partial class UserStorage {
        public partial class Request
        {
            #region ReadOperations
            public Account GetAccount(long accountId) {
                return storage.accountConverter.FromModel(context.Account.Find(accountId));
            }

            public Character GetCharacter(long characterId) {
                return context.Character.AsQueryable()
                    .Where(character => character.Id == characterId)
                    .AsEnumerable()
                    .Select(storage.characterConverter.FromModel)
                    .SingleOrDefault();
            }

            public long GetCharacterId(string name) {
                return context.Character.AsQueryable()
                    .Where(character => character.Name == name)
                    .Select(character => character.Id)
                    .SingleOrDefault();
            }

            public List<Character> GetCharacters(long accountId) {
                return context.Character.AsQueryable()
                    .Where(character => character.AccountId == accountId)
                    .AsEnumerable()
                    .Select(storage.characterConverter.FromModel)
                    .ToList();
            }

            public ProgressState GetProgress(long characterId) {
                return context.CharacterProgress.AsQueryable()
                    .Where(dbProgress => dbProgress.CharacterId == characterId)
                    .AsEnumerable()
                    .Select(storage.progressConverter.FromModel)
                    .SingleOrDefault();
            }

            public List<SkillTab> GetSkillTabs(long characterId) {
                return context.SkillTab.AsQueryable()
                    .Where(dbTab => dbTab.CharacterId == characterId)
                    .AsEnumerable()
                    .Select(storage.skillTabConverter.FromModel)
                    .ToList();
            }

            public QuestState GetQuests(long characterId) {
                var state = new QuestState();
                IEnumerable<Quest> quests = context.Quest.AsQueryable()
                    .Where(dbQuest => dbQuest.CharacterId == characterId)
                    .AsEnumerable()
                    .Select(storage.questConverter.FromModel);
                foreach (Quest quest in quests) {
                    state.Quests[quest.Id] = quest;
                }

                return state;
            }
            #endregion

            #region WriteOperations
            public long CreateAccount(Account account) {
                Maple2.Sql.Model.Account model = storage.accountConverter.ToModel(account);
                model.Id = 0;
                context.Account.Add(model);
                return context.TrySaveChanges() ? model.Id : -1;
            }

            public long CreateCharacter(Character character) {
                using IDbContextTransaction transaction = context.Database.BeginTransaction();
                Maple2.Sql.Model.Character dbCharacter = storage.characterConverter.ToModel(character);
                dbCharacter.Id = 0;
                context.Character.Add(dbCharacter);
                if (!context.TrySaveChanges()) {
                    return -1;
                }

                var progress = new ProgressState();
                progress.Emotes.UnionWith(new [] {
                    90200001, 90200003, 90200004, 90200005, 90200006, 90200011,
                    90200022, 90200023, 90200024, 90200031, 90200041, 90200042
                });
                progress.Titles.Add(10000357);
                Maple2.Sql.Model.CharacterProgress dbProgress = storage.progressConverter.ToModel(progress);
                dbProgress.CharacterId = dbCharacter.Id;
                context.CharacterProgress.Add(dbProgress);

                var quests = new QuestState();
                // Lapenshard/Housing quests to skip
                // Lapenshard
                quests.StartQuest(20002391);
                quests.FinishQuest(20002391);
                // Housing
                quests.StartQuest(90000660);
                quests.FinishQuest(90000660);
                quests.StartQuest(90000661);
                quests.FinishQuest(90000661);
                quests.StartQuest(90000760);
                quests.FinishQuest(90000760);
                quests.StartQuest(90000762);
                quests.FinishQuest(90000762);
                quests.StartQuest(90000690);
                quests.FinishQuest(90000690);
                quests.StartQuest(90000692);
                quests.FinishQuest(90000692);
                quests.StartQuest(90000670);
                quests.FinishQuest(90000670);
                quests.StartQuest(90000680);
                quests.FinishQuest(90000680);
                StageQuests(dbCharacter.Id, quests);

                transaction.Commit();
                return dbCharacter.Id;
            }

            public long CreateSkillTab(long characterId, SkillTab skillTab) {
                Maple2.Sql.Model.SkillTab model = storage.skillTabConverter.ToModel(skillTab);
                model.Id = 0;
                model.CharacterId = characterId;
                context.SkillTab.Add(model);
                return context.TrySaveChanges() ? model.Id : -1;
            }

            // Soft delete by assigning accountId to 0
            public bool DeleteCharacter(long characterId) {
                Maple2.Sql.Model.Character character = context.Character.Find(characterId);
                if (character == null) {
                    return false;
                }

                character.AccountId = 0;
                return context.TrySaveChanges();
            }

            public void StageAccount(Account account) {
                Maple2.Sql.Model.Account dbAccount = context.Account.Find(account.Id);
                Debug.Assert(dbAccount != null);
                storage.accountConverter.ToModel(account, dbAccount);
            }

            public void StageCharacter(Character character) {
                Maple2.Sql.Model.Character dbCharacter = context.Character.Find(character.Id);
                Debug.Assert(dbCharacter != null);
                storage.characterConverter.ToModel(character, dbCharacter);
            }

            public void StageProgress(long characterId, ProgressState progress) {
                Maple2.Sql.Model.CharacterProgress dbProgress = context.CharacterProgress
                    .SingleOrDefault(cProgress => cProgress.CharacterId == characterId);
                storage.progressConverter.ToModel(progress, dbProgress);
            }

            public void StageSkillTabs(long characterId, ICollection<SkillTab> skillTabs) {
                foreach (SkillTab skillTab in skillTabs) {
                    Maple2.Sql.Model.SkillTab dbSkillTab = context.SkillTab.Find(skillTab.Id);
                    Debug.Assert(dbSkillTab?.CharacterId == characterId);
                    storage.skillTabConverter.ToModel(skillTab, dbSkillTab);
                }
            }

            public void StageQuests(long characterId, QuestState quests) {
                foreach (Quest quest in quests.Quests.Values) {
                    Maple2.Sql.Model.Quest dbQuest = context.Quest.Find(characterId, quest.Id);
                    if (dbQuest != null) {
                        storage.questConverter.ToModel(quest, dbQuest);
                    } else {
                        dbQuest = storage.questConverter.ToModel(quest);
                        dbQuest.CharacterId = characterId;
                        context.Add(dbQuest);
                    }
                }
            }
            #endregion
        }
    }
}