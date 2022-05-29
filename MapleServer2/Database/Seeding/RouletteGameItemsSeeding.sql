--
-- Table structure for table `roulette_game_items`
--

DROP TABLE IF EXISTS `roulette_game_items`;
CREATE TABLE `roulette_game_items`
(
    `uid`                              int      NOT NULL AUTO_INCREMENT,
    `roulette_id`                      int      NOT NULL,
    `item_id`                          int      NOT NULL,
    `item_amount`                      int      NOT NULL,
    `item_rarity`                      int      NOT NULL,    
    PRIMARY KEY (`uid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

INSERT INTO `roulette_game_items` (`roulette_id`, `item_id`, `item_amount`, `item_rarity`)
VALUES (13, 20000527, 1, 1),
       (13, 20000528, 1, 1),
       (13, 20000529, 1, 1),
       (13, 12100073, 1, 3),
       (13, 12088889, 1, 3),
       (13, 11200069, 1, 3),
       (13, 11900089, 1, 3),
       (13, 11800087, 1, 3),
       (13, 40100039, 1000, 1),
       (13, 11050011, 1, 1),
       (13, 11050020, 1, 1),
       (13, 20300041, 1, 1),
       (20, 20000479, 2, 1),
       (20, 40100039, 1000, 1),
       (20, 20000004, 5, 1),
       (20, 20000117, 1, 1),
       (20, 20300040, 1, 1),
       (20, 20000478, 2, 1),
       (20, 20000480, 2, 1),
       (20, 20000481, 2, 1),
       (20, 20000482, 2, 1),
       (20, 20300041, 1, 1),
       (20, 20300603, 1, 1),
       (20, 20300478, 1, 1),
       (22, 70400014, 1, 3),
       (22, 50600177, 1, 3),
       (22, 20000310, 1, 1),
       (22, 59200295, 1, 1),
       (22, 59200296, 1, 1),
       (22, 20300041, 1, 1),
       (22, 20301148, 8, 1),
       (22, 20300603, 1, 1),
       (22, 20300040, 1, 1),
       (22, 40100039, 100, 1),
       (22, 20000644, 1, 2),
       (22, 20300053, 1, 1)