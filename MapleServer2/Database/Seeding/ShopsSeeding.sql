--
-- Table structure for table `shops`
--

DROP TABLE IF EXISTS `shops`;
CREATE TABLE `shops`
(
    `id`             int    NOT NULL,
    `category`       int    NOT NULL,
    `name`           varchar(25) DEFAULT NULL,
    `shop_type`      tinyint unsigned NOT NULL,
    `restrict_sales` tinyint(1) NOT NULL,
    `can_restock`    tinyint(1) NOT NULL,
    `next_restock`   bigint NOT NULL,
    `allow_buyback`  tinyint(1) NOT NULL,
    PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

INSERT INTO `shops`
VALUES (103, 3, 'shop', 0, 0, 0, 0, 1),
       (101010, 101010, 'shopetc', 4, 1, 0, 0, 1),
       (102001, 102001, 'shopetc', 4, 1, 0, 0, 1),
       (102002, 102001, 'shopetc', 4, 1, 0, 0, 1),
       (102003, 102001, 'shopetc', 4, 1, 0, 0, 1),
       (102004, 102001, 'shopetc', 4, 1, 0, 0, 1),
       (102005, 102001, 'shopetc', 4, 1, 0, 0, 1),
       (102006, 102001, 'shopetc', 4, 1, 0, 0, 1),
       (102007, 102001, 'shopetc', 4, 1, 0, 0, 1),
       (102008, 102001, 'shopetc', 4, 1, 0, 0, 1),
       (104201, 104201, 'shopetc', 4, 1, 0, 0, 1),
       (104202, 104202, 'shopetc', 4, 1, 0, 0, 1),
       (161, 22, 'shopetc', 0, 0, 0, 0, 1),
       (963, 920, 'shopetc', 0, 0, 0, 0, 1),
       (105, 3, 'shop', 0, 0, 0, 0, 1),
       (123, 7, 'shopetc', 0, 0, 0, 0, 1),
       (148, 5, 'equip', 0, 0, 0, 0, 1),
       (121, 8, 'ride', 0, 0, 0, 0, 1),
       (101009, 101009, 'shopetc', 4, 1, 0, 0, 1),
       (116, 3, 'shop', 0, 0, 0, 0, 1),
       (101008, 101008, 'shopetc', 4, 1, 0, 0, 1),
       (101006, 101006, 'shopetc', 4, 1, 0, 0, 1),
       (152, 17, 'habishop', 0, 0, 0, 0, 1),
       (154, 19, 'redstarshop', 0, 0, 0, 0, 1),
       (153, 19, 'bluestarshop', 0, 0, 0, 0, 1),
       (151, 16, 'lushop', 0, 0, 0, 0, 1),
       (177, 30, 'chaosashop', 0, 0, 0, 0, 1),
       (135, 3, 'karmashop', 0, 0, 0, 0, 1),
       (138, 6, 'karmaequip', 0, 0, 0, 0, 1),
       (142, 12, 'karmaride', 0, 0, 0, 0, 1),
       (168, 28, 'guildtokenetc', 0, 1, 0, 0, 1),
       (196, 3, 'dungeonhelp', 0, 0, 1, 0, 1),
       (160, 20, 'musicshop', 0, 0, 0, 0, 1),
       (101001, 101001, 'shopetc', 4, 1, 0, 0, 1),
       (101002, 101002, 'shopetc', 4, 1, 0, 0, 1),
       (101003, 101003, 'shopetc', 4, 1, 0, 0, 1),
       (101004, 101004, 'shopetc', 4, 1, 0, 0, 1),
       (101005, 101005, 'shopetc', 4, 1, 0, 0, 1),
       (101007, 101007, 'shopetc', 4, 1, 0, 0, 1),
       (130, 8, 'ride', 0, 0, 0, 0, 1),
       (249, 66, 'wandershop_pietro', 3, 1, 0, 0, 1),
       (999999, 3, 'gmshop', 0, 0, 0, 0, 1);
