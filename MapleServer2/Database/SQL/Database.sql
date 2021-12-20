/*!40101 SET @OLD_CHARACTER_SET_CLIENT = @@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS = @@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION = @@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE = @@TIME_ZONE */;
/*!40103 SET TIME_ZONE = '+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS = @@UNIQUE_CHECKS, UNIQUE_CHECKS = 0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS = @@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS = 0 */;
/*!40101 SET @OLD_SQL_MODE = @@SQL_MODE, SQL_MODE = 'NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES = @@SQL_NOTES, SQL_NOTES = 0 */;

DROP DATABASE IF EXISTS `DATABASE_NAME`;

CREATE DATABASE `DATABASE_NAME`;
USE `DATABASE_NAME`;

--
-- Table structure for table `accounts`
--

DROP TABLE IF EXISTS `accounts`;
CREATE TABLE `accounts`
(
    `id`                            bigint       NOT NULL AUTO_INCREMENT,
    `username`                      varchar(25)  NOT NULL,
    `password_hash`                 varchar(255) NOT NULL,
    `creation_time`                 bigint       NOT NULL,
    `last_login_time`               bigint       NOT NULL,
    `character_slots`               int          NOT NULL,
    `meret`                         bigint DEFAULT NULL,
    `game_meret`                    bigint DEFAULT NULL,
    `event_meret`                   bigint DEFAULT NULL,
    `meso_token`                    bigint DEFAULT NULL,
    `bank_inventory_id`             bigint DEFAULT NULL,
    `vip_expiration`                bigint       NOT NULL,
    `meso_market_daily_listings`    int          NOT NULL,
    `meso_market_monthly_purchases` int          NOT NULL,
    PRIMARY KEY (`id`),
    UNIQUE KEY `ix_accounts_username` (`username`),
    KEY `accounts_bankinventoryid_fk` (`bank_inventory_id`),
    CONSTRAINT `accounts_bankinventoryid_fk` FOREIGN KEY (`bank_inventory_id`) REFERENCES `bank_inventories` (`id`) ON DELETE RESTRICT
) ENGINE = InnoDB
  DEFAULT CHARSET = utf8mb4
  COLLATE = utf8mb4_0900_ai_ci;

--
-- Table structure for table `auth_data`
--

DROP TABLE IF EXISTS `auth_data`;
CREATE TABLE `auth_data`
(
    `account_id`          bigint NOT NULL,
    `token_a`             INT    NOT NULL,
    `token_b`             INT    NOT NULL,
    `online_character_id` bigint NULL,
    PRIMARY KEY (`account_id`),
    CONSTRAINT `auth_data_FK` FOREIGN KEY (`account_id`) REFERENCES `accounts` (`id`),
    CONSTRAINT `auth_data_FK_1` FOREIGN KEY (`online_character_id`) REFERENCES `characters` (`character_id`)
) ENGINE = InnoDB
  DEFAULT CHARSET = utf8mb4
  COLLATE = utf8mb4_0900_ai_ci;

--
-- Table structure for table `bankinventories`
--

DROP TABLE IF EXISTS `bank_inventories`;
CREATE TABLE `bank_inventories`
(
    `id`         bigint NOT NULL AUTO_INCREMENT,
    `extra_size` int    NOT NULL,
    `mesos`      bigint DEFAULT NULL,
    PRIMARY KEY (`id`)
) ENGINE = InnoDB
  DEFAULT CHARSET = utf8mb4
  COLLATE = utf8mb4_0900_ai_ci;

--
-- Table structure for table `buddies`
--

DROP TABLE IF EXISTS `buddies`;
CREATE TABLE `buddies`
(
    `id`                  bigint     NOT NULL AUTO_INCREMENT,
    `shared_id`           bigint     NOT NULL,
    `character_id`        bigint      DEFAULT NULL,
    `friend_character_id` bigint      DEFAULT NULL,
    `message`             varchar(25) DEFAULT '',
    `is_friend_request`   tinyint(1) NOT NULL,
    `is_pending`          tinyint(1) NOT NULL,
    `blocked`             tinyint(1) NOT NULL,
    `block_reason`        varchar(25) DEFAULT '',
    `timestamp`           bigint     NOT NULL,
    PRIMARY KEY (`id`),
    KEY `ix_buddies_friendcharacterid` (`friend_character_id`),
    KEY `ix_buddies_playercharacterid` (`character_id`),
    CONSTRAINT `fk_buddies_characters_friendcharacterid` FOREIGN KEY (`friend_character_id`) REFERENCES `characters` (`character_id`) ON DELETE RESTRICT,
    CONSTRAINT `fk_buddies_characters_playercharacterid` FOREIGN KEY (`character_id`) REFERENCES `characters` (`character_id`) ON DELETE RESTRICT
) ENGINE = InnoDB
  DEFAULT CHARSET = utf8mb4
  COLLATE = utf8mb4_0900_ai_ci;

