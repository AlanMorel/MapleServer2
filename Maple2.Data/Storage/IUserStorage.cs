using System;
using System.Collections.Generic;
using System.Text;
using Maple2.Data.Types;

namespace Maple2.Data.Storage
{
    public interface IUserStorage
    {
        // Read operations for UserStorage_Player
        #region ReadOperations
        Account GetAccount(long accountId);
        Character GetCharacter(long characterId);
        long GetCharacterId(string name);
        List<Character> GetCharacters(long accountId);
        ProgressState GetProgress(long characterId);
        List<SkillTab> GetSkillTabs(long characterId);
        QuestState GetQuests(long characterId);
        #endregion

        // Write Operations for UserStorage_Player
        #region WriteOperations
        bool DeleteCharacter(long characterId);
        void StageAccount(Account account);
        void StageCharacter(Character character);
        void StageProgress(long characterId, ProgressState progress);
        void StageSkillTabs(long characterId, ICollection<SkillTab> skillTabs);
        void StageQuests(long characterId, QuestState quests);
        #endregion

    }
}
