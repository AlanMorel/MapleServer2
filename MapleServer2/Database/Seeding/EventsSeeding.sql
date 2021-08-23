--
-- Table structure for table `events`
--
DROP TABLE IF EXISTS `events`;

CREATE TABLE `events` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Active` tinyint(1) NOT NULL,
  `Type` smallint NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;

--
-- Table structure for table `event_fieldpopup`
--
DROP TABLE IF EXISTS `event_fieldpopup`;

CREATE TABLE `event_fieldpopup` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `MapId` int NOT NULL,
  `GameEventId` int NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `event_fieldpopup_FK` (`GameEventId`),
  CONSTRAINT `event_fieldpopup_FK` FOREIGN KEY (`GameEventId`) REFERENCES `events` (`Id`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;

--
-- Table structure for table `event_mapleopoly`
--
DROP TABLE IF EXISTS `event_mapleopoly`;

CREATE TABLE `event_mapleopoly` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `TripAmount` int NOT NULL,
  `ItemId` int NOT NULL,
  `ItemRarity` tinyint unsigned NOT NULL,
  `ItemAmount` int NOT NULL,
  `GameEventId` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Event_Mapleopoly_GameEventId` (`GameEventId`),
  CONSTRAINT `FK_Event_Mapleopoly_Events_GameEventId` FOREIGN KEY (`GameEventId`) REFERENCES `events` (`Id`) ON DELETE RESTRICT
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;

--
-- Table structure for table `event_stringboards`
--
DROP TABLE IF EXISTS `event_stringboards`;

CREATE TABLE `event_stringboards` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `StringId` int NOT NULL,
  `String` text,
  `GameEventId` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Event_StringBoards_GameEventId` (`GameEventId`),
  CONSTRAINT `FK_Event_StringBoards_Events_GameEventId` FOREIGN KEY (`GameEventId`) REFERENCES `events` (`Id`) ON DELETE RESTRICT
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;

--
-- Table structure for table `event_ugcmapcontractsale`
--
DROP TABLE IF EXISTS `event_ugcmapcontractsale`;

CREATE TABLE `event_ugcmapcontractsale` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `DiscountAmount` int NOT NULL,
  `GameEventId` int NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `event_ugcmapcontractsale_FK` (`GameEventId`),
  CONSTRAINT `event_ugcmapcontractsale_FK` FOREIGN KEY (`GameEventId`) REFERENCES `events` (`Id`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;

--
-- Table structure for table `event_ugcmapextensionsale`
--
DROP TABLE IF EXISTS `event_ugcmapextensionsale`;

CREATE TABLE `event_ugcmapextensionsale` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `DiscountAmount` int NOT NULL,
  `GameEventId` int NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `event_ugcmapextensionsale_FK` (`GameEventId`),
  CONSTRAINT `event_ugcmapextensionsale_FK` FOREIGN KEY (`GameEventId`) REFERENCES `events` (`Id`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;

INSERT INTO
  `events` (`Id`, `Active`, `Type`)
VALUES
  (1, 1, 0),
  (2, 1, 1),
  (3, 0, 9),
  (4, 0, 10),
  (5, 1, 12);

INSERT INTO
  `event_stringboards` (`StringId`, `String`, `GameEventId`)
VALUES
  (
    0,
    'Welcome! Please see our Github or Discord for updates and to report any bugs. discord.gg/fVZRdBN7Nd',
    1
  );

INSERT INTO
  `event_mapleopoly` (
    `TripAmount`,
    `ItemId`,
    `ItemRarity`,
    `ItemAmount`,
    `GameEventId`
  )
VALUES
  (0, 40100050, 1, 300, 2),
  (1, 20301835, 1, 5, 2),
  (10, 31001060, 4, 3, 2),
  (20, 70100004, 1, 1, 2),
  (35, 20302684, 4, 2, 2),
  (50, 20302524, 1, 1, 2);

INSERT INTO
  `event_ugcmapcontractsale` (`DiscountAmount`, `GameEventId`)
VALUES
  (9000, 3);

INSERT INTO
  `event_ugcmapextensionsale` (`DiscountAmount`, `GameEventId`)
VALUES
  (9000, 4);

INSERT INTO
  `event_fieldpopup` (`MapId`, `GameEventId`)
VALUES
  (63000049, 5);