--
-- Table structure for table `characters`
--

DROP TABLE IF EXISTS `characters`;
CREATE TABLE `characters`
(
    `character_id`             bigint           NOT NULL AUTO_INCREMENT,
    `account_id`               bigint           NOT NULL,
    `creation_time`            bigint           NOT NULL,
    `last_login_time`          bigint           NOT NULL,
    `name`                     varchar(25)      NOT NULL,
    `gender`                   tinyint unsigned NOT NULL,
    `awakened`                 tinyint(1)       NOT NULL,
    `channel_id`               smallint         NOT NULL,
    `instance_id`              bigint           NOT NULL,
    `is_migrating`             tinyint(1)       NOT NULL,
    `job`                      int              NOT NULL,
    `levels_id`                bigint                    DEFAULT NULL,
    `map_id`                   int              NOT NULL,
    `title_id`                 int              NOT NULL,
    `insignia_id`              smallint         NOT NULL,
    `titles`                   text,
    `prestige_rewards_claimed` text,
    `max_skill_tabs`           int              NOT NULL,
    `active_skill_tab_id`      bigint           NOT NULL,
    `game_options_id`          bigint                    DEFAULT NULL,
    `wallet_id`                bigint                    DEFAULT NULL,
    `chat_sticker`             text,
    `club_id`                  bigint           NOT NULL,
    `coord`                    text             NOT NULL,
    `emotes`                   text,
    `favorite_stickers`        text,
    `group_chat_id`            text,
    `guild_applications`       text,
    `guild_id`                 bigint                    DEFAULT NULL,
    `guild_member_id`          bigint                    DEFAULT NULL,
    `inventory_id`             bigint                    DEFAULT NULL,
    `is_deleted`               tinyint(1)       NOT NULL DEFAULT '0',
    `mapleopoly`               text,
    `motto`                    varchar(25)               DEFAULT '',
    `profile_url`              text             NOT NULL,
    `return_coord`             text             NOT NULL,
    `return_map_id`            int              NOT NULL,
    `skin_color`               text             NOT NULL,
    `statpoint_distribution`   text,
    `stats`                    text,
    `trophy_count`             text,
    `unlocked_maps`            text,
    `unlocked_taxis`           text,
    `visiting_home_id`         bigint           NOT NULL,
    `gathering_count`          text,
    PRIMARY KEY (`character_id`),
    UNIQUE KEY `ix_characters_name` (`name`),
    KEY `ix_characters_accountid` (`account_id`),
    KEY `ix_characters_gameoptionsid` (`game_options_id`),
    KEY `ix_characters_guildid` (`guild_id`),
    KEY `ix_characters_guildmemberid` (`guild_member_id`),
    KEY `ix_characters_inventoryid` (`inventory_id`),
    KEY `ix_characters_levelsid` (`levels_id`),
    KEY `ix_characters_walletid` (`wallet_id`),
    CONSTRAINT `fk_characters_accounts_accountid` FOREIGN KEY (`account_id`) REFERENCES `accounts` (`id`) ON DELETE CASCADE,
    CONSTRAINT `fk_characters_gameoptions_gameoptionsid` FOREIGN KEY (`game_options_id`) REFERENCES `game_options` (`id`) ON DELETE RESTRICT,
    CONSTRAINT `fk_characters_guildmembers_guildmemberid` FOREIGN KEY (`guild_member_id`) REFERENCES `guild_members` (`id`) ON DELETE RESTRICT,
    CONSTRAINT `fk_characters_guilds_guildid` FOREIGN KEY (`guild_id`) REFERENCES `guilds` (`id`) ON DELETE RESTRICT,
    CONSTRAINT `fk_characters_inventories_inventoryid` FOREIGN KEY (`inventory_id`) REFERENCES `inventories` (`id`) ON DELETE RESTRICT,
    CONSTRAINT `fk_characters_levels_levelsid` FOREIGN KEY (`levels_id`) REFERENCES `levels` (`id`) ON DELETE RESTRICT,
    CONSTRAINT `fk_characters_wallets_walletid` FOREIGN KEY (`wallet_id`) REFERENCES `wallets` (`id`) ON DELETE RESTRICT
) ENGINE = InnoDB
  DEFAULT CHARSET = utf8mb4
  COLLATE = utf8mb4_0900_ai_ci;

