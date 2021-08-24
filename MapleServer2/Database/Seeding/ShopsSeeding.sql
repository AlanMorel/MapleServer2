--
-- Table structure for table `shops`
--

DROP TABLE IF EXISTS `shops`;
CREATE TABLE `shops` (
  `Uid` int NOT NULL AUTO_INCREMENT,
  `Id` int NOT NULL,
  `Category` int NOT NULL,
  `Name` varchar(25) DEFAULT NULL,
  `ShopType` tinyint unsigned NOT NULL,
  `RestrictSales` tinyint(1) NOT NULL,
  `CanRestock` tinyint(1) NOT NULL,
  `NextRestock` bigint NOT NULL,
  `AllowBuyback` tinyint(1) NOT NULL,
  PRIMARY KEY (`Uid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

INSERT INTO `shops` (`Id`, `Category`, `Name`, `ShopType`, `RestrictSales`, `CanRestock`, `NextRestock`, `AllowBuyback`) VALUES

(103, 3, 'shop', 0, 0, 0, 1629086030, 1),
(105, 3, 'shop', 0, 0, 0, 1629086030, 1),
(116, 3, 'shop', 0, 0, 0, 1629086030, 1),
(121, 8, 'ride', 0, 0, 0, 1629086030, 1),
(123, 7, 'shopetc', 0, 0, 0, 1629086030, 1),
(130, 8, 'ride', 0, 0, 0, 1629086030, 1),
(135, 3, 'karmashop', 0, 0, 0, 1629086030, 1),
(138, 6, 'karmaequip', 0, 0, 0, 1629086030, 1),
(142, 12, 'karmaride', 0, 0, 0, 1629086030, 1),
(148, 5, 'equip', 0, 0, 0, 1629086030, 1),
(151, 16, 'lushop', 0, 0, 0, 1629086030, 1),
(152, 17, 'habishop', 0, 0, 0, 1629086030, 1),
(153, 19, 'bluestarshop', 0, 0, 0, 1629086030, 1),
(154, 19, 'redstarshop', 0, 0, 0, 1629086030, 1),
(160, 20, 'musicshop', 0, 0, 0, 1629086030, 1),
(161, 22, 'shopetc', 0, 0, 0, 1629086030, 1),
(168, 28, 'guildtokenetc', 0, 1, 0, 1629086030, 1),
(177, 30, 'chaosashop', 0, 0, 0, 1629086030, 1),
(196, 3, 'dungeonhelp', 0, 0, 1, 1629086030, 1),
(963, 920, 'shopetc', 0, 0, 0, 1629086030, 1),
(101001, 101001, 'shopetc', 4, 1, 0, 1629086030, 1),
(101002, 101002, 'shopetc', 4, 1, 0, 1629086030, 1),
(101003, 101003, 'shopetc', 4, 1, 0, 1629086030, 1),
(101004, 101004, 'shopetc', 4, 1, 0, 1629086030, 1),
(101005, 101005, 'shopetc', 4, 1, 0, 1629086030, 1),
(101006, 101006, 'shopetc', 4, 1, 0, 1629086030, 1),
(101007, 101007, 'shopetc', 4, 1, 0, 1629086030, 1),
(101008, 101008, 'shopetc', 4, 1, 0, 1629086030, 1),
(101009, 101009, 'shopetc', 4, 1, 0, 1629086030, 1),
(101010, 101010, 'shopetc', 4, 1, 0, 1629086030, 1),
(102001, 102001, 'shopetc', 4, 1, 0, 1629086030, 1),
(102002, 102001, 'shopetc', 4, 1, 0, 1629086030, 1),
(102003, 102001, 'shopetc', 4, 1, 0, 1629086030, 1),
(102004, 102001, 'shopetc', 4, 1, 0, 1629086030, 1),
(102005, 102001, 'shopetc', 4, 1, 0, 1629086030, 1),
(102006, 102001, 'shopetc', 4, 1, 0, 1629086030, 1),
(102007, 102001, 'shopetc', 4, 1, 0, 1629086030, 1),
(102008, 102001, 'shopetc', 4, 1, 0, 1629086030, 1),
(104201, 104201, 'shopetc', 4, 0, 0, 1629086030, 0),
(104201, 104201, 'shopetc', 4, 1, 0, 1629086030, 1),
(104202, 104202, 'shopetc', 4, 1, 0, 1629086030, 1);
