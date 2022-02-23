﻿--
-- Table structure for table `events`
--
DROP TABLE IF EXISTS `events`;

CREATE TABLE `events`
(
    `id`     int      NOT NULL AUTO_INCREMENT,
    `begin_timestamp` bigint NOT NULL,
    `end_timestamp` bigint NOT NULL,
    PRIMARY KEY (`id`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;

--
-- Table structure for table `event_fieldpopup`
--
DROP TABLE IF EXISTS `event_field_popup`;

CREATE TABLE `event_field_popup`
(
    `map_id`        int NOT NULL,
    `game_event_id` int NOT NULL,
    PRIMARY KEY (`game_event_id`),
    KEY             `event_fieldpopup_fk` (`game_event_id`),
    CONSTRAINT `event_fieldpopup_fk` FOREIGN KEY (`game_event_id`) REFERENCES `events` (`id`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;

--
-- Table structure for table `event_mapleopoly`
--
DROP TABLE IF EXISTS `event_mapleopoly`;

CREATE TABLE `event_mapleopoly`
(
    `id`            int NOT NULL AUTO_INCREMENT,
    `trip_amount`   int NOT NULL,
    `item_id`       int NOT NULL,
    `item_rarity`   tinyint unsigned NOT NULL,
    `item_amount`   int NOT NULL,
    `game_event_id` int DEFAULT NULL,
    PRIMARY KEY (`id`),
    KEY             `ix_event_mapleopoly_gameeventid` (`game_event_id`),
    CONSTRAINT `fk_event_mapleopoly_events_gameeventid` FOREIGN KEY (`game_event_id`) REFERENCES `events` (`id`) ON DELETE RESTRICT
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;

--
-- Table structure for table `event_stringboards`
--
DROP TABLE IF EXISTS `event_string_boards`;

CREATE TABLE `event_string_boards`
(
    `message_id`    int NOT NULL,
    `message`       text,
    `game_event_id` int NOT NULL,
    PRIMARY KEY (`game_event_id`),
    KEY             `ix_event_stringboards_gameeventid` (`game_event_id`),
    CONSTRAINT `fk_event_stringboards_events_gameeventid` FOREIGN KEY (`game_event_id`) REFERENCES `events` (`id`) ON DELETE RESTRICT
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;

--
-- Table structure for table `event_ugcmapcontractsale`
--
DROP TABLE IF EXISTS `event_ugc_map_contract_sale`;

CREATE TABLE `event_ugc_map_contract_sale`
(
    `discount_amount` int NOT NULL,
    `game_event_id`   int NOT NULL,
    PRIMARY KEY (`game_event_id`),
    KEY               `event_ugcmapcontractsale_fk` (`game_event_id`),
    CONSTRAINT `event_ugcmapcontractsale_fk` FOREIGN KEY (`game_event_id`) REFERENCES `events` (`id`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;

--
-- Table structure for table `event_ugcmapextensionsale`
--
DROP TABLE IF EXISTS `event_ugc_map_extension_sale`;

CREATE TABLE `event_ugc_map_extension_sale`
(
    `discount_amount` int NOT NULL,
    `game_event_id`   int NOT NULL,
    PRIMARY KEY (`game_event_id`),
    KEY               `event_ugcmapextensionsale_fk` (`game_event_id`),
    CONSTRAINT `event_ugcmapextensionsale_fk` FOREIGN KEY (`game_event_id`) REFERENCES `events` (`id`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;

--
-- Table structure for table `event_attend_gift`
--
DROP TABLE IF EXISTS `event_attend_gift`;

CREATE TABLE `event_attend_gift`
(
    `game_event_id`   int NOT NULL,
    `name`            text,
    `url`             text,
    `disable_claim_button` tinyint(1) NOT NULL,
    `time_required`   int NOT NULL,
    `skip_day_currency_type` int,
    `skip_days_allowed`      int,
    `skip_day_cost`          bigint,
    `days`                   text,
    PRIMARY KEY (`game_event_id`),
    KEY               `event_attend_gift_fk` (`game_event_id`),
    CONSTRAINT `event_attend_gift_fk` FOREIGN KEY (`game_event_id`) REFERENCES `events` (`id`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;

INSERT INTO `events` (`id`, `begin_timestamp`, `end_timestamp`)
VALUES (1, 1590577200 ,2531386800),
       (2, 1590577200 ,2531386800),
       (3, 1590577200 ,2531386800),
       (4, 1590577200 ,2531386800),
       (5, 1590577200 ,2531386800),
       (6, 1590577200 ,2531386800);

INSERT INTO `event_string_boards` (`message_id`, `message`, `game_event_id`)
VALUES (0,
        'Welcome! Please see our Github or Discord for updates and to report any bugs. discord.gg/fVZRdBN7Nd',
        1);

INSERT INTO `event_mapleopoly` (`trip_amount`,
                                `item_id`,
                                `item_rarity`,
                                `item_amount`,
                                `game_event_id`)
VALUES (0, 40100050, 1, 300, 2),
       (1, 20301835, 1, 5, 2),
       (10, 31001060, 4, 3, 2),
       (20, 70100004, 1, 1, 2),
       (35, 20302684, 4, 2, 2),
       (50, 20302524, 1, 1, 2);

INSERT INTO `event_ugc_map_contract_sale` (`discount_amount`, `game_event_id`)
VALUES (9000, 3);

INSERT INTO `event_ugc_map_extension_sale` (`discount_amount`, `game_event_id`)
VALUES (9000, 4);

INSERT INTO `event_field_popup` (`map_id`, `game_event_id`)
VALUES (63000049, 5);

INSERT INTO `event_attend_gift` (`game_event_id`, `name`, `url`, `disable_claim_button`, `time_required`, `skip_day_currency_type`, `skip_days_allowed`, `skip_day_cost`, `days`)
VALUES (6, 
        'Emulator Attendance',
        'https://google.com',
        1, 
        1800, 
        2, 
        5,
        500,
        '[
{
	"Day": 1,
	"ItemId": 20600010,
	"ItemRarity": 4,
	"ItemAmount": 1
},
{
	"Day": 2,
	"ItemId": 30001445,
	"ItemRarity": 1,
	"ItemAmount": 20
},
{
	"Day": 3,
	"ItemId": 20800015,
	"ItemRarity": 1,
	"ItemAmount": 1
},
{
	"Day": 4,
	"ItemId": 22001001,
	"ItemRarity": 4,
	"ItemAmount": 1
},
{
	"Day": 5,
	"ItemId": 20500010,
	"ItemRarity": 1,
	"ItemAmount": 1
},
{
	"Day": 6,
	"ItemId": 20303152,
	"ItemRarity": 1,
	"ItemAmount": 1
},
{
	"Day": 7,
	"ItemId": 20302945,
	"ItemRarity": 1,
	"ItemAmount": 1
},
{
	"Day": 8,
	"ItemId": 30001445,
	"ItemRarity": 1,
	"ItemAmount": 100
},
{
	"Day": 9,
	"ItemId": 32000037,
	"ItemRarity": 4,
	"ItemAmount": 1
},
{
	"Day": 10,
	"ItemId": 34000098,
	"ItemRarity": 4,
	"ItemAmount": 1
},
{
	"Day": 11,
	"ItemId": 40400063,
	"ItemRarity": 4,
	"ItemAmount": 1
},
{
	"Day": 12,
	"ItemId": 59400040,
	"ItemRarity": 1,
	"ItemAmount": 1
},
{
	"Day": 13,
	"ItemId": 20000702,
	"ItemRarity": 5,
	"ItemAmount": 1
},
{
	"Day": 14,
	"ItemId": 20200104,
	"ItemRarity": 4,
	"ItemAmount": 1
}
]');