--
-- Table structure for table `cubes`
--

DROP TABLE IF EXISTS `cubes`;
CREATE TABLE `cubes`
(
    `uid`             bigint NOT NULL AUTO_INCREMENT,
    `coord_x`         float  NOT NULL,
    `coord_y`         float  NOT NULL,
    `coord_z`         float  NOT NULL,
    `home_id`         bigint DEFAULT NULL,
    `item_uid`        bigint DEFAULT NULL,
    `layout_uid`      bigint DEFAULT NULL,
    `plot_number`     int    NOT NULL,
    `rotation`        float  NOT NULL,
    `portal_settings` text,
    PRIMARY KEY (`uid`),
    KEY `ix_cubes_homeid` (`home_id`),
    KEY `ix_cubes_itemuid` (`item_uid`),
    KEY `ix_cubes_layoutuid` (`layout_uid`),
    CONSTRAINT `fk_cubes_homelayouts_layoutuid` FOREIGN KEY (`layout_uid`) REFERENCES `home_layouts` (`uid`) ON DELETE RESTRICT,
    CONSTRAINT `fk_cubes_homes_homeid` FOREIGN KEY (`home_id`) REFERENCES `homes` (`id`) ON DELETE RESTRICT,
    CONSTRAINT `fk_cubes_items_itemuid` FOREIGN KEY (`item_uid`) REFERENCES `items` (`uid`) ON DELETE RESTRICT
) ENGINE = InnoDB
  DEFAULT CHARSET = utf8mb4
  COLLATE = utf8mb4_0900_ai_ci;

--
-- Table structure for table `gameoptions`
--

DROP TABLE IF EXISTS `game_options`;
CREATE TABLE `game_options`
(
    `id`               bigint   NOT NULL AUTO_INCREMENT,
    `keybinds`         text,
    `active_hotbar_id` smallint NOT NULL,
    PRIMARY KEY (`id`)
) ENGINE = InnoDB
  DEFAULT CHARSET = utf8mb4
  COLLATE = utf8mb4_0900_ai_ci;

--
-- Table structure for table `guildapplications`
--

DROP TABLE IF EXISTS `guild_applications`;
CREATE TABLE `guild_applications`
(
    `id`                 bigint NOT NULL AUTO_INCREMENT,
    `guild_id`           bigint NOT NULL,
    `character_id`       bigint NOT NULL,
    `creation_timestamp` bigint NOT NULL,
    PRIMARY KEY (`id`),
    KEY `ix_guildapplications_guildid` (`guild_id`),
    CONSTRAINT `fk_guildapplications_guilds_guildid` FOREIGN KEY (`guild_id`) REFERENCES `guilds` (`id`) ON DELETE CASCADE
) ENGINE = InnoDB
  DEFAULT CHARSET = utf8mb4
  COLLATE = utf8mb4_0900_ai_ci;

--
-- Table structure for table `guildmembers`
--

DROP TABLE IF EXISTS `guild_members`;
CREATE TABLE `guild_members`
(
    `id`                   bigint           NOT NULL,
    `rank`                 tinyint unsigned NOT NULL,
    `daily_contribution`   int              NOT NULL,
    `contribution_total`   int              NOT NULL,
    `daily_donation_count` tinyint unsigned NOT NULL,
    `attendance_timestamp` bigint           NOT NULL,
    `join_timestamp`       bigint           NOT NULL,
    `last_login_timestamp` bigint           NOT NULL,
    `guild_id`             bigint      DEFAULT NULL,
    `motto`                varchar(50) DEFAULT '',
    PRIMARY KEY (`id`),
    KEY `ix_guildmembers_guildid` (`guild_id`),
    CONSTRAINT `fk_guildmembers_characters_id` FOREIGN KEY (`id`) REFERENCES `characters` (`character_id`) ON DELETE RESTRICT,
    CONSTRAINT `fk_guildmembers_guilds_guildid` FOREIGN KEY (`guild_id`) REFERENCES `guilds` (`id`) ON DELETE RESTRICT
) ENGINE = InnoDB
  DEFAULT CHARSET = utf8mb4
  COLLATE = utf8mb4_0900_ai_ci;

