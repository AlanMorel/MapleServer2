using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Database;
using MapleServer2.Packets;
using MapleServer2.Types;

namespace MapleServer2.Managers;

internal static class TrophyManager
{
    public static void OnAcceptQuest(Player player, int questId)
    {
        IEnumerable<TrophyMetadata> questAcceptTrophies = GetRelevantTrophies(TrophyTypes.QuestAccept);
        IEnumerable<TrophyMetadata> matchingTrophies = questAcceptTrophies
            .Where(t => t.Grades.Any(g => IsMatching(g.ConditionCodes, questId)));

        UpdateMatchingTrophies(player, matchingTrophies, 1);
    }

    public static void OnMapEntered(Player player, long mapId)
    {
        IEnumerable<TrophyMetadata> mapTrophies = GetRelevantTrophies(TrophyTypes.Map);
        IEnumerable<TrophyMetadata> matchingTrophies = mapTrophies
            .Where(t => IsMatching(t.Grades.First().ConditionCodes, mapId));

        UpdateMatchingTrophies(player, matchingTrophies, 1);
    }

    public static void OnJump(Player player)
    {
        IEnumerable<TrophyMetadata> jumpTrophies = GetRelevantTrophies(TrophyTypes.Jump);

        UpdateMatchingTrophies(player, jumpTrophies, 1);
    }

    public static void OnPlayTimeTick(Player player)
    {
        IEnumerable<TrophyMetadata> playtimeTrophies = GetRelevantTrophies(TrophyTypes.PlayTime);

        UpdateMatchingTrophies(player, playtimeTrophies, 1);
    }

    public static void OnLevelUp(Player player)
    {
        int jobId = (int) player.Job;
        int level = player.Levels.Level;
        IEnumerable<TrophyMetadata> levelUpTrophies = GetRelevantTrophies(TrophyTypes.LevelUp);
        IEnumerable<TrophyMetadata> levelTrophies = GetRelevantTrophies(TrophyTypes.Level);

        IEnumerable<TrophyMetadata> matchingTrophies = levelUpTrophies
            .Where(t => IsMatching(t.Grades.First().ConditionCodes, jobId) && IsMatching(t.Grades.First().ConditionTargets, level));

        UpdateMatchingTrophies(player, matchingTrophies, 1);
        UpdateMatchingTrophies(player, levelTrophies, 1);
    }

    public static void OnObjectInteract(Player player, long objectId)
    {
        IEnumerable<TrophyMetadata> interactTrophies = GetRelevantTrophies(TrophyTypes.InteractObject)
            .Concat(GetRelevantTrophies(TrophyTypes.Controller));

        IEnumerable<TrophyMetadata> matchingTrophies = interactTrophies
            .Where(t => IsMatching(t.Grades.First().ConditionCodes, objectId));

        UpdateMatchingTrophies(player, matchingTrophies, 1);
    }

    public static void OnGainMasteryLevel(Player player)
    {
        IEnumerable<TrophyMetadata> masteryTrophies = GetRelevantTrophies(TrophyTypes.MasteryGrade)
            .Concat(GetRelevantTrophies(TrophyTypes.SetMasteryGrade));

        UpdateMatchingTrophies(player, masteryTrophies, 1);
    }

    public static void OnGuildJoin(Player player)
    {
        IEnumerable<TrophyMetadata> guildJoinTrophies = GetRelevantTrophies(TrophyTypes.GuildJoin);

        UpdateMatchingTrophies(player, guildJoinTrophies, 1);
    }

    public static void OnTrigger(Player player, string trigger)
    {
        IEnumerable<TrophyMetadata> triggerTrophies = GetRelevantTrophies(TrophyTypes.Trigger);

        IEnumerable<TrophyMetadata> matchingTrophies = triggerTrophies
            .Where(t => IsMatching(t.Grades.First().ConditionCodes, trigger));

        UpdateMatchingTrophies(player, matchingTrophies, 1);
    }

    #region Helper Methods

    private static IEnumerable<TrophyMetadata> GetRelevantTrophies(string category) => TrophyMetadataStorage.GetTrophiesByType(category);

