--
-- Table structure for table `events`
--
DROP TABLE IF EXISTS `events`;

CREATE TABLE `events` (
  `id` int NOT NULL AUTO_INCREMENT,
  `active` tinyint(1) NOT NULL,
  `type` smallint NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;

--
-- Table structure for table `event_fieldpopup`
--
DROP TABLE IF EXISTS `event_fieldpopup`;

CREATE TABLE `event_fieldpopup` (
  `id` int NOT NULL AUTO_INCREMENT,
  `mapid` int NOT NULL,
  `gameeventid` int NOT NULL,
  PRIMARY KEY (`id`),
  KEY `event_fieldpopup_fk` (`gameeventid`),
  CONSTRAINT `event_fieldpopup_fk` FOREIGN KEY (`gameeventid`) REFERENCES `events` (`id`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;

--
-- Table structure for table `event_mapleopoly`
--
DROP TABLE IF EXISTS `event_mapleopoly`;

CREATE TABLE `event_mapleopoly` (
  `id` int NOT NULL AUTO_INCREMENT,
  `tripamount` int NOT NULL,
  `itemid` int NOT NULL,
  `itemrarity` tinyint unsigned NOT NULL,
  `itemamount` int NOT NULL,
  `gameeventid` int DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `ix_event_mapleopoly_gameeventid` (`gameeventid`),
  CONSTRAINT `fk_event_mapleopoly_events_gameeventid` FOREIGN KEY (`gameeventid`) REFERENCES `events` (`id`) ON DELETE RESTRICT
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;

--
-- Table structure for table `event_stringboards`
--
DROP TABLE IF EXISTS `event_stringboards`;

CREATE TABLE `event_stringboards` (
  `id` int NOT NULL AUTO_INCREMENT,
  `stringid` int NOT NULL,
  `string` text,
  `gameeventid` int DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `ix_event_stringboards_gameeventid` (`gameeventid`),
  CONSTRAINT `fk_event_stringboards_events_gameeventid` FOREIGN KEY (`gameeventid`) REFERENCES `events` (`id`) ON DELETE RESTRICT
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;

--
-- Table structure for table `event_ugcmapcontractsale`
--
DROP TABLE IF EXISTS `event_ugcmapcontractsale`;

CREATE TABLE `event_ugcmapcontractsale` (
  `id` int NOT NULL AUTO_INCREMENT,
  `discountamount` int NOT NULL,
  `gameeventid` int NOT NULL,
  PRIMARY KEY (`id`),
  KEY `event_ugcmapcontractsale_fk` (`gameeventid`),
  CONSTRAINT `event_ugcmapcontractsale_fk` FOREIGN KEY (`gameeventid`) REFERENCES `events` (`id`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;

--
-- Table structure for table `event_ugcmapextensionsale`
--
DROP TABLE IF EXISTS `event_ugcmapextensionsale`;

CREATE TABLE `event_ugcmapextensionsale` (
  `id` int NOT NULL AUTO_INCREMENT,
  `discountamount` int NOT NULL,
  `gameeventid` int NOT NULL,
  PRIMARY KEY (`id`),
  KEY `event_ugcmapextensionsale_fk` (`gameeventid`),
  CONSTRAINT `event_ugcmapextensionsale_fk` FOREIGN KEY (`gameeventid`) REFERENCES `events` (`id`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;

INSERT INTO
  `events` (`id`, `active`, `type`)
VALUES
  (1, 1, 0),
  (2, 1, 1),
  (3, 0, 9),
  (4, 0, 10),
  (5, 1, 12);

INSERT INTO
  `event_stringboards` (`stringid`, `string`, `gameeventid`)
VALUES
  (
    0,
    'Welcome! Please see our Github or Discord for updates and to report any bugs. discord.gg/fVZRdBN7Nd',
    1
  );

INSERT INTO
  `event_mapleopoly` (
    `tripamount`,
    `itemid`,
    `itemrarity`,
    `itemamount`,
    `gameeventid`
  )
VALUES
  (0, 40100050, 1, 300, 2),
  (1, 20301835, 1, 5, 2),
  (10, 31001060, 4, 3, 2),
  (20, 70100004, 1, 1, 2),
  (35, 20302684, 4, 2, 2),
  (50, 20302524, 1, 1, 2);

INSERT INTO
  `event_ugcmapcontractsale` (`discountamount`, `gameeventid`)
VALUES
  (9000, 3);

INSERT INTO
  `event_ugcmapextensionsale` (`discountamount`, `gameeventid`)
VALUES
  (9000, 4);

INSERT INTO
  `event_fieldpopup` (`mapid`, `gameeventid`)
VALUES
  (63000049, 5);