--
-- Table structure for table `guilds`
--

DROP TABLE IF EXISTS `guilds`;
CREATE TABLE `guilds`
(
    `id`                  bigint           NOT NULL AUTO_INCREMENT,
    `name`                varchar(12)      NOT NULL,
    `creation_timestamp`  bigint           NOT NULL,
    `leader_account_id`   bigint           NOT NULL,
    `leader_character_id` bigint           NOT NULL,
    `leader_name`         varchar(50)      NOT NULL,
    `capacity`            tinyint unsigned NOT NULL,
    `funds`               int              NOT NULL,
    `exp`                 int              NOT NULL,
    `searchable`          tinyint(1)       NOT NULL,
    `buffs`               text,
    `emblem`              varchar(50)  DEFAULT '',
    `focus_attributes`    int              NOT NULL,
    `house_rank`          int              NOT NULL,
    `house_theme`         int              NOT NULL,
    `notice`              varchar(300) DEFAULT '',
    `ranks`               text,
    `services`            text,
    PRIMARY KEY (`id`),
    KEY `ix_guilds_leadercharacterid` (`leader_character_id`),
    KEY `ix_guilds_leaderaccountid` (`leader_account_id`),
    CONSTRAINT `fk_guilds_characters_leadercharacterid` FOREIGN KEY (`leader_character_id`) REFERENCES `characters` (`character_id`) ON DELETE RESTRICT,
    CONSTRAINT `fk_guilds_characters_leaderaccountid` FOREIGN KEY (`leader_account_id`) REFERENCES `accounts` (`id`) ON DELETE RESTRICT
) ENGINE = InnoDB
  DEFAULT CHARSET = utf8mb4
  COLLATE = utf8mb4_0900_ai_ci;

--
-- Table structure for table `homelayouts`
--

DROP TABLE IF EXISTS `home_layouts`;
CREATE TABLE `home_layouts`
(
    `uid`       bigint           NOT NULL AUTO_INCREMENT,
    `height`    tinyint unsigned NOT NULL,
    `home_id`   bigint DEFAULT NULL,
    `id`        int              NOT NULL,
    `name`      text,
    `size`      tinyint unsigned NOT NULL,
    `timestamp` bigint           NOT NULL,
    PRIMARY KEY (`uid`),
    KEY `ix_homelayouts_homeid` (`home_id`),
    CONSTRAINT `fk_homelayouts_homes_homeid` FOREIGN KEY (`home_id`) REFERENCES `homes` (`id`) ON DELETE RESTRICT
) ENGINE = InnoDB
  DEFAULT CHARSET = utf8mb4
  COLLATE = utf8mb4_0900_ai_ci;

--
-- Table structure for table `homes`
--

DROP TABLE IF EXISTS `homes`;
CREATE TABLE `homes`
(
    `id`                          bigint           NOT NULL AUTO_INCREMENT,
    `account_id`                  bigint           NOT NULL,
    `map_id`                      int              NOT NULL,
    `plot_map_id`                 int              NOT NULL,
    `plot_number`                 int              NOT NULL,
    `apartment_number`            int              NOT NULL,
    `expiration`                  bigint           NOT NULL,
    `name`                        varchar(16)  DEFAULT NULL,
    `description`                 varchar(100) DEFAULT NULL,
    `size`                        tinyint unsigned NOT NULL,
    `height`                      tinyint unsigned NOT NULL,
    `architect_score_current`     int              NOT NULL,
    `architect_score_total`       int              NOT NULL,
    `decoration_exp`              bigint           NOT NULL,
    `decoration_level`            bigint           NOT NULL,
    `decoration_reward_timestamp` bigint           NOT NULL,
    `interior_rewards_claimed`    text,
    `lighting`                    tinyint unsigned NOT NULL,
    `background`                  tinyint unsigned NOT NULL,
    `camera`                      tinyint unsigned NOT NULL,
    `password`                    text,
    `permissions`                 text,
    PRIMARY KEY (`id`),
    UNIQUE KEY `ix_homes_accountid` (`account_id`),
    CONSTRAINT `fk_homes_accounts_accountid` FOREIGN KEY (`account_id`) REFERENCES `accounts` (`id`) ON DELETE CASCADE
) ENGINE = InnoDB
  DEFAULT CHARSET = utf8mb4
  COLLATE = utf8mb4_0900_ai_ci;

--
-- Table structure for table `hotbars`
--