    /// <summary>
    /// Checks if the given condition code or target match the given string value.
    /// </summary>
    /// <param name="trophyCondition">Trophy code or target</param>
    /// <param name="value">Value to try match</param>
    /// <returns>True or false</returns>
    private static bool IsMatching(string trophyCondition, string value)
    {
        return trophyCondition.Equals(value, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Checks if the given condition codes or targets match the given long value.
    /// </summary>
    /// <param name="trophyCondition">Trophy codes or targets</param>
    /// <param name="value">Value to try match</param>
    /// <returns>True or false</returns>
    private static bool IsMatching(string trophyCondition, long value)
    {
        // Check if the value is in the range
        if (trophyCondition.Contains('-') && IsInRange(trophyCondition, value))
        {
            return true;
        }

        // Check if the value is in the list
        if (trophyCondition.Contains(',') && IsInList(trophyCondition, value))
        {
            return true;
        }

        return long.TryParse(trophyCondition, out long parsedCondition) && parsedCondition == value;
    }

    private static bool IsInList(string trophyCondition, long value)
    {
        string[] conditions = trophyCondition.Split(',');
        foreach (string c in conditions)
        {
            if (long.TryParse(c, out long listItem) && value == listItem)
            {
                return true;
            }
        }

        return false;
    }

    private static bool IsInRange(string trophyCondition, long value)
    {
        string[] parts = trophyCondition.Split('-');
        if (!long.TryParse(parts[0], out long lowerBound))
        {
            return false;
        }

        if (!long.TryParse(parts[1], out long upperBound))
        {
            return false;
        }

        return value >= lowerBound && value <= upperBound;
    }

    private static void UpdateMatchingTrophies(Player player, IEnumerable<TrophyMetadata> trophies, int progress)
    {
        IEnumerable<int> trophyIds = trophies.Select(t => t.Id);
        foreach (int trophyId in trophyIds)
        {
            if (!player.TrophyData.ContainsKey(trophyId))
            {
                player.TrophyData[trophyId] = new(player.CharacterId, player.AccountId, trophyId);
            }

            player.TrophyData[trophyId].AddCounter(player, progress);

            player.TrophyData.TryGetValue(trophyId, out Trophy trophy);

            player.Session?.Send(TrophyPacket.WriteUpdate(trophy));
            DatabaseManager.Trophies.Update(trophy);
        }
    }

    #endregion

    private static class TrophyTypes
    {
        public const string AdventureLevel = "adventure_level";
        public const string AutoFishing = "auto_fishing";
        public const string Banner = "banner";
        public const string BeautyAdd = "beauty_add";
        public const string BeautyChangeColor = "beauty_change_color";
        public const string BeautyRandom = "beauty_random";
        public const string BeautyStyleAdd = "beauty_style_add";
        public const string BeautyStyleApply = "beauty_style_apply";
        public const string BreakableObject = "breakable_object";
        public const string Buff = "buff";
        public const string BuyHouse = "buy_house";
        public const string ChangeProfile = "change_profile";
        public const string CharacterAbilityLearn = "character_ability_learn";
        public const string CharacterAbilityReset = "character_ability_reset";
        public const string Climb = "climb";
        public const string CommendHome = "commend_home";
        public const string Continent = "continent";
        public const string Controller = "controller";
        public const string Crawl = "crawl";
        public const string CreateBlueprint = "create_blueprint";
        public const string Dialogue = "dialogue";
        public const string DonationItem = "donation_item";
        public const string DungeonClear = "dungeon_clear";
        public const string DungeonFirstClear = "dungeon_first_clear";
        public const string DungeonRandomBonus = "dungeon_random_bonus";
        public const string DungeonRank = "dungeon_rank";
        public const string DungeonReward = "dungeon_reward";
        public const string DungeonRoundClear = "dungeon_round_clear";
        public const string Emotion = "emotion";
        public const string EmotionTime = "emotiontime";
        public const string Empty = "empty";
        public const string EnchantResult = "enchant_result";
        public const string Experience = "exp";
        public const string Explore = "explore";
        public const string ExploreContinent = "explore_continent";
        public const string ExtendHouse = "extend_house";
        public const string Fall = "fall";
        public const string FallDamage = "fall_damage";
        public const string FallSurvive = "fall_survive";
        public const string FameGrade = "fame_grade";
        public const string FestivalEvent = "festival_event";
        public const string FieldMission = "field_mission";
        public const string Fish = "fish";
        public const string FishBig = "fish_big";
        public const string FishCollect = "fish_collect";
        public const string FishFail = "fish_fail";
        public const string FishGoldMedal = "fish_goldmedal";
        public const string FisherGrade = "fisher_grade";
        public const string GameHelperService = "game_helper_service";
        public const string GemstonePutoff = "gemstone_putoff";
        public const string GemstonePuton = "gemstone_puton";
        public const string GemstoneUpgrade = "gemstone_upgrade";
        public const string GemstoneUpgradeFail = "gemstone_upgrade_fail";
        public const string GemstoneUpgradeSuccess = "gemstone_upgrade_success";
        public const string GemstoneUpgradeTry = "gemstone_upgrade_try";
        public const string GetHonorToken = "get_honor_token";
        public const string GetKarmaToken = "get_karma_token";
        public const string Glide = "glide";
        public const string GuildChampionship = "guild_championship";
        public const string GuildExperience = "guild_exp";
        public const string GuildJoin = "guild_join";
        public const string GuildPvpDie = "guildpvp_die";
        public const string GuildPvpKill = "guildpvp_kill";
        public const string GuildPvpWin = "guildpvp_win";
        public const string HeroAchieve = "hero_achieve";
        public const string HeroAchieveGrade = "hero_achieve_grade";
        public const string HitTombstone = "hit_tombstone";
        public const string HoldTime = "holdtime";
        public const string HomeBank = "home_bank";
        public const string HomeDoctor = "home_doctor";
        public const string HomeGoto = "home_goto";
        public const string InstallItem = "install_item";
        public const string InteractNpc = "interact_npc";
        public const string InteractObject = "interact_object";
        public const string InteractObjectRep = "interact_object_rep";
        public const string InteriorExpOffset = "interior_exp_offset";
        public const string InteriorLevel = "interior_level";
        public const string InteriorPoint = "interior_point";
        public const string ItemAdd = "item_add";
        public const string ItemBreak = "item_break";
        public const string ItemCollect = "item_collect";
        public const string ItemDestroy = "item_destroy";
        public const string ItemGearScore = "item_gear_score";
        public const string ItemMergeSuccess = "item_merge_success";
        public const string ItemPickup = "item_pickup";
        public const string ItemRemakeOption = "item_remake_option";
        public const string ItemRemakeOptionRecord = "item_remake_option_record";
        public const string JobChange = "job_change";
        public const string Jump = "jump";
        public const string KillCount = "killcount";
        public const string LadderTime = "laddertime";
        public const string LapenshardUpgradeFail = "lapenshard_upgrade_fail";
        public const string LapenshardUpgradeResult = "lapenshard_upgrade_result";
        public const string LapenshardUpgradeSuccess = "lapenshard_upgrade_success";
        public const string LapenshardUpgradeTry = "lapenshard_upgrade_try";
        public const string Level = "level";
        public const string LevelUp = "level_up";
        public const string LimitedBundleBuy = "limited_bundle_buy";
        public const string MaidAffinity = "maid_affinity";
        public const string MaidGetItem = "maid_get_item";
        public const string MaidProfile = "maid_profile";
        public const string Map = "map";
        public const string MasteryGrade = "mastery_grade";
        public const string Meso = "meso";
        public const string MinigameClear = "minigame_clear";
        public const string MissionAttackSystem = "mission_attack_system";
        public const string MusicPlayEnsemble = "music_play_ensemble";
        public const string MusicPlayGrade = "music_play_grade";
        public const string MusicPlayInstrumentMastery = "music_play_instrument_mastery";
        public const string MusicPlayScore = "music_play_score";
        public const string Npc = "npc";
        public const string NpcAssistBonus = "npc_assist_bonus";
        public const string NpcLastHit = "npc_lasthit";
        public const string NpcLastHitBuff = "npc_lasthit_buff";
        public const string NpcLastHitTime = "npc_lasthit_time";
        public const string NpcRace = "npc_race";
        public const string NpcTimeAttack = "npc_timeattack";
        public const string NurturingEat = "nurturing_eat";
        public const string NurturingGrowth = "nurturing_growth";
        public const string NurturingPlay = "nurturing_play";
        public const string OpenItemBox = "openItemBox";
        public const string PetCollect = "pet_collect";
        public const string PetEvolutionByRank = "pet_evolution_by_rank";
        public const string PetEvolutionPointByRank = "pet_evolution_point_by_rank";
        public const string PetFirstCollect = "pet_first_collect";
        public const string PetRemakeOption = "pet_remake_option";
        public const string PetRemakeOptionRecord = "pet_remake_option_record";
        public const string PlayRpsDraw = "play_rps_draw";
        public const string PlayRpsLose = "play_rps_lose";
        public const string PlayRpsWin = "play_rps_win";
        public const string PlayTime = "playtime";
        public const string PvpDie = "pvp_die";
        public const string PvpKill = "pvp_kill";
        public const string PvpParticipation = "pvp_participation";
        public const string PvpWin = "pvp_win";
        public const string PvpWinPerfect = "pvp_win_perfect";
        public const string PvpWinScore = "pvp_win_score";
        public const string PvpWinTime = "pvp_win_time";
        public const string PvpWinWithBuff = "pvp_win_with_buff";
        public const string PvpWinWithGrade = "pvp_win_with_grade";
        public const string Quest = "quest";
        public const string QuestAccept = "quest_accept";
        public const string QuestAllianceByGrade = "quest_alliance_by_grade";
        public const string QuestClear = "quest_clear";
        public const string QuestClearByChapter = "quest_clear_by_chapter";
        public const string QuestDaily = "quest_daily";
        public const string QuestField = "quest_field";
        public const string QuestFieldFirstClear = "quest_field_first_clear";
        public const string QuestGuide = "quest_guide";
        public const string QuestReviseAchieve = "quest_revise_achieve";
        public const string ResolvePanelty = "resolve_panelty"; // Is this supposed to be penalty? What's a panelty?
        public const string ReviseAchieveMultiGrade = "revise_achieve_multi_grade";
        public const string ReviseAchieveSingleGrade = "revise_achieve_single_grade";
        public const string Revival = "revival";
        public const string Riding = "riding";
        public const string RopeTime = "ropetime";
        public const string Run = "run";
        public const string SetMasteryGrade = "set_mastery_grade";
        public const string ShadowWorldDie = "shadow_world_die";
        public const string ShadowWorldKill = "shadow_world_kill";
        public const string ShopBuy = "shop_buy";
        public const string ShopBuyHonorToken = "shop_buy_honor_token";
        public const string ShopBuyKarmaToken = "shop_buy_karma_token";
        public const string Skill = "skill";
        public const string SkillDamageNpc = "skill_damage_npc";
        public const string SkillDie = "skill_die";
        public const string SkyFortressSystem = "skyfortress_system";
        public const string SocketUnlock = "socket_unlock";
        public const string SocketUnlockFail = "socket_unlock_fail";
        public const string SocketUnlockSuccess = "socket_unlock_success";
        public const string SocketUnlockTry = "socket_unlock_try";
        public const string SplashPlant = "splash_plant";
        public const string StayMap = "stay_map";
        public const string SubjobChange = "subjob_change";
        public const string SurvivalBreakableObject = "survival_breakable_object";
        public const string SurvivalBuyGoldPass = "survival_buy_gold_pass";
        public const string SurvivalDoubleKill = "survival_double_kill";
        public const string SurvivalEnter = "survival_enter";
        public const string SurvivalItemGet = "survival_item_get";
        public const string SurvivalKill = "survival_kill";
        public const string SurvivalKillOutside = "survival_kill_outside";
        public const string SurvivalKillUseSkill = "survival_kill_use_skill";
        public const string SurvivalNpcKill = "survival_npc_kill";
        public const string SurvivalRankWithKill = "survival_rank_with_kill";
        public const string SurvivalTotalKillUseSingleSkill = "survival_total_kill_use_single_skill";
        public const string SurvivalTotalKillUseSkill = "survival_total_kill_use_skill";
        public const string SurvivalWinUseOneSkill = "survival_win_use_one_skill";
        public const string SurvivalWinWithoutInteract = "survival_win_without_interact";
        public const string SurvivalWinWithoutNpcKill = "survival_win_without_npckill";
        public const string SurviveCube = "survive_cube";
        public const string SurviveMap = "survive_map";
        public const string Swim = "swim";
        public const string TalkIn = "talk_in";
        public const string TaxiFee = "taxifee";
        public const string TaxiFind = "taxifind";
        public const string TaxiUse = "taxiuse";
        public const string Trigger = "trigger";
        public const string TrophyPoint = "trophy_point";
        public const string UnlimitedEnchant = "unlimited_enchant";
        public const string UseMerat = "use_merat";
        public const string VibrateObject = "vibrate_object";
        public const string WeddingComplete = "wedding_complete";
        public const string WeddingDivorce = "wedding_divorce";
        public const string WeddingGuest = "wedding_guest";
        public const string WeddingHallChange = "wedding_hall_change";
        public const string WeddingHallReserve = "wedding_hall_reserve";
        public const string WeddingPropose = "wedding_propose";
        public const string WeddingProposeDecline = "wedding_propose_decline";
        public const string WeddingProposeDeclined = "wedding_propose_declined";
        public const string WorldChampionDamage = "worldchampion_damage";
        public const string WorldChampionReward = "worldchampion_reward";
    }
}
