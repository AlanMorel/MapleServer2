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

DROP DATABASE IF EXISTS `Maple2DB`;

CREATE DATABASE `Maple2DB`;
USE `Maple2DB`;

--
-- Table structure for table `accounts`
--

DROP TABLE IF EXISTS `accounts`;
CREATE TABLE `accounts` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `Username` varchar(25) NOT NULL,
  `PasswordHash` varchar(255) NOT NULL,
  `CreationTime` bigint NOT NULL,
  `LastLoginTime` bigint NOT NULL,
  `CharacterSlots` int NOT NULL,
  `Meret` bigint DEFAULT NULL,
  `GameMeret` bigint DEFAULT NULL,
  `EventMeret` bigint DEFAULT NULL,
  `MesoToken` bigint DEFAULT NULL,
  `BankInventoryId` bigint DEFAULT NULL,
  `VIPExpiration` bigint NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_Accounts_Username` (`Username`),
  KEY `accounts_bankinventoryid_FK` (`BankInventoryId`),
  CONSTRAINT `accounts_bankinventoryid_FK` FOREIGN KEY (`BankInventoryId`) REFERENCES `bankinventories` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Table structure for table `bankinventories`
--

DROP TABLE IF EXISTS `bankinventories`;
CREATE TABLE `bankinventories` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `ExtraSize` int NOT NULL,
  `Mesos` bigint DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Table structure for table `buddies`
--

DROP TABLE IF EXISTS `buddies`;
CREATE TABLE `buddies` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `SharedId` bigint NOT NULL,
  `PlayerCharacterId` bigint DEFAULT NULL,
  `FriendCharacterId` bigint DEFAULT NULL,
  `Message` varchar(25) DEFAULT '',
  `IsFriendRequest` tinyint(1) NOT NULL,
  `IsPending` tinyint(1) NOT NULL,
  `Blocked` tinyint(1) NOT NULL,
  `BlockReason` varchar(25) DEFAULT '',
  `Timestamp` bigint NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Buddies_FriendCharacterId` (`FriendCharacterId`),
  KEY `IX_Buddies_PlayerCharacterId` (`PlayerCharacterId`),
  CONSTRAINT `FK_Buddies_Characters_FriendCharacterId` FOREIGN KEY (`FriendCharacterId`) REFERENCES `characters` (`CharacterId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_Buddies_Characters_PlayerCharacterId` FOREIGN KEY (`PlayerCharacterId`) REFERENCES `characters` (`CharacterId`) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Table structure for table `characters`
--

DROP TABLE IF EXISTS `characters`;
CREATE TABLE `characters` (
  `CharacterId` bigint NOT NULL AUTO_INCREMENT,
  `AccountId` bigint NOT NULL,
  `CreationTime` bigint NOT NULL,
  `Name` varchar(25) NOT NULL,
  `Gender` tinyint unsigned NOT NULL,
  `Awakened` tinyint(1) NOT NULL,
  `Job` int NOT NULL,
  `LevelsId` bigint DEFAULT NULL,
  `MapId` int NOT NULL,
  `TitleId` int NOT NULL,
  `InsigniaId` smallint NOT NULL,
  `Titles` text,
  `PrestigeRewardsClaimed` text,
  `MaxSkillTabs` int NOT NULL,
  `ActiveSkillTabId` bigint NOT NULL,
  `GameOptionsId` bigint DEFAULT NULL,
  `WalletId` bigint DEFAULT NULL,
  `ChatSticker` text,
  `ClubId` bigint NOT NULL,
  `Coord` text NOT NULL,
  `Emotes` text,
  `FavoriteStickers` text,
  `GroupChatId` text,
  `GuildApplications` text,
  `GuildId` bigint DEFAULT NULL,
  `GuildMemberId` bigint DEFAULT NULL,
  `InventoryId` bigint DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL DEFAULT '0',
  `Mapleopoly` text,
  `Motto` varchar(25) DEFAULT '',
  `ProfileUrl` varchar(50) DEFAULT '',
  `ReturnCoord` text NOT NULL,
  `ReturnMapId` int NOT NULL,
  `SkinColor` text NOT NULL,
  `StatPointDistribution` text,
  `Stats` text,
  `TrophyCount` text,
  `UnlockedMaps` text,
  `UnlockedTaxis` text,
  `VisitingHomeId` bigint NOT NULL,
  PRIMARY KEY (`CharacterId`),
  UNIQUE KEY `IX_Characters_Name` (`Name`),
  KEY `IX_Characters_AccountId` (`AccountId`),
  KEY `IX_Characters_GameOptionsId` (`GameOptionsId`),
  KEY `IX_Characters_GuildId` (`GuildId`),
  KEY `IX_Characters_GuildMemberId` (`GuildMemberId`),
  KEY `IX_Characters_InventoryId` (`InventoryId`),
  KEY `IX_Characters_LevelsId` (`LevelsId`),
  KEY `IX_Characters_WalletId` (`WalletId`),
  CONSTRAINT `FK_Characters_Accounts_AccountId` FOREIGN KEY (`AccountId`) REFERENCES `accounts` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_Characters_GameOptions_GameOptionsId` FOREIGN KEY (`GameOptionsId`) REFERENCES `gameoptions` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_Characters_GuildMembers_GuildMemberId` FOREIGN KEY (`GuildMemberId`) REFERENCES `guildmembers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_Characters_Guilds_GuildId` FOREIGN KEY (`GuildId`) REFERENCES `guilds` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_Characters_Inventories_InventoryId` FOREIGN KEY (`InventoryId`) REFERENCES `inventories` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_Characters_Levels_LevelsId` FOREIGN KEY (`LevelsId`) REFERENCES `levels` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_Characters_Wallets_WalletId` FOREIGN KEY (`WalletId`) REFERENCES `wallets` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Table structure for table `cubes`
--

DROP TABLE IF EXISTS `cubes`;
CREATE TABLE `cubes` (
  `Uid` bigint NOT NULL AUTO_INCREMENT,
  `CoordX` float NOT NULL,
  `CoordY` float NOT NULL,
  `CoordZ` float NOT NULL,
  `HomeId` bigint DEFAULT NULL,
  `ItemUid` bigint DEFAULT NULL,
  `LayoutUid` bigint DEFAULT NULL,
  `PlotNumber` int NOT NULL,
  `Rotation` float NOT NULL,
  PRIMARY KEY (`Uid`),
  KEY `IX_Cubes_HomeId` (`HomeId`),
  KEY `IX_Cubes_ItemUid` (`ItemUid`),
  KEY `IX_Cubes_LayoutUid` (`LayoutUid`),
  CONSTRAINT `FK_Cubes_HomeLayouts_LayoutUid` FOREIGN KEY (`LayoutUid`) REFERENCES `homelayouts` (`Uid`) ON DELETE RESTRICT,
  CONSTRAINT `FK_Cubes_Homes_HomeId` FOREIGN KEY (`HomeId`) REFERENCES `homes` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_Cubes_Items_ItemUid` FOREIGN KEY (`ItemUid`) REFERENCES `items` (`Uid`) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Table structure for table `gameoptions`
--

DROP TABLE IF EXISTS `gameoptions`;
CREATE TABLE `gameoptions` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `KeyBinds` text,
  `ActiveHotbarId` smallint NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Table structure for table `guildapplications`
--

DROP TABLE IF EXISTS `guildapplications`;
CREATE TABLE `guildapplications` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `GuildId` bigint NOT NULL,
  `CharacterId` bigint NOT NULL,
  `CreationTimestamp` bigint NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_GuildApplications_GuildId` (`GuildId`),
  CONSTRAINT `FK_GuildApplications_Guilds_GuildId` FOREIGN KEY (`GuildId`) REFERENCES `guilds` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Table structure for table `guildmembers`
--

DROP TABLE IF EXISTS `guildmembers`;
CREATE TABLE `guildmembers` (
  `Id` bigint NOT NULL,
  `Rank` tinyint unsigned NOT NULL,
  `DailyContribution` int NOT NULL,
  `ContributionTotal` int NOT NULL,
  `DailyDonationCount` tinyint unsigned NOT NULL,
  `AttendanceTimestamp` bigint NOT NULL,
  `JoinTimestamp` bigint NOT NULL,
  `GuildId` bigint DEFAULT NULL,
  `Motto` varchar(50) DEFAULT '',
  PRIMARY KEY (`Id`),
  KEY `IX_GuildMembers_GuildId` (`GuildId`),
  CONSTRAINT `FK_GuildMembers_Characters_Id` FOREIGN KEY (`Id`) REFERENCES `characters` (`CharacterId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_GuildMembers_Guilds_GuildId` FOREIGN KEY (`GuildId`) REFERENCES `guilds` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Table structure for table `guilds`
--

DROP TABLE IF EXISTS `guilds`;
CREATE TABLE `guilds` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `Name` varchar(12) NOT NULL,
  `CreationTimestamp` bigint NOT NULL,
  `LeaderAccountId` bigint NOT NULL,
  `LeaderCharacterId` bigint NOT NULL,
  `LeaderName` varchar(50) NOT NULL,
  `Capacity` tinyint unsigned NOT NULL,
  `Funds` int NOT NULL,
  `Exp` int NOT NULL,
  `Searchable` tinyint(1) NOT NULL,
  `Buffs` text,
  `Emblem` varchar(50) DEFAULT '',
  `FocusAttributes` int NOT NULL,
  `HouseRank` int NOT NULL,
  `HouseTheme` int NOT NULL,
  `Notice` varchar(300) DEFAULT '',
  `Ranks` text,
  `Services` text,
  PRIMARY KEY (`Id`),
  KEY `IX_Guilds_LeaderCharacterId` (`LeaderCharacterId`),
  KEY `IX_Guilds_LeaderAccountId` (`LeaderAccountId`),
  CONSTRAINT `FK_Guilds_Characters_LeaderCharacterId` FOREIGN KEY (`LeaderCharacterId`) REFERENCES `characters` (`CharacterId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_Guilds_Characters_LeaderAccountId` FOREIGN KEY (`LeaderAccountId`) REFERENCES `accounts` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Table structure for table `homelayouts`
--

DROP TABLE IF EXISTS `homelayouts`;
CREATE TABLE `homelayouts` (
  `Uid` bigint NOT NULL AUTO_INCREMENT,
  `Height` tinyint unsigned NOT NULL,
  `HomeId` bigint DEFAULT NULL,
  `Id` int NOT NULL,
  `Name` text,
  `Size` tinyint unsigned NOT NULL,
  `Timestamp` bigint NOT NULL,
  PRIMARY KEY (`Uid`),
  KEY `IX_HomeLayouts_HomeId` (`HomeId`),
  CONSTRAINT `FK_HomeLayouts_Homes_HomeId` FOREIGN KEY (`HomeId`) REFERENCES `homes` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Table structure for table `homes`
--

DROP TABLE IF EXISTS `homes`;
CREATE TABLE `homes` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `AccountId` bigint NOT NULL,
  `MapId` int NOT NULL,
  `PlotMapId` int NOT NULL,
  `PlotNumber` int NOT NULL,
  `ApartmentNumber` int NOT NULL,
  `Expiration` bigint NOT NULL,
  `Name` varchar(16) DEFAULT NULL,
  `Description` varchar(100) DEFAULT NULL,
  `Size` tinyint unsigned NOT NULL,
  `Height` tinyint unsigned NOT NULL,
  `ArchitectScoreCurrent` int NOT NULL,
  `ArchitectScoreTotal` int NOT NULL,
  `DecorationExp` bigint NOT NULL,
  `DecorationLevel` bigint NOT NULL,
  `DecorationRewardTimestamp` bigint NOT NULL,
  `InteriorRewardsClaimed` text,
  `Lighting` tinyint unsigned NOT NULL,
  `Background` tinyint unsigned NOT NULL,
  `Camera` tinyint unsigned NOT NULL,
  `Password` text,
  `Permissions` text,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_Homes_AccountId` (`AccountId`),
  CONSTRAINT `FK_Homes_Accounts_AccountId` FOREIGN KEY (`AccountId`) REFERENCES `accounts` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Table structure for table `hotbars`
--

DROP TABLE IF EXISTS `hotbars`;
CREATE TABLE `hotbars` (
  `HotbarId` bigint NOT NULL AUTO_INCREMENT,
  `Slots` text,
  `GameOptionsId` bigint DEFAULT NULL,
  PRIMARY KEY (`HotbarId`),
  KEY `IX_Hotbars_GameOptionsId` (`GameOptionsId`),
  CONSTRAINT `FK_Hotbars_GameOptions_GameOptionsId` FOREIGN KEY (`GameOptionsId`) REFERENCES `gameoptions` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Table structure for table `inventories`
--

DROP TABLE IF EXISTS `inventories`;
CREATE TABLE `inventories` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `ExtraSize` text,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Table structure for table `items`
--

DROP TABLE IF EXISTS `items`;
CREATE TABLE `items` (
  `Uid` bigint NOT NULL AUTO_INCREMENT,
  `Level` int NOT NULL,
  `ItemSlot` tinyint unsigned NOT NULL,
  `Rarity` int NOT NULL,
  `PlayCount` int NOT NULL,
  `Amount` int NOT NULL,
  `BankInventoryId` bigint DEFAULT NULL,
  `CanRepackage` tinyint(1) NOT NULL,
  `Charges` int NOT NULL,
  `Color` text NOT NULL,
  `CreationTime` bigint NOT NULL,
  `EnchantExp` int NOT NULL,
  `Enchants` int NOT NULL,
  `ExpiryTime` bigint NOT NULL,
  `FaceDecorationData` text,
  `GachaDismantleId` int NOT NULL,
  `GuildId` bigint DEFAULT NULL,
  `HairData` text NOT NULL,
  `HatData` text NOT NULL,
  `HomeId` bigint DEFAULT NULL,
  `Id` int NOT NULL,
  `InventoryId` bigint DEFAULT NULL,
  `IsEquipped` tinyint(1) NOT NULL,
  `IsLocked` tinyint(1) NOT NULL,
  `MailUid` int DEFAULT NULL,
  `OwnerCharacterId` bigint DEFAULT NULL,
  `OwnerCharacterName` varchar(25) DEFAULT '',
  `PairedCharacterId` bigint NOT NULL,
  `PairedCharacterName` varchar(25) DEFAULT '',
  `PetSkinBadgeId` int NOT NULL,
  `RemainingGlamorForges` smallint NOT NULL,
  `Score` text,
  `Slot` smallint NOT NULL,
  `Stats` text,
  `TimesAttributesChanged` int NOT NULL,
  `TransferFlag` tinyint unsigned NOT NULL,
  `TransparencyBadgeBools` text,
  `UnlockTime` bigint NOT NULL,
  PRIMARY KEY (`Uid`),
  KEY `IX_Items_BankInventoryId` (`BankInventoryId`),
  KEY `IX_Items_GuildId` (`GuildId`),
  KEY `IX_Items_HomeId` (`HomeId`),
  KEY `IX_Items_InventoryId` (`InventoryId`),
  KEY `IX_Items_MailUid` (`MailUid`),
  KEY `IX_Items_OwnerCharacterId` (`OwnerCharacterId`),
  CONSTRAINT `FK_Items_BankInventories_BankInventoryId` FOREIGN KEY (`BankInventoryId`) REFERENCES `bankinventories` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_Items_Characters_OwnerCharacterId` FOREIGN KEY (`OwnerCharacterId`) REFERENCES `characters` (`CharacterId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_Items_Guilds_GuildId` FOREIGN KEY (`GuildId`) REFERENCES `guilds` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_Items_Homes_HomeId` FOREIGN KEY (`HomeId`) REFERENCES `homes` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_Items_Inventories_InventoryId` FOREIGN KEY (`InventoryId`) REFERENCES `inventories` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_Items_Mails_MailUid` FOREIGN KEY (`MailUid`) REFERENCES `mails` (`Uid`) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Table structure for table `levels`
--

DROP TABLE IF EXISTS `levels`;
CREATE TABLE `levels` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `Level` smallint NOT NULL,
  `Exp` bigint NOT NULL,
  `RestExp` bigint NOT NULL,
  `PrestigeLevel` int NOT NULL,
  `PrestigeExp` bigint NOT NULL,
  `MasteryExp` text,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Table structure for table `mails`
--

DROP TABLE IF EXISTS `mails`;
CREATE TABLE `mails` (
  `Uid` int NOT NULL AUTO_INCREMENT,
  `Type` tinyint unsigned NOT NULL,
  `PlayerId` bigint NOT NULL,
  `SenderName` varchar(25) DEFAULT '',
  `Title` varchar(25) DEFAULT '',
  `Body` text,
  `ReadTimestamp` bigint NOT NULL,
  `SentTimestamp` bigint NOT NULL,
  PRIMARY KEY (`Uid`),
  KEY `IX_Mails_PlayerId` (`PlayerId`),
  CONSTRAINT `FK_Mails_Characters_PlayerId` FOREIGN KEY (`PlayerId`) REFERENCES `characters` (`CharacterId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Table structure for table `quests`
--

DROP TABLE IF EXISTS `quests`;
CREATE TABLE `quests` (
  `Uid` bigint NOT NULL AUTO_INCREMENT,
  `Id` int NOT NULL,
  `Started` tinyint(1) NOT NULL,
  `Completed` tinyint(1) NOT NULL,
  `StartTimestamp` bigint NOT NULL,
  `CompleteTimestamp` bigint NOT NULL,
  `Condition` text,
  `CharacterId` bigint DEFAULT NULL,
  PRIMARY KEY (`Uid`),
  KEY `IX_Quests_CharacterId` (`CharacterId`),
  CONSTRAINT `FK_Quests_Characters_CharacterId` FOREIGN KEY (`CharacterId`) REFERENCES `characters` (`CharacterId`) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Table structure for table `skilltabs`
--

DROP TABLE IF EXISTS `skilltabs`;
CREATE TABLE `skilltabs` (
  `Uid` bigint NOT NULL AUTO_INCREMENT,
  `TabId` bigint NOT NULL,
  `Name` varchar(25) DEFAULT '',
  `SkillLevels` text,
  `CharacterId` bigint DEFAULT NULL,
  PRIMARY KEY (`Uid`),
  KEY `IX_SkillTabs_CharacterId` (`CharacterId`),
  CONSTRAINT `FK_SkillTabs_Characters_CharacterId` FOREIGN KEY (`CharacterId`) REFERENCES `characters` (`CharacterId`) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Table structure for table `trophies`
--

DROP TABLE IF EXISTS `trophies`;
CREATE TABLE `trophies` (
  `Uid` bigint NOT NULL AUTO_INCREMENT,
  `Id` int NOT NULL,
  `NextGrade` int NOT NULL,
  `MaxGrade` int NOT NULL,
  `Counter` bigint NOT NULL,
  `Condition` bigint NOT NULL,
  `IsDone` tinyint(1) NOT NULL,
  `Type` varchar(25) DEFAULT NULL,
  `Timestamps` text,
  `CharacterId` bigint DEFAULT NULL,
  PRIMARY KEY (`Uid`),
  KEY `IX_Trophies_CharacterId` (`CharacterId`),
  CONSTRAINT `FK_Trophies_Characters_CharacterId` FOREIGN KEY (`CharacterId`) REFERENCES `characters` (`CharacterId`) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Table structure for table `wallets`
--

DROP TABLE IF EXISTS `wallets`;
CREATE TABLE `wallets` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `Meso` bigint DEFAULT NULL,
  `ValorToken` bigint DEFAULT NULL,
  `Treva` bigint DEFAULT NULL,
  `Rue` bigint DEFAULT NULL,
  `HaviFruit` bigint DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;