DROP TABLE IF EXISTS `hotbars`;
CREATE TABLE `hotbars`
(
    `hotbar_id`       bigint NOT NULL AUTO_INCREMENT,
    `slots`           text,
    `game_options_id` bigint DEFAULT NULL,
    PRIMARY KEY (`hotbar_id`),
    KEY `ix_hotbars_gameoptionsid` (`game_options_id`),
    CONSTRAINT `fk_hotbars_gameoptions_gameoptionsid` FOREIGN KEY (`game_options_id`) REFERENCES `game_options` (`id`) ON DELETE RESTRICT
) ENGINE = InnoDB
  DEFAULT CHARSET = utf8mb4
  COLLATE = utf8mb4_0900_ai_ci;

--
-- Table structure for table `inventories`
--

DROP TABLE IF EXISTS `inventories`;
CREATE TABLE `inventories`
(
    `id`         bigint NOT NULL AUTO_INCREMENT,
    `extra_size` text,
    PRIMARY KEY (`id`)
) ENGINE = InnoDB
  DEFAULT CHARSET = utf8mb4
  COLLATE = utf8mb4_0900_ai_ci;

--
-- Table structure for table `items`
--

DROP TABLE IF EXISTS `items`;
CREATE TABLE `items`
(
    `uid`                      bigint           NOT NULL AUTO_INCREMENT,
    `name`                     text        DEFAULT '',
    `level`                    int              NOT NULL,
    `item_slot`                tinyint unsigned NOT NULL,
    `gem_slot`                 tinyint unsigned NOT NULL,
    `rarity`                   int              NOT NULL,
    `play_count`               int              NOT NULL,
    `amount`                   int              NOT NULL,
    `bank_inventory_id`        bigint      DEFAULT NULL,
    `repackage_count`          smallint         NOT NULL,
    `charges`                  int              NOT NULL,
    `color`                    text             NOT NULL,
    `creation_time`            bigint           NOT NULL,
    `enchant_exp`              int              NOT NULL,
    `enchants`                 int              NOT NULL,
    `expiry_time`              bigint           NOT NULL,
    `face_decoration_data`     text,
    `gacha_dismantle_id`       int              NOT NULL,
    `guild_id`                 bigint      DEFAULT NULL,
    `hair_data`                text             NOT NULL,
    `hat_data`                 text             NOT NULL,
    `home_id`                  bigint      DEFAULT NULL,
    `id`                       int              NOT NULL,
    `inventory_id`             bigint      DEFAULT NULL,
    `is_equipped`              tinyint(1)       NOT NULL,
    `is_locked`                tinyint(1)       NOT NULL,
    `is_template`              tinyint(1)       NOT NULL,
    `mail_id`                  bigint      DEFAULT NULL,
    `owner_character_id`       bigint      DEFAULT NULL,
    `owner_character_name`     varchar(25) DEFAULT '',
    `paired_character_id`      bigint           NOT NULL,
    `paired_character_name`    varchar(25) DEFAULT '',
    `pet_skin_badge_id`        int              NOT NULL,
    `remaining_glamor_forges`  smallint         NOT NULL,
    `remaining_trades`         smallint         NOT NULL,
    `score`                    text,
    `slot`                     smallint         NOT NULL,
    `stats`                    text,
    `times_attributes_changed` int              NOT NULL,
    `transfer_flag`            tinyint unsigned NOT NULL,
    `transparency_badge_bools` text,
    `unlock_time`              bigint           NOT NULL,
    `blackmarket_category`     text,
    `ugc_uid`                  bigint           NULL,
    PRIMARY KEY (`uid`),
    KEY `ix_items_bankinventoryid` (`bank_inventory_id`),
    KEY `ix_items_guildid` (`guild_id`),
    KEY `ix_items_homeid` (`home_id`),
    KEY `ix_items_inventoryid` (`inventory_id`),
    KEY `ix_items_mailid` (`mail_id`),
    KEY `ix_items_ownercharacterid` (`owner_character_id`),
    KEY `ix_items_ugcuid` (`ugc_uid`),
    CONSTRAINT `fk_items_bankinventories_bankinventoryid` FOREIGN KEY (`bank_inventory_id`) REFERENCES `bank_inventories` (`id`) ON DELETE RESTRICT,
    CONSTRAINT `fk_items_characters_ownercharacterid` FOREIGN KEY (`owner_character_id`) REFERENCES `characters` (`character_id`) ON DELETE RESTRICT,
    CONSTRAINT `fk_items_guilds_guildid` FOREIGN KEY (`guild_id`) REFERENCES `guilds` (`id`) ON DELETE RESTRICT,
    CONSTRAINT `fk_items_homes_homeid` FOREIGN KEY (`home_id`) REFERENCES `homes` (`id`) ON DELETE RESTRICT,
    CONSTRAINT `fk_items_inventories_inventoryid` FOREIGN KEY (`inventory_id`) REFERENCES `inventories` (`id`) ON DELETE RESTRICT,
    CONSTRAINT `fk_items_mails_mailid` FOREIGN KEY (`mail_id`) REFERENCES `mails` (`id`) ON DELETE RESTRICT,
    CONSTRAINT `fk_items_ugc_ugcuid` FOREIGN KEY (`ugc_uid`) REFERENCES `ugc` (`uid`) ON DELETE RESTRICT
) ENGINE = InnoDB
  DEFAULT CHARSET = utf8mb4
  COLLATE = utf8mb4_0900_ai_ci;

