--
-- Table structure for table `mapleopolytiles`
--
DROP TABLE IF EXISTS `mapleopoly_tiles`;

CREATE TABLE `mapleopoly_tiles`
(
    `tile_position`  int      NOT NULL AUTO_INCREMENT,
    `item_amount`    int      NOT NULL,
    `item_id`        int      NOT NULL,
    `item_rarity`    tinyint unsigned NOT NULL,
    `tile_parameter` int      NOT NULL,
    `type`           smallint NOT NULL,
    PRIMARY KEY (`tile_position`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;

INSERT INTO `mapleopoly_tiles` (`tile_position`,
                                `item_amount`,
                                `item_id`,
                                `item_rarity`,
                                `tile_parameter`,
                                `type`)
VALUES (1, 0, 0, 0, 0, 6),
       (2, 10, 20001130, 1, 0, 0),
       (3, 3, 20301360, 3, 0, 0),
       (4, 0, 0, 0, 5, 3),
       (5, 300, 40100023, 1, 0, 0),
       (6, 2, 20000279, 1, 0, 0),
       (7, 3, 20301360, 3, 0, 0),
       (8, 1, 20000569, 1, 0, 9),
       (9, 300, 40100036, 1, 0, 0),
       (10, 300, 40100037, 1, 0, 0),
       (11, 0, 0, 0, 0, 4),
       (12, 10, 20001130, 1, 0, 0),
       (13, 1, 20300855, 1, 0, 0),
       (14, 5, 20301493, 1, 0, 0),
       (15, 0, 0, 0, 0, 8),
       (16, 300, 40100037, 1, 0, 0),
       (17, 0, 0, 0, 5, 2),
       (18, 10, 20302777, 1, 0, 0),
       (19, 300, 40100036, 1, 0, 0),
       (20, 2, 20000280, 1, 0, 0),
       (21, 0, 0, 0, 3, 3),
       (22, 1, 20000569, 1, 0, 9),
       (23, 300, 40100037, 1, 0, 0),
       (24, 10, 20001130, 1, 0, 0),
       (25, 10, 20300078, 1, 0, 0),
       (26, 0, 0, 0, 3, 2),
       (27, 5, 20301360, 3, 0, 0),
       (28, 300, 40100037, 1, 0, 0);