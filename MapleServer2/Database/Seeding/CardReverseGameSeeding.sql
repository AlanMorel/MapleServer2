DROP TABLE IF EXISTS `card_reverse_game`;

CREATE TABLE `card_reverse_game`
(
    `id`          int NOT NULL AUTO_INCREMENT,
    `item_id`     int NOT NULL,
    `item_rarity` tinyint unsigned NOT NULL,
    `item_amount` int NOT NULL,
    PRIMARY KEY (`id`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;

INSERT INTO `card_reverse_game` (`item_id`, `item_rarity`, `item_amount`)
VALUES (30000946, 1, 1),
       (20001690, 1, 3),
       (20001691, 1, 3),
       (20001689, 1, 10),
       (20001443, 1, 1),
       (20000939, 1, 20),
       (20300546, 1, 4),
       (20300429, 1, 4),
       (20300416, 1, 3),
       (20300311, 1, 1),
       (20300055, 1, 1),
       (20300078, 1, 10),
       (20300055, 1, 1),
       (20301360, 3, 5),
       (20000671, 1, 5),
       (20000589, 1, 1),
       (40100024, 4, 30),
       (20001138, 1, 5),
       (20300312, 1, 1),
       (40100042, 1, 200),
       (40100001, 1, 300),
       (20000258, 1, 1),
       (30000937, 1, 1),
       (20301498, 1, 5),
       (20300417, 1, 3);