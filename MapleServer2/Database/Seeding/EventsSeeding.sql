--
-- Table structure for table `events`
--
DROP TABLE IF EXISTS `events`;

CREATE TABLE `events`
(
    `id`     int      NOT NULL AUTO_INCREMENT,
    `active` tinyint(1) NOT NULL,
    `type`   smallint NOT NULL,
    PRIMARY KEY (`id`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;

--
-- Table structure for table `event_fieldpopup`
--
DROP TABLE IF EXISTS `event_field_popup`;

CREATE TABLE `event_field_popup`
(
    `id`            int NOT NULL AUTO_INCREMENT,
    `map_id`        int NOT NULL,
    `game_event_id` int NOT NULL,
    PRIMARY KEY (`id`),
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
    `id`            int NOT NULL AUTO_INCREMENT,
    `message_id`    int NOT NULL,
    `message`       text,
    `game_event_id` int DEFAULT NULL,
    PRIMARY KEY (`id`),
    KEY             `ix_event_stringboards_gameeventid` (`game_event_id`),
    CONSTRAINT `fk_event_stringboards_events_gameeventid` FOREIGN KEY (`game_event_id`) REFERENCES `events` (`id`) ON DELETE RESTRICT
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;

--
-- Table structure for table `event_ugcmapcontractsale`
--
DROP TABLE IF EXISTS `event_ugc_map_contract_sale`;

CREATE TABLE `event_ugc_map_contract_sale`
(
    `id`              int NOT NULL AUTO_INCREMENT,
    `discount_amount` int NOT NULL,
    `game_event_id`   int NOT NULL,
    PRIMARY KEY (`id`),
    KEY               `event_ugcmapcontractsale_fk` (`game_event_id`),
    CONSTRAINT `event_ugcmapcontractsale_fk` FOREIGN KEY (`game_event_id`) REFERENCES `events` (`id`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;

--
-- Table structure for table `event_ugcmapextensionsale`
--
DROP TABLE IF EXISTS `event_ugc_map_extension_sale`;

CREATE TABLE `event_ugc_map_extension_sale`
(
    `id`              int NOT NULL AUTO_INCREMENT,
    `discount_amount` int NOT NULL,
    `game_event_id`   int NOT NULL,
    PRIMARY KEY (`id`),
    KEY               `event_ugcmapextensionsale_fk` (`game_event_id`),
    CONSTRAINT `event_ugcmapextensionsale_fk` FOREIGN KEY (`game_event_id`) REFERENCES `events` (`id`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;

INSERT INTO `events` (`id`, `active`, `type`)
VALUES (1, 1, 0),
       (2, 1, 1),
       (3, 0, 9),
       (4, 0, 10),
       (5, 1, 12);

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