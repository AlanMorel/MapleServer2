using Maple2Storage.Enums;
using MapleServer2.Database;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;
using MapleServer2.Types;

namespace MapleServer2.Managers;

public static class QuestManager
{
    public static void OnMapEnter(Player player, int mapId)
    {
        GetRelevantQuests(player, ConditionTypes.Map)
            .UpdateRelevantConditions(player.Session, ConditionTypes.Map, mapId);
    }

    public static void OnInteractObject(Player player, int objectId)
    {
        GetRelevantQuests(player, ConditionTypes.InteractObject)
            .UpdateRelevantConditions(player.Session, ConditionTypes.InteractObject, objectId, 0);

        GetRelevantQuests(player, ConditionTypes.InteractObjectRep)
            .UpdateRelevantConditions(player.Session, ConditionTypes.InteractObjectRep, objectId, 0);
    }

    public static void OnTalkNpc(Player player, int npcId, int scriptId)
    {
        GetRelevantQuests(player, ConditionTypes.TalkIn)
            .UpdateRelevantConditions(player.Session, ConditionTypes.TalkIn, npcId, scriptId);
    }

    public static void OnItemMove(Player player, int itemId, int targetId)
    {
        GetRelevantQuests(player, ConditionTypes.ItemMove)
            .UpdateRelevantConditions(player.Session, ConditionTypes.ItemMove, itemId, targetId);
    }

    public static void OnNpcKill(Player player, int npcId, int mapId)
    {
        GetRelevantQuests(player, ConditionTypes.Npc)
            .UpdateRelevantConditions(player.Session, ConditionTypes.Npc, npcId, mapId);
    }

    #region Helper Methods

    /// <summary>
    /// Updates all Conditions where the condition type, code and target match the given parameters.
    /// </summary>
    /// <param name="relevantQuests"><see cref="GetRelevantQuests"/></param>
    /// <param name="session">GameSession of request</param>
    /// <param name="conditionType">Condition type, see <see cref="ConditionTypes"/> for all types</param>
    /// <param name="code">Code as string</param>
    /// <param name="target">Target as string</param>
    private static void UpdateRelevantConditions(this IEnumerable<QuestStatus> relevantQuests, GameSession session, string conditionType, string code,
        string target = "0")
    {
        foreach (QuestStatus quest in relevantQuests)
        {
            quest.Condition.Where(condition => ConditionHelper.IsMatching(condition.Type, conditionType)
                                               && ConditionHelper.IsMatching(condition.Code, code)
                                               && ConditionHelper.IsMatching(condition.Target, target)
                                               && !condition.Completed)
                .UpdateConditions(session, quest);

            DatabaseManager.Quests.Update(quest);
        }
    }

    /// <summary>
    /// Updates all Conditions where the condition type, code and target match the given parameters.
    /// </summary>
    /// <param name="relevantQuests"><see cref="GetRelevantQuests"/></param>
    /// <param name="session">GameSession of request</param>
    /// <param name="conditionType">Condition type, see <see cref="ConditionTypes"/> for all types</param>
    /// <param name="code">Code as long</param>
    /// <param name="target">Target as long</param>
    private static void UpdateRelevantConditions(this IEnumerable<QuestStatus> relevantQuests, GameSession session, string conditionType, long code,
        long target = 0)
    {
        foreach (QuestStatus quest in relevantQuests)
        {
            quest.Condition.Where(condition => ConditionHelper.IsMatching(condition.Type, conditionType)
                                               && ConditionHelper.IsMatching(condition.Code, code)
                                               && (ConditionHelper.IsMatching(condition.Target, target) || ConditionHelper.IsMatching(condition.Target, "0"))
                                               && !condition.Completed)
                .UpdateConditions(session, quest);
        }
    }

    /// <summary>
    /// Increase the current condition value by 1.
    /// If the condition value is already at the maximum, the condition is completed.
    /// Send the respective quest packets to the player.
    /// </summary>
    private static void UpdateConditions(this IEnumerable<Condition> relevantConditions, GameSession session, QuestStatus quest)
    {
        foreach (Condition condition in relevantConditions)
        {
            if (condition.Goal != 0 && condition.Goal == condition.Current)
            {
                continue;
            }

            condition.Current++;
            if (condition.Current >= condition.Goal)
            {
                condition.Completed = true;
            }

            session.Send(QuestPacket.UpdateCondition(quest.Basic.Id, quest.Condition));

            if (!condition.Completed || quest.Basic.QuestType is not QuestType.Exploration)
            {
                continue;
            }

            quest.State = QuestState.Completed;
            quest.AmountCompleted++;
            quest.CompleteTimestamp = TimeInfo.Now();

            session.Player.Levels.GainExp(quest.Reward.Exp);
            session.Player.Wallet.Meso.Modify(quest.Reward.Money);
            session.Send(QuestPacket.CompleteQuest(quest.Basic.Id, false));

            DatabaseManager.Quests.Update(quest);
        }
    }

    /// <summary>
    /// Gets all QuestStatus that contains at least one condition that matches the given condition type.
    /// </summary>
    /// <param name="player">Player</param>
    /// <param name="conditionType">Condition type, see <see cref="ConditionTypes"/> for all types</param>
    private static IEnumerable<QuestStatus> GetRelevantQuests(Player player, string conditionType)
    {
        return player.QuestData.Values.Where(quest => quest.State is QuestState.Started
                                                      && quest.Condition is not null
                                                      && quest.Condition.Any(condition => ConditionHelper.IsMatching(condition.Type, conditionType)));
    }

    #endregion