--
-- Table structure for table `levels`
--

DROP TABLE IF EXISTS `levels`;
CREATE TABLE `levels`
(
    `id`             bigint   NOT NULL AUTO_INCREMENT,
    `level`          smallint NOT NULL,
    `exp`            bigint   NOT NULL,
    `rest_exp`       bigint   NOT NULL,
    `prestige_level` int      NOT NULL,
    `prestige_exp`   bigint   NOT NULL,
    `mastery_exp`    text,
    PRIMARY KEY (`id`)
) ENGINE = InnoDB
  DEFAULT CHARSET = utf8mb4
  COLLATE = utf8mb4_0900_ai_ci;

--
-- Table structure for table `mails`
--

DROP TABLE IF EXISTS `mails`;
CREATE TABLE `mails`
(
    `id`                     bigint           NOT NULL AUTO_INCREMENT,
    `type`                   tinyint unsigned NOT NULL,
    `recipient_character_id` bigint           NOT NULL,
    `sender_character_id`    bigint           NOT NULL,
    `sender_name`            text,
    `title`                  text,
    `body`                   text,
    `read_timestamp`         bigint           NOT NULL,
    `sent_timestamp`         bigint           NOT NULL,
    `expiry_timestamp`       bigint           NOT NULL,
    `mesos`                  bigint           NOT NULL,
    `merets`                 bigint           NOT NULL,
    `additional_parameter1`  text DEFAULT '',
    `additional_parameter2`  text DEFAULT '',
    PRIMARY KEY (`id`),
    KEY `ix_mails_playerid` (`recipient_character_id`),
    CONSTRAINT `fk_mails_recipient_character_id` FOREIGN KEY (`recipient_character_id`) REFERENCES `characters` (`character_id`) ON DELETE CASCADE
) ENGINE = InnoDB
  DEFAULT CHARSET = utf8mb4
  COLLATE = utf8mb4_0900_ai_ci;

--
-- Table structure for table `quests`
--

DROP TABLE IF EXISTS `quests`;
CREATE TABLE `quests`
(
    `uid`                bigint           NOT NULL AUTO_INCREMENT,
    `id`                 int              NOT NULL,
    `state`              tinyint unsigned NOT NULL,
    `start_timestamp`    bigint           NOT NULL,
    `complete_timestamp` bigint           NOT NULL,
    `condition`          text,
    `character_id`       bigint DEFAULT NULL,
    `tracked`            tinyint(1)       NOT NULL,
    PRIMARY KEY (`uid`),
    KEY `ix_quests_characterid` (`character_id`),
    CONSTRAINT `fk_quests_characters_characterid` FOREIGN KEY (`character_id`) REFERENCES `characters` (`character_id`) ON DELETE RESTRICT
) ENGINE = InnoDB
  DEFAULT CHARSET = utf8mb4
  COLLATE = utf8mb4_0900_ai_ci;

--
-- Table structure for table `server`
--

DROP TABLE IF EXISTS `server`;
CREATE TABLE `server`
(
    last_daily_reset datetime NOT NULL
) ENGINE = InnoDB
  DEFAULT CHARSET = utf8mb4
  COLLATE = utf8mb4_0900_ai_ci;

INSERT INTO `server`
VALUES ('2021-01-01 00:00:00');
--
-- Table structure for table `skilltabs`
--

