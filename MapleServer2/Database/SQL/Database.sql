/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

DROP DATABASE IF EXISTS `DATABASE_NAME`;

CREATE DATABASE `DATABASE_NAME`;
USE `DATABASE_NAME`;

--
-- Table structure for table `accounts`
--

DROP TABLE IF EXISTS `accounts`;
CREATE TABLE `accounts` (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `username` varchar(25) NOT NULL,
  `passwordhash` varchar(255) NOT NULL,
  `creationtime` bigint NOT NULL,
  `lastlogintime` bigint NOT NULL,
  `characterslots` int NOT NULL,
  `meret` bigint DEFAULT NULL,
  `gamemeret` bigint DEFAULT NULL,
  `eventmeret` bigint DEFAULT NULL,
  `mesotoken` bigint DEFAULT NULL,
  `bankinventoryid` bigint DEFAULT NULL,
  `vipexpiration` bigint NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `ix_accounts_username` (`username`),
  KEY `accounts_bankinventoryid_fk` (`bankinventoryid`),
  CONSTRAINT `accounts_bankinventoryid_fk` FOREIGN KEY (`bankinventoryid`) REFERENCES `bankinventories` (`id`) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Table structure for table `bankinventories`
--

DROP TABLE IF EXISTS `bankinventories`;
CREATE TABLE `bankinventories` (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `extrasize` int NOT NULL,
  `mesos` bigint DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Table structure for table `buddies`
--

DROP TABLE IF EXISTS `buddies`;
CREATE TABLE `buddies` (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `sharedid` bigint NOT NULL,
  `characterid` bigint DEFAULT NULL,
  `friendcharacterid` bigint DEFAULT NULL,
  `message` varchar(25) DEFAULT '',
  `isfriendrequest` tinyint(1) NOT NULL,
  `ispending` tinyint(1) NOT NULL,
  `blocked` tinyint(1) NOT NULL,
  `blockreason` varchar(25) DEFAULT '',
  `timestamp` bigint NOT NULL,
  PRIMARY KEY (`id`),
  KEY `ix_buddies_friendcharacterid` (`friendcharacterid`),
  KEY `ix_buddies_playercharacterid` (`characterid`),
  CONSTRAINT `fk_buddies_characters_friendcharacterid` FOREIGN KEY (`friendcharacterid`) REFERENCES `characters` (`characterid`) ON DELETE RESTRICT,
  CONSTRAINT `fk_buddies_characters_playercharacterid` FOREIGN KEY (`characterid`) REFERENCES `characters` (`characterid`) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Table structure for table `characters`
--

DROP TABLE IF EXISTS `characters`;
CREATE TABLE `characters` (
  `characterid` bigint NOT NULL AUTO_INCREMENT,
  `accountid` bigint NOT NULL,
  `creationtime` bigint NOT NULL,
  `name` varchar(25) NOT NULL,
  `gender` tinyint unsigned NOT NULL,
  `awakened` tinyint(1) NOT NULL,
  `job` int NOT NULL,
  `levelsid` bigint DEFAULT NULL,
  `mapid` int NOT NULL,
  `titleid` int NOT NULL,
  `insigniaid` smallint NOT NULL,
  `titles` text,
  `prestigerewardsclaimed` text,
  `maxskilltabs` int NOT NULL,
  `activeskilltabid` bigint NOT NULL,
  `gameoptionsid` bigint DEFAULT NULL,
  `walletid` bigint DEFAULT NULL,
  `chatsticker` text,
  `clubid` bigint NOT NULL,
  `coord` text NOT NULL,
  `emotes` text,
  `favoritestickers` text,
  `groupchatid` text,
  `guildapplications` text,
  `guildid` bigint DEFAULT NULL,
  `guildmemberid` bigint DEFAULT NULL,
  `inventoryid` bigint DEFAULT NULL,
  `isdeleted` tinyint(1) NOT NULL DEFAULT '0',
  `mapleopoly` text,
  `motto` varchar(25) DEFAULT '',
  `profileurl` text NOT NULL,
  `returncoord` text NOT NULL,
  `returnmapid` int NOT NULL,
  `skincolor` text NOT NULL,
  `statpointdistribution` text,
  `stats` text,
  `trophycount` text,
  `unlockedmaps` text,
  `unlockedtaxis` text,
  `visitinghomeid` bigint NOT NULL,
  PRIMARY KEY (`characterid`),
  UNIQUE KEY `ix_characters_name` (`name`),
  KEY `ix_characters_accountid` (`accountid`),
  KEY `ix_characters_gameoptionsid` (`gameoptionsid`),
  KEY `ix_characters_guildid` (`guildid`),
  KEY `ix_characters_guildmemberid` (`guildmemberid`),
  KEY `ix_characters_inventoryid` (`inventoryid`),
  KEY `ix_characters_levelsid` (`levelsid`),
  KEY `ix_characters_walletid` (`walletid`),
  CONSTRAINT `fk_characters_accounts_accountid` FOREIGN KEY (`accountid`) REFERENCES `accounts` (`id`) ON DELETE CASCADE,
  CONSTRAINT `fk_characters_gameoptions_gameoptionsid` FOREIGN KEY (`gameoptionsid`) REFERENCES `gameoptions` (`id`) ON DELETE RESTRICT,
  CONSTRAINT `fk_characters_guildmembers_guildmemberid` FOREIGN KEY (`guildmemberid`) REFERENCES `guildmembers` (`id`) ON DELETE RESTRICT,
  CONSTRAINT `fk_characters_guilds_guildid` FOREIGN KEY (`guildid`) REFERENCES `guilds` (`id`) ON DELETE RESTRICT,
  CONSTRAINT `fk_characters_inventories_inventoryid` FOREIGN KEY (`inventoryid`) REFERENCES `inventories` (`id`) ON DELETE RESTRICT,
  CONSTRAINT `fk_characters_levels_levelsid` FOREIGN KEY (`levelsid`) REFERENCES `levels` (`id`) ON DELETE RESTRICT,
  CONSTRAINT `fk_characters_wallets_walletid` FOREIGN KEY (`walletid`) REFERENCES `wallets` (`id`) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Table structure for table `cubes`
--

DROP TABLE IF EXISTS `cubes`;
CREATE TABLE `cubes` (
  `uid` bigint NOT NULL AUTO_INCREMENT,
  `coordx` float NOT NULL,
  `coordy` float NOT NULL,
  `coordz` float NOT NULL,
  `homeid` bigint DEFAULT NULL,
  `itemuid` bigint DEFAULT NULL,
  `layoutuid` bigint DEFAULT NULL,
  `plotnumber` int NOT NULL,
  `rotation` float NOT NULL,
  PRIMARY KEY (`uid`),
  KEY `ix_cubes_homeid` (`homeid`),
  KEY `ix_cubes_itemuid` (`itemuid`),
  KEY `ix_cubes_layoutuid` (`layoutuid`),
  CONSTRAINT `fk_cubes_homelayouts_layoutuid` FOREIGN KEY (`layoutuid`) REFERENCES `homelayouts` (`uid`) ON DELETE RESTRICT,
  CONSTRAINT `fk_cubes_homes_homeid` FOREIGN KEY (`homeid`) REFERENCES `homes` (`id`) ON DELETE RESTRICT,
  CONSTRAINT `fk_cubes_items_itemuid` FOREIGN KEY (`itemuid`) REFERENCES `items` (`uid`) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Table structure for table `gameoptions`
--

DROP TABLE IF EXISTS `gameoptions`;
CREATE TABLE `gameoptions` (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `keybinds` text,
  `activehotbarid` smallint NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Table structure for table `guildapplications`
--

DROP TABLE IF EXISTS `guildapplications`;
CREATE TABLE `guildapplications` (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `guildid` bigint NOT NULL,
  `characterid` bigint NOT NULL,
  `creationtimestamp` bigint NOT NULL,
  PRIMARY KEY (`id`),
  KEY `ix_guildapplications_guildid` (`guildid`),
  CONSTRAINT `fk_guildapplications_guilds_guildid` FOREIGN KEY (`guildid`) REFERENCES `guilds` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Table structure for table `guildmembers`
--

DROP TABLE IF EXISTS `guildmembers`;
CREATE TABLE `guildmembers` (
  `id` bigint NOT NULL,
  `rank` tinyint unsigned NOT NULL,
  `dailycontribution` int NOT NULL,
  `contributiontotal` int NOT NULL,
  `dailydonationcount` tinyint unsigned NOT NULL,
  `attendancetimestamp` bigint NOT NULL,
  `jointimestamp` bigint NOT NULL,
  `guildid` bigint DEFAULT NULL,
  `motto` varchar(50) DEFAULT '',
  PRIMARY KEY (`id`),
  KEY `ix_guildmembers_guildid` (`guildid`),
  CONSTRAINT `fk_guildmembers_characters_id` FOREIGN KEY (`id`) REFERENCES `characters` (`characterid`) ON DELETE RESTRICT,
  CONSTRAINT `fk_guildmembers_guilds_guildid` FOREIGN KEY (`guildid`) REFERENCES `guilds` (`id`) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Table structure for table `guilds`
--

DROP TABLE IF EXISTS `guilds`;
CREATE TABLE `guilds` (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `name` varchar(12) NOT NULL,
  `creationtimestamp` bigint NOT NULL,
  `leaderaccountid` bigint NOT NULL,
  `leadercharacterid` bigint NOT NULL,
  `leadername` varchar(50) NOT NULL,
  `capacity` tinyint unsigned NOT NULL,
  `funds` int NOT NULL,
  `exp` int NOT NULL,
  `searchable` tinyint(1) NOT NULL,
  `buffs` text,
  `emblem` varchar(50) DEFAULT '',
  `focusattributes` int NOT NULL,
  `houserank` int NOT NULL,
  `housetheme` int NOT NULL,
  `notice` varchar(300) DEFAULT '',
  `ranks` text,
  `services` text,
  PRIMARY KEY (`id`),
  KEY `ix_guilds_leadercharacterid` (`leadercharacterid`),
  KEY `ix_guilds_leaderaccountid` (`leaderaccountid`),
  CONSTRAINT `fk_guilds_characters_leadercharacterid` FOREIGN KEY (`leadercharacterid`) REFERENCES `characters` (`characterid`) ON DELETE RESTRICT,
  CONSTRAINT `fk_guilds_characters_leaderaccountid` FOREIGN KEY (`leaderaccountid`) REFERENCES `accounts` (`id`) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Table structure for table `homelayouts`
--

DROP TABLE IF EXISTS `homelayouts`;
CREATE TABLE `homelayouts` (
  `uid` bigint NOT NULL AUTO_INCREMENT,
  `height` tinyint unsigned NOT NULL,
  `homeid` bigint DEFAULT NULL,
  `id` int NOT NULL,
  `name` text,
  `size` tinyint unsigned NOT NULL,
  `timestamp` bigint NOT NULL,
  PRIMARY KEY (`uid`),
  KEY `ix_homelayouts_homeid` (`homeid`),
  CONSTRAINT `fk_homelayouts_homes_homeid` FOREIGN KEY (`homeid`) REFERENCES `homes` (`id`) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Table structure for table `homes`
--

DROP TABLE IF EXISTS `homes`;
CREATE TABLE `homes` (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `accountid` bigint NOT NULL,
  `mapid` int NOT NULL,
  `plotmapid` int NOT NULL,
  `plotnumber` int NOT NULL,
  `apartmentnumber` int NOT NULL,
  `expiration` bigint NOT NULL,
  `name` varchar(16) DEFAULT NULL,
  `description` varchar(100) DEFAULT NULL,
  `size` tinyint unsigned NOT NULL,
  `height` tinyint unsigned NOT NULL,
  `architectscorecurrent` int NOT NULL,
  `architectscoretotal` int NOT NULL,
  `decorationexp` bigint NOT NULL,
  `decorationlevel` bigint NOT NULL,
  `decorationrewardtimestamp` bigint NOT NULL,
  `interiorrewardsclaimed` text,
  `lighting` tinyint unsigned NOT NULL,
  `background` tinyint unsigned NOT NULL,
  `camera` tinyint unsigned NOT NULL,
  `password` text,
  `permissions` text,
  PRIMARY KEY (`id`),
  UNIQUE KEY `ix_homes_accountid` (`accountid`),
  CONSTRAINT `fk_homes_accounts_accountid` FOREIGN KEY (`accountid`) REFERENCES `accounts` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Table structure for table `hotbars`
--

DROP TABLE IF EXISTS `hotbars`;
CREATE TABLE `hotbars` (
  `hotbarid` bigint NOT NULL AUTO_INCREMENT,
  `slots` text,
  `gameoptionsid` bigint DEFAULT NULL,
  PRIMARY KEY (`hotbarid`),
  KEY `ix_hotbars_gameoptionsid` (`gameoptionsid`),
  CONSTRAINT `fk_hotbars_gameoptions_gameoptionsid` FOREIGN KEY (`gameoptionsid`) REFERENCES `gameoptions` (`id`) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Table structure for table `inventories`
--

DROP TABLE IF EXISTS `inventories`;
CREATE TABLE `inventories` (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `extrasize` text,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Table structure for table `items`
--

DROP TABLE IF EXISTS `items`;
CREATE TABLE `items` (
  `uid` bigint NOT NULL AUTO_INCREMENT,
  `level` int NOT NULL,
  `itemslot` tinyint unsigned NOT NULL,
  `rarity` int NOT NULL,
  `playcount` int NOT NULL,
  `amount` int NOT NULL,
  `bankinventoryid` bigint DEFAULT NULL,
  `repackagecount` smallint NOT NULL,
  `charges` int NOT NULL,
  `color` text NOT NULL,
  `creationtime` bigint NOT NULL,
  `enchantexp` int NOT NULL,
  `enchants` int NOT NULL,
  `expirytime` bigint NOT NULL,
  `facedecorationdata` text,
  `gachadismantleid` int NOT NULL,
  `guildid` bigint DEFAULT NULL,
  `hairdata` text NOT NULL,
  `hatdata` text NOT NULL,
  `homeid` bigint DEFAULT NULL,
  `id` int NOT NULL,
  `inventoryid` bigint DEFAULT NULL,
  `isequipped` tinyint(1) NOT NULL,
  `islocked` tinyint(1) NOT NULL,
  `mailuid` int DEFAULT NULL,
  `ownercharacterid` bigint DEFAULT NULL,
  `ownercharactername` varchar(25) DEFAULT '',
  `pairedcharacterid` bigint NOT NULL,
  `pairedcharactername` varchar(25) DEFAULT '',
  `petskinbadgeid` int NOT NULL,
  `remainingglamorforges` smallint NOT NULL,
  `remainingtrades` smallint NOT NULL,
  `score` text,
  `slot` smallint NOT NULL,
  `stats` text,
  `timesattributeschanged` int NOT NULL,
  `transferflag` tinyint unsigned NOT NULL,
  `transparencybadgebools` text,
  `unlocktime` bigint NOT NULL,
  PRIMARY KEY (`uid`),
  KEY `ix_items_bankinventoryid` (`bankinventoryid`),
  KEY `ix_items_guildid` (`guildid`),
  KEY `ix_items_homeid` (`homeid`),
  KEY `ix_items_inventoryid` (`inventoryid`),
  KEY `ix_items_mailuid` (`mailuid`),
  KEY `ix_items_ownercharacterid` (`ownercharacterid`),
  CONSTRAINT `fk_items_bankinventories_bankinventoryid` FOREIGN KEY (`bankinventoryid`) REFERENCES `bankinventories` (`id`) ON DELETE RESTRICT,
  CONSTRAINT `fk_items_characters_ownercharacterid` FOREIGN KEY (`ownercharacterid`) REFERENCES `characters` (`characterid`) ON DELETE RESTRICT,
  CONSTRAINT `fk_items_guilds_guildid` FOREIGN KEY (`guildid`) REFERENCES `guilds` (`id`) ON DELETE RESTRICT,
  CONSTRAINT `fk_items_homes_homeid` FOREIGN KEY (`homeid`) REFERENCES `homes` (`id`) ON DELETE RESTRICT,
  CONSTRAINT `fk_items_inventories_inventoryid` FOREIGN KEY (`inventoryid`) REFERENCES `inventories` (`id`) ON DELETE RESTRICT,
  CONSTRAINT `fk_items_mails_mailuid` FOREIGN KEY (`mailuid`) REFERENCES `mails` (`uid`) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Table structure for table `levels`
--

DROP TABLE IF EXISTS `levels`;
CREATE TABLE `levels` (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `level` smallint NOT NULL,
  `exp` bigint NOT NULL,
  `restexp` bigint NOT NULL,
  `prestigelevel` int NOT NULL,
  `prestigeexp` bigint NOT NULL,
  `masteryexp` text,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Table structure for table `mails`
--

DROP TABLE IF EXISTS `mails`;
CREATE TABLE `mails` (
  `uid` int NOT NULL AUTO_INCREMENT,
  `type` tinyint unsigned NOT NULL,
  `playerid` bigint NOT NULL,
  `sendername` varchar(25) DEFAULT '',
  `title` varchar(25) DEFAULT '',
  `body` text,
  `readtimestamp` bigint NOT NULL,
  `senttimestamp` bigint NOT NULL,
  PRIMARY KEY (`uid`),
  KEY `ix_mails_playerid` (`playerid`),
  CONSTRAINT `fk_mails_characters_playerid` FOREIGN KEY (`playerid`) REFERENCES `characters` (`characterid`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Table structure for table `quests`
--

DROP TABLE IF EXISTS `quests`;
CREATE TABLE `quests` (
  `uid` bigint NOT NULL AUTO_INCREMENT,
  `id` int NOT NULL,
  `started` tinyint(1) NOT NULL,
  `completed` tinyint(1) NOT NULL,
  `starttimestamp` bigint NOT NULL,
  `completetimestamp` bigint NOT NULL,
  `condition` text,
  `characterid` bigint DEFAULT NULL,
  `tracked` tinyint(1) NOT NULL,
  PRIMARY KEY (`uid`),
  KEY `ix_quests_characterid` (`characterid`),
  CONSTRAINT `fk_quests_characters_characterid` FOREIGN KEY (`characterid`) REFERENCES `characters` (`characterid`) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Table structure for table `skilltabs`
--

DROP TABLE IF EXISTS `skilltabs`;
CREATE TABLE `skilltabs` (
  `uid` bigint NOT NULL AUTO_INCREMENT,
  `tabid` bigint NOT NULL,
  `name` varchar(25) DEFAULT '',
  `skilllevels` text,
  `characterid` bigint DEFAULT NULL,
  PRIMARY KEY (`uid`),
  KEY `ix_skilltabs_characterid` (`characterid`),
  CONSTRAINT `fk_skilltabs_characters_characterid` FOREIGN KEY (`characterid`) REFERENCES `characters` (`characterid`) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Table structure for table `trophies`
--

DROP TABLE IF EXISTS `trophies`;
CREATE TABLE `trophies` (
  `uid` bigint NOT NULL AUTO_INCREMENT,
  `id` int NOT NULL,
  `nextgrade` int NOT NULL,
  `counter` bigint NOT NULL,
  `isdone` tinyint(1) NOT NULL,
  `lastreward` tinyint unsigned NOT NULL,
  `timestamps` text,
  `characterid` bigint DEFAULT NULL,
  `accountid` bigint DEFAULT NULL,
  PRIMARY KEY (`uid`),
  KEY `ix_trophies_characterid` (`characterid`),
  KEY `ix_trophies_accountid` (`accountid`),
  CONSTRAINT `fk_trophies_characters_characterid` FOREIGN KEY (`characterid`) REFERENCES `characters` (`characterid`) ON DELETE RESTRICT,
  CONSTRAINT `fk_trophies_account_accountid` FOREIGN KEY (`accountid`) REFERENCES `accounts` (`id`) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Table structure for table `wallets`
--

DROP TABLE IF EXISTS `wallets`;
CREATE TABLE `wallets` (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `meso` bigint DEFAULT NULL,
  `valortoken` bigint DEFAULT NULL,
  `treva` bigint DEFAULT NULL,
  `rue` bigint DEFAULT NULL,
  `havifruit` bigint DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;