    /// <summary>
    /// All possible conditions for a quest.
    /// </summary>
    private static class ConditionTypes
    {
        public const string AdventureLevelUp = "adventure_level_up";
        public const string BreakableObject = "breakable_object";
        public const string BuddyRequest = "buddy_request";
        public const string Buff = "buff";
        public const string BuyCube = "buy_cube";
        public const string ChangeProfile = "change_profile";
        public const string ChangeUgcEquip = "change_ugc_equip";
        public const string Chat = "chat";
        public const string Climb = "climb";
        public const string ClubJoin = "club_join";
        public const string Continent = "continent";
        public const string Controller = "controller";
        public const string Crawl = "crawl";
        public const string Dialogue = "dialogue";
        public const string DonationType = "donation_type";
        public const string DungeonClear = "dungeon_clear";
        public const string DungeonClearGroup = "dungeon_clear_group";
        public const string DungeonHelpBeginnerHelpee = "dungeon_help_beginner_helpee";
        public const string DungeonRandomBonus = "dungeon_random_bonus";
        public const string DungeonRank = "dungeon_rank";
        public const string DungeonRankClear = "dungeon_rank_clear";
        public const string DungeonReward = "dungeon_reward";
        public const string DungeonRewardGroup = "dungeon_reward_group";
        public const string Emotion = "emotion";
        public const string EmotionTime = "emotiontime";
        public const string EnchantResult = "enchant_result";
        public const string EnterOtherHouse = "enter_otherhouse";
        public const string EquipGemstonePutOn = "equip_gemstone_puton";
        public const string Exp = "exp";
        public const string Fall = "fall";
        public const string Fish = "fish";
        public const string FishBig = "fish_big";
        public const string GemstoneUpgradeSuccess = "gemstone_upgrade_success";
        public const string GemstoneUpgradeTry = "gemstone_upgrade_try";
        public const string GetMenteeToken = "get_mentee_token";
        public const string GetMentorToken = "get_mentor_token";
        public const string Glide = "glide";
        public const string GuildAttendance = "guild_attendance";
        public const string GuildExp = "guild_exp";
        public const string GuildJoin = "guild_join";
        public const string GuildJoinReq = "guild_join_req";
        public const string HeroAchieve = "hero_achieve";
        public const string HitTombstone = "hit_tombstone";
        public const string HoldTime = "holdtime";
        public const string HomeBank = "home_bank";
        public const string InstallItem = "install_item";
        public const string InteractObject = "interact_object";
        public const string InteractObjectRep = "interact_object_rep";
        public const string InteriorPoint = "interior_point";
        public const string ItemAdd = "item_add";
        public const string ItemBreak = "item_break";
        public const string ItemDesign = "item_design";
        public const string ItemExist = "item_exist";
        public const string ItemMove = "item_move";
        public const string ItemPickup = "item_pickup";
        public const string ItemRemakeOption = "item_remake_option";
        public const string Job = "job";
        public const string Jump = "jump";
        public const string LadderTime = "laddertime";
        public const string Level = "level";
        public const string Map = "map";
        public const string MasteryFarming = "mastery_farming";
        public const string MasteryGathering = "mastery_gathering";
        public const string MasteryGatheringTry = "mastery_gathering_try";
        public const string MasteryGrade = "mastery_grade";
        public const string MasteryHarvest = "mastery_harvest";
        public const string MasteryHarvestOtherHouse = "mastery_harvest_otherhouse";
        public const string MasteryHarvestTry = "mastery_harvest_try";
        public const string MasteryManufacturing = "mastery_manufacturing";
        public const string MesoDonation = "meso_donation";
        public const string MissionPoint = "mission_point";
        public const string MusicPlayEnsemble = "music_play_ensemble";
        public const string MusicPlayEnsembleIn = "music_play_ensemble_in";
        public const string MusicPlayInstrumentTime = "music_play_instrument_time";
        public const string MusicPlayScore = "music_play_score";
        public const string Npc = "npc";
        public const string NpcFieldBoss = "npc_field_boss";
        public const string NpcFieldElite = "npc_field_elite";
        public const string NpcRace = "npc_race";
        public const string OpenItemBox = "openitembox";
        public const string OpenStoryBook = "openstorybook";
        public const string PetCatchCategory = "pet_catch_category";
        public const string PetCatchGrade = "pet_catch_grade";
        public const string PetCatchId = "pet_catch_id";
        public const string PetEnchant = "pet_enchant";
        public const string PetEnchantExp = "pet_enchant_exp";
        public const string PetTaming = "pet_taming";
        public const string PlayEnsenbleTime = "play_ensenble_time";
        public const string PvpParticipation = "pvp_participation";
        public const string Quest = "quest";
        public const string QuestAlliance = "quest_alliance";
        public const string QuestClear = "quest_clear";
        public const string QuestClearByChapter = "quest_clear_by_chapter";
        public const string QuestDaily = "quest_daily";
        public const string QuestField = "quest_field";
        public const string QuestGuide = "quest_guide";
        public const string Riding = "riding";
        public const string RopeTime = "ropetime";
        public const string RotateCube = "rotate_cube";
        public const string Run = "run";
        public const string SendMail = "send_mail";
        public const string ShadowWorldKill = "shadow_world_kill";
        public const string Skill = "skill";
        public const string SkillDamageNpc = "skill_damage_npc";
        public const string SocketUnlockTry = "socket_unlock_try";
        public const string Spawner = "spawner";
        public const string StayCube = "stay_cube";
        public const string StayMap = "stay_map";
        public const string SurvivalEnter = "survival_enter";
        public const string SurvivalKill = "survival_kill";
        public const string Swim = "swim";
        public const string SwimTime = "swimtime";
        public const string TalkIn = "talk_in";
        public const string TaxiUse = "taxiuse";
        public const string Trigger = "trigger";
        public const string TrophyPoint = "trophy_point";
        public const string UninstallItem = "uninstall_item";
        public const string UserFind = "user_find";
        public const string UserOpenMinigameClear = "useropen_minigame_clear";
        public const string VibrateObject = "vibrate_object";
        public const string VipGm = "vipgm";
    }
}