DROP TABLE IF EXISTS `skill_tabs`;
CREATE TABLE `skill_tabs`
(
    `uid`          bigint NOT NULL AUTO_INCREMENT,
    `tab_id`       bigint NOT NULL,
    `name`         varchar(25) DEFAULT '',
    `skill_levels` text,
    `character_id` bigint      DEFAULT NULL,
    PRIMARY KEY (`uid`),
    KEY `ix_skilltabs_characterid` (`character_id`),
    CONSTRAINT `fk_skilltabs_characters_characterid` FOREIGN KEY (`character_id`) REFERENCES `characters` (`character_id`) ON DELETE RESTRICT
) ENGINE = InnoDB
  DEFAULT CHARSET = utf8mb4
  COLLATE = utf8mb4_0900_ai_ci;

--
-- Table structure for table `trophies`
--

DROP TABLE IF EXISTS `trophies`;
CREATE TABLE `trophies`
(
    `uid`          bigint           NOT NULL AUTO_INCREMENT,
    `id`           int              NOT NULL,
    `next_grade`   int              NOT NULL,
    `counter`      bigint           NOT NULL,
    `is_done`      tinyint(1)       NOT NULL,
    `last_reward`  tinyint unsigned NOT NULL,
    `timestamps`   text,
    `character_id` bigint DEFAULT NULL,
    `account_id`   bigint DEFAULT NULL,
    PRIMARY KEY (`uid`),
    KEY `ix_trophies_characterid` (`character_id`),
    KEY `ix_trophies_accountid` (`account_id`),
    CONSTRAINT `fk_trophies_characters_characterid` FOREIGN KEY (`character_id`) REFERENCES `characters` (`character_id`) ON DELETE RESTRICT,
    CONSTRAINT `fk_trophies_account_accountid` FOREIGN KEY (`account_id`) REFERENCES `accounts` (`id`) ON DELETE RESTRICT
) ENGINE = InnoDB
  DEFAULT CHARSET = utf8mb4
  COLLATE = utf8mb4_0900_ai_ci;

--
-- Table structure for table `wallets`
--

DROP TABLE IF EXISTS `wallets`;
CREATE TABLE `wallets`
(
    `id`          bigint NOT NULL AUTO_INCREMENT,
    `meso`        bigint DEFAULT NULL,
    `valor_token` bigint DEFAULT NULL,
    `treva`       bigint DEFAULT NULL,
    `rue`         bigint DEFAULT NULL,
    `havi_fruit`  bigint DEFAULT NULL,
    PRIMARY KEY (`id`)
) ENGINE = InnoDB
  DEFAULT CHARSET = utf8mb4
  COLLATE = utf8mb4_0900_ai_ci;

--
-- Table structure for table `black_market_listings`
--
DROP TABLE IF EXISTS `black_market_listings`;
CREATE TABLE `black_market_listings`
(
    `id`                 bigint NOT NULL AUTO_INCREMENT,
    `listing_timestamp`  bigint NOT NULL,
    `expiry_timestamp`   bigint NOT NULL,
    `price`              bigint NOT NULL,
    `deposit`            bigint NOT NULL,
    `item_uid`           bigint DEFAULT NULL,
    `listed_quantity`    int    DEFAULT NULL,
    `owner_account_id`   bigint DEFAULT NULL,
    `owner_character_id` bigint DEFAULT NULL,
    PRIMARY KEY (`id`),
    KEY `ix_black_market_listings_accountid` (`owner_account_id`),
    KEY `ix_black_market_listings_characterid` (`owner_character_id`),
    KEY `ix_black_market_listings_item_uid` (`item_uid`),
    CONSTRAINT `fk_black_market_account_accountid` FOREIGN KEY (`owner_account_id`) REFERENCES `accounts` (`id`) ON DELETE RESTRICT,
    CONSTRAINT `fk_black_market_characters_characterid` FOREIGN KEY (`owner_character_id`) REFERENCES `characters` (`character_id`) ON DELETE RESTRICT,
    CONSTRAINT `fk_black_market_items_itemuid` FOREIGN KEY (`item_uid`) REFERENCES `items` (`uid`) ON DELETE RESTRICT
) ENGINE = InnoDB
  DEFAULT CHARSET = utf8mb4
  COLLATE = utf8mb4_0900_ai_ci;

--
-- Table structure for table `meso_market_listings`
--

DROP TABLE IF EXISTS `meso_market_listings`;
CREATE TABLE `meso_market_listings`
(
    `id`                 bigint NOT NULL AUTO_INCREMENT,
    `listing_timestamp`  bigint NOT NULL,
    `expiry_timestamp`   bigint NOT NULL,
    `price`              bigint NOT NULL,
    `mesos`              bigint NOT NULL,
    `owner_account_id`   bigint DEFAULT NULL,
    `owner_character_id` bigint DEFAULT NULL,
    PRIMARY KEY (`id`),
    KEY `ix_meso_market_listings_accountid` (`owner_account_id`),
    KEY `ix_meso_market_listings_characterid` (`owner_character_id`),
    CONSTRAINT `fk_meso_market_account_accountid` FOREIGN KEY (`owner_account_id`) REFERENCES `accounts` (`id`) ON DELETE RESTRICT,
    CONSTRAINT `fk_meso_market_characters_characterid` FOREIGN KEY (`owner_character_id`) REFERENCES `characters` (`character_id`) ON DELETE RESTRICT
) ENGINE = InnoDB
  DEFAULT CHARSET = utf8mb4
  COLLATE = utf8mb4_0900_ai_ci;

--
-- Table structure for table `ugc`
--

DROP TABLE IF EXISTS `ugc`;
CREATE TABLE `ugc`
(
    `uid`            bigint       NOT NULL AUTO_INCREMENT,
    `guid`           varchar(100) NOT NULL,
    `name`           varchar(100) NOT NULL,
    `url`            varchar(200) NOT NULL,
    `character_id`   bigint       NOT NULL,
    `character_name` varchar(100) NOT NULL,
    `account_id`     bigint       NOT NULL,
    `creation_time`  bigint       NOT NULL,
    `sale_price`     bigint       NOT NULL,
    PRIMARY KEY (`uid`),
    KEY `ix_ugc_account_id` (`account_id`),
    KEY `ix_ugc_character_id` (`character_id`),
    CONSTRAINT `ugc_FK` FOREIGN KEY (`character_id`) REFERENCES `characters` (`character_id`),
    CONSTRAINT `ugc_FK_1` FOREIGN KEY (`account_id`) REFERENCES `accounts` (`id`)
) ENGINE = InnoDB
  DEFAULT CHARSET = utf8mb4
  COLLATE = utf8mb4_0900_ai_ci
  AUTO_INCREMENT = 1000;

  --
  -- Table structure for table `ugc_market_items`
  --

  DROP TABLE IF EXISTS `ugc_market_items`;
  CREATE TABLE `ugc_market_items` (
    `id`                        bigint NOT NULL AUTO_INCREMENT,
    `price`                     bigint NOT NULL,
    `item_uid`                  bigint NOT NULL,
    `status`                    tinyint NOT NULL,
    `creation_time`             bigint NOT NULL,
    `listing_expiration_time`   bigint NOT NULL,
    `promotion_expiration_time` bigint NOT NULL,
    `seller_account_id`         bigint NOT NULL,
    `seller_character_id`       bigint NOT NULL,
    `seller_character_name`     varchar(100) NOT NULL,
    `description`               text,
    `sales_count`               int NOT NULL,
    `tags`                      text,
    PRIMARY KEY (`id`),
    KEY `ix_ugc_market_listing_account_id` (`seller_account_id`),
    KEY `ix_ugc_market_listing_character_id` (`seller_character_id`),
    CONSTRAINT `fk_ugc_market_listing_character_id` FOREIGN KEY (`seller_character_id`) REFERENCES `characters` (`character_id`),
    CONSTRAINT `fk_ugc_market_listing_account_id` FOREIGN KEY (`seller_account_id`) REFERENCES `characters` (`character_id`)
  ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


  --
  -- Table structure for table `ugc_market_sales`
  --

  DROP TABLE IF EXISTS `ugc_market_sales`;
  CREATE TABLE `ugc_market_sales` (
    `id`                   bigint NOT NULL AUTO_INCREMENT,
    `price`                bigint NOT NULL,
    `profit`               bigint NOT NULL,
    `item_name`            varchar(100) NOT NULL,
    `sold_timestamp`       bigint NOT NULL,
    `seller_character_id`  bigint NOT NULL,
    PRIMARY KEY (`id`),
    KEY `ix_ugc_market_sale_character_id` (`seller_character_id`),
    CONSTRAINT `fk_ugc_market_sale_character_id` FOREIGN KEY (`seller_character_id`) REFERENCES `characters` (`character_id`)
  ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*!40103 SET TIME_ZONE = @OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE = @OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS = @OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS = @OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT = @OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS = @OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION = @OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES = @OLD_SQL_NOTES */;
