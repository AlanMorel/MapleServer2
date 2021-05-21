﻿using System.Collections.Generic;
using GameDataParser.Files;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers
{
    public class MeretMarketParser : Exporter<List<MeretMarketMetadata>>
    {
        public MeretMarketParser() : base(null, "meret-market") { }

        protected override List<MeretMarketMetadata> Parse()
        {
            List<MeretMarketMetadata> market = new List<MeretMarketMetadata>();

            MeretMarketMetadata gatherbadge30day = new MeretMarketMetadata()
            {

                Category = MeretMarketCategory.Functional,
                MarketItemId = 140300010,
                ItemId = 70100005,
                Rarity = 1,
                Quantity = 1,
                TokenType = MeretMarketCurrencyType.Meret,
                SellBeginTime = 1262304000,
                SellEndTime = 4102444800,
                JobRequirement = MeretMarketJobRequirement.All,
                Price = 360,
                SalePrice = 360,
            };
            market.Add(gatherbadge30day);

            MeretMarketMetadata gatherbadge7day = new MeretMarketMetadata()
            {
                Category = MeretMarketCategory.Functional,
                MarketItemId = 140300020,
                ItemId = 70100004,
                Rarity = 1,
                Quantity = 1,
                TokenType = MeretMarketCurrencyType.Meret,
                SellBeginTime = 1262304000,
                SellEndTime = 4102444800,
                JobRequirement = MeretMarketJobRequirement.All,
                Price = 140,
                SalePrice = 140,
            };
            market.Add(gatherbadge7day);

            MeretMarketMetadata transparencybadge = new MeretMarketMetadata()
            {
                Category = MeretMarketCategory.Functional,
                MarketItemId = 140300025,
                ItemId = 70100001,
                Rarity = 1,
                Quantity = 1,
                TokenType = MeretMarketCurrencyType.Meret,
                SellBeginTime = 1262304000,
                SellEndTime = 4102444800,
                JobRequirement = MeretMarketJobRequirement.All,
                Price = 1450,
                SalePrice = 1450,
            };
            market.Add(transparencybadge);

            MeretMarketMetadata glamoranvil = new MeretMarketMetadata()
            {
                Category = MeretMarketCategory.Functional,
                MarketItemId = 140300030,
                ItemId = 30000896,
                Rarity = 1,
                Quantity = 1,
                TokenType = MeretMarketCurrencyType.Meret,
                SellBeginTime = 1262304000,
                SellEndTime = 4102444800,
                JobRequirement = MeretMarketJobRequirement.All,
                Price = 650,
                SalePrice = 650,
            };
            market.Add(glamoranvil);

            MeretMarketMetadata petskinscroll = new MeretMarketMetadata()
            {
                Category = MeretMarketCategory.Functional,
                MarketItemId = 140300040,
                ItemId = 20302314,
                Rarity = 1,
                Quantity = 1,
                TokenType = MeretMarketCurrencyType.Meret,
                SellBeginTime = 1262304000,
                SellEndTime = 4102444800,
                JobRequirement = MeretMarketJobRequirement.All,
                Price = 650,
                SalePrice = 650,
            };
            market.Add(petskinscroll);

            MeretMarketMetadata emergencyradio = new MeretMarketMetadata()
            {
                Category = MeretMarketCategory.Functional,
                MarketItemId = 140300050,
                ItemId = 20300011,
                Rarity = 1,
                Quantity = 1,
                TokenType = MeretMarketCurrencyType.Meret,
                SellBeginTime = 1262304000,
                SellEndTime = 4102444800,
                JobRequirement = MeretMarketJobRequirement.All,
                Price = 30,
                SalePrice = 30,
                AdditionalQuantities = new List<MeretMarketMetadata>()
                {
                    new MeretMarketMetadata()
                    {
                         MarketItemId = 140300051,
                         ParentMarketItemId = 140300050,
                         ItemId = 20300011,
                         Rarity = 1,
                         Quantity = 5,
                         TokenType = MeretMarketCurrencyType.Meret,
                         SellBeginTime = 1262304000,
                         SellEndTime = 4102444800,
                         JobRequirement = MeretMarketJobRequirement.All,
                         Price = 150,
                         SalePrice = 150,
                    },
                    new MeretMarketMetadata()
                    {
                         MarketItemId = 140300052,
                         ParentMarketItemId = 140300050,
                         ItemId = 20300011,
                         Rarity = 1,
                         Quantity = 10,
                         TokenType = MeretMarketCurrencyType.Meret,
                         SellBeginTime = 1262304000,
                         SellEndTime = 4102444800,
                         JobRequirement = MeretMarketJobRequirement.All,
                         Price = 300,
                         SalePrice = 300,
                    },
                    new MeretMarketMetadata()
                    {
                         MarketItemId = 140300053,
                         ParentMarketItemId = 140300050,
                         ItemId = 20300011,
                         Rarity = 1,
                         Quantity = 30,
                         TokenType = MeretMarketCurrencyType.Meret,
                         SellBeginTime = 1262304000,
                         SellEndTime = 4102444800,
                         JobRequirement = MeretMarketJobRequirement.All,
                         Price = 900,
                         SalePrice = 900,
                    }
                }
            };
            market.Add(emergencyradio);

            MeretMarketMetadata elixir = new MeretMarketMetadata()
            {
                Category = MeretMarketCategory.Functional,
                MarketItemId = 140300060,
                ItemId = 20000004,
                Rarity = 1,
                Quantity = 1,
                TokenType = MeretMarketCurrencyType.Meret,
                SellBeginTime = 1262304000,
                SellEndTime = 4102444800,
                JobRequirement = MeretMarketJobRequirement.All,
                Price = 7,
                SalePrice = 7,
                AdditionalQuantities = new List<MeretMarketMetadata>()
                {
                    new MeretMarketMetadata()
                    {
                         MarketItemId = 140300061,
                         ParentMarketItemId = 140300060,
                         ItemId = 20000004,
                         Rarity = 1,
                         Quantity = 10,
                         TokenType = MeretMarketCurrencyType.Meret,
                         SellBeginTime = 1262304000,
                         SellEndTime = 4102444800,
                         JobRequirement = MeretMarketJobRequirement.All,
                         Price = 70,
                         SalePrice = 65,
                    },
                    new MeretMarketMetadata()
                    {
                         MarketItemId = 140300062,
                         ParentMarketItemId = 140300060,
                         ItemId = 20000004,
                         Rarity = 1,
                         Quantity = 50,
                         TokenType = MeretMarketCurrencyType.Meret,
                         SellBeginTime = 1262304000,
                         SellEndTime = 4102444800,
                         JobRequirement = MeretMarketJobRequirement.All,
                         Price = 350,
                         SalePrice = 300,
                    },
                    new MeretMarketMetadata()
                    {
                         MarketItemId = 140300063,
                         ParentMarketItemId = 140300060,
                         ItemId = 20000004,
                         Rarity = 1,
                         Quantity = 100,
                         TokenType = MeretMarketCurrencyType.Meret,
                         SellBeginTime = 1262304000,
                         SellEndTime = 4102444800,
                         JobRequirement = MeretMarketJobRequirement.All,
                         Price = 700,
                         SalePrice = 550,
                    }
                }
            };
            market.Add(elixir);

            MeretMarketMetadata rotos = new MeretMarketMetadata()
            {
                Category = MeretMarketCategory.Functional,
                MarketItemId = 140300070,
                ItemId = 20300226,
                Rarity = 1,
                Quantity = 1,
                TokenType = MeretMarketCurrencyType.Meret,
                SellBeginTime = 1262304000,
                SellEndTime = 4102444800,
                JobRequirement = MeretMarketJobRequirement.All,
                Price = 5,
                SalePrice = 5,
                AdditionalQuantities = new List<MeretMarketMetadata>()
                {
                    new MeretMarketMetadata()
                    {
                         MarketItemId = 140300071,
                         ParentMarketItemId = 140300070,
                         ItemId = 20300226,
                         Rarity = 1,
                         Quantity = 10,
                         TokenType = MeretMarketCurrencyType.Meret,
                         SellBeginTime = 1262304000,
                         SellEndTime = 4102444800,
                         JobRequirement = MeretMarketJobRequirement.All,
                         Price = 50,
                         SalePrice = 45,
                    },
                    new MeretMarketMetadata()
                    {
                         MarketItemId = 140300072,
                         ParentMarketItemId = 140300070,
                         ItemId = 20300226,
                         Rarity = 1,
                         Quantity = 50,
                         TokenType = MeretMarketCurrencyType.Meret,
                         SellBeginTime = 1262304000,
                         SellEndTime = 4102444800,
                         JobRequirement = MeretMarketJobRequirement.All,
                         Price = 250,
                         SalePrice = 200,
                    },
                    new MeretMarketMetadata()
                    {
                         MarketItemId = 140300073,
                         ParentMarketItemId = 140300070,
                         ItemId = 20300226,
                         Rarity = 1,
                         Quantity = 100,
                         TokenType = MeretMarketCurrencyType.Meret,
                         SellBeginTime = 1262304000,
                         SellEndTime = 4102444800,
                         JobRequirement = MeretMarketJobRequirement.All,
                         Price = 500,
                         SalePrice = 380,
                    }
                }
            };
            market.Add(rotos);

            MeretMarketMetadata skilltabvoucher = new MeretMarketMetadata()
            {
                Category = MeretMarketCategory.Functional,
                MarketItemId = 140300080,
                ItemId = 20301114,
                Rarity = 1,
                Quantity = 1,
                TokenType = MeretMarketCurrencyType.Meret,
                SellBeginTime = 1262304000,
                SellEndTime = 4102444800,
                JobRequirement = MeretMarketJobRequirement.All,
                Price = 990,
                SalePrice = 990,
            };
            market.Add(skilltabvoucher);

            MeretMarketMetadata characterslotvoucher = new MeretMarketMetadata()
            {
                Category = MeretMarketCategory.Functional,
                MarketItemId = 140300090,
                ItemId = 20300225,
                Rarity = 1,
                Quantity = 1,
                TokenType = MeretMarketCurrencyType.Meret,
                SellBeginTime = 1262304000,
                SellEndTime = 4102444800,
                JobRequirement = MeretMarketJobRequirement.All,
                Price = 790,
                SalePrice = 790,
            };
            market.Add(characterslotvoucher);

            MeretMarketMetadata genderchangevoucher = new MeretMarketMetadata()
            {
                Category = MeretMarketCategory.Functional,
                MarketItemId = 140300100,
                ItemId = 20300224,
                Rarity = 1,
                Quantity = 1,
                TokenType = MeretMarketCurrencyType.Meret,
                SellBeginTime = 1262304000,
                SellEndTime = 4102444800,
                JobRequirement = MeretMarketJobRequirement.All,
                Price = 1390,
                SalePrice = 1390,
            };
            market.Add(genderchangevoucher);

            MeretMarketMetadata namechangevoucher = new MeretMarketMetadata()
            {
                Category = MeretMarketCategory.Functional,
                MarketItemId = 140300110,
                ItemId = 20300222,
                Rarity = 1,
                Quantity = 1,
                TokenType = MeretMarketCurrencyType.Meret,
                SellBeginTime = 1262304000,
                SellEndTime = 4102444800,
                JobRequirement = MeretMarketJobRequirement.All,
                Price = 990,
                SalePrice = 990,
            };
            market.Add(namechangevoucher);

            MeretMarketMetadata guildsummon = new MeretMarketMetadata()
            {
                Category = MeretMarketCategory.Functional,
                MarketItemId = 140300120,
                ItemId = 20300054,
                Rarity = 1,
                Quantity = 1,
                TokenType = MeretMarketCurrencyType.Meret,
                SellBeginTime = 1262304000,
                SellEndTime = 4102444800,
                JobRequirement = MeretMarketJobRequirement.All,
                Price = 150,
                SalePrice = 150,
            };
            market.Add(guildsummon);

            MeretMarketMetadata heliumballoon = new MeretMarketMetadata()
            {
                Category = MeretMarketCategory.Functional,
                MarketItemId = 140300130,
                ItemId = 20300046,
                Rarity = 1,
                Quantity = 1,
                TokenType = MeretMarketCurrencyType.Meret,
                SellBeginTime = 1262304000,
                SellEndTime = 4102444800,
                JobRequirement = MeretMarketJobRequirement.All,
                Price = 10,
                SalePrice = 10,
            };
            market.Add(heliumballoon);

            MeretMarketMetadata goldusmtm = new MeretMarketMetadata()
            {
                Category = MeretMarketCategory.Functional,
                MarketItemId = 140300140,
                ItemId = 20300089,
                Rarity = 1,
                Quantity = 1,
                Duration = 1,
                TokenType = MeretMarketCurrencyType.Meret,
                SellBeginTime = 1262304000,
                SellEndTime = 4102444800,
                JobRequirement = MeretMarketJobRequirement.All,
                Price = 60,
                SalePrice = 60,
                AdditionalQuantities = new List<MeretMarketMetadata>()
                {
                    new MeretMarketMetadata()
                    {
                         MarketItemId = 140300141,
                         ParentMarketItemId = 140300140,
                         ItemId = 20300089,
                         Rarity = 1,
                         Quantity = 1,
                         Duration = 7,
                         TokenType = MeretMarketCurrencyType.Meret,
                         SellBeginTime = 1262304000,
                         SellEndTime = 4102444800,
                         JobRequirement = MeretMarketJobRequirement.All,
                         Price = 420,
                         SalePrice = 270,
                    }
                }
            };
            market.Add(goldusmtm);

            MeretMarketMetadata goldbassdrum = new MeretMarketMetadata()
            {
                Category = MeretMarketCategory.Lifestyle,
                MarketItemId = 140600000,
                ItemId = 34000088,
                Rarity = 3,
                Quantity = 1,
                TokenType = MeretMarketCurrencyType.Meret,
                Price = 500,
                SalePrice = 500,
                SellBeginTime = 1262304000,
                SellEndTime = 4102444800,
                JobRequirement = MeretMarketJobRequirement.All,
            };
            market.Add(goldbassdrum);

            MeretMarketMetadata goldsnaredrum = new MeretMarketMetadata()
            {
                Category = MeretMarketCategory.Lifestyle,
                MarketItemId = 140600010,
                ItemId = 34000089,
                Rarity = 3,
                Quantity = 1,
                TokenType = MeretMarketCurrencyType.Meret,
                Price = 500,
                SalePrice = 500,
                SellBeginTime = 1262304000,
                SellEndTime = 4102444800,
                JobRequirement = MeretMarketJobRequirement.All,
            };
            market.Add(goldsnaredrum);

            MeretMarketMetadata goldcymbals = new MeretMarketMetadata()
            {
                Category = MeretMarketCategory.Lifestyle,
                MarketItemId = 140600020,
                ItemId = 34000090,
                Rarity = 3,
                Quantity = 1,
                TokenType = MeretMarketCurrencyType.Meret,
                Price = 500,
                SalePrice = 500,
                SellBeginTime = 1262304000,
                SellEndTime = 4102444800,
                JobRequirement = MeretMarketJobRequirement.All,
            };
            market.Add(goldcymbals);

            MeretMarketMetadata arlanocelesta = new MeretMarketMetadata()
            {
                Category = MeretMarketCategory.Lifestyle,
                MarketItemId = 140600030,
                ItemId = 34000071,
                Rarity = 3,
                Quantity = 1,
                TokenType = MeretMarketCurrencyType.Meret,
                Price = 500,
                SalePrice = 500,
                SellBeginTime = 1262304000,
                SellEndTime = 4102444800,
                JobRequirement = MeretMarketJobRequirement.All,
            };
            market.Add(arlanocelesta);

            MeretMarketMetadata arlanorecorder = new MeretMarketMetadata()
            {
                Category = MeretMarketCategory.Lifestyle,
                MarketItemId = 140600040,
                ItemId = 34000070,
                Rarity = 3,
                Quantity = 1,
                TokenType = MeretMarketCurrencyType.Meret,
                Price = 500,
                SalePrice = 500,
                SellBeginTime = 1262304000,
                SellEndTime = 4102444800,
                JobRequirement = MeretMarketJobRequirement.All,
            };
            market.Add(arlanorecorder);

            MeretMarketMetadata arlanoxylophone = new MeretMarketMetadata()
            {
                Category = MeretMarketCategory.Lifestyle,
                MarketItemId = 140600050,
                ItemId = 34000049,
                Rarity = 3,
                Quantity = 1,
                TokenType = MeretMarketCurrencyType.Meret,
                Price = 500,
                SalePrice = 500,
                SellBeginTime = 1262304000,
                SellEndTime = 4102444800,
                JobRequirement = MeretMarketJobRequirement.All,
            };
            market.Add(arlanoxylophone);

            MeretMarketMetadata arlanoharmonica = new MeretMarketMetadata()
            {
                Category = MeretMarketCategory.Lifestyle,
                MarketItemId = 140600060,
                ItemId = 34000048,
                Rarity = 3,
                Quantity = 1,
                TokenType = MeretMarketCurrencyType.Meret,
                Price = 500,
                SalePrice = 500,
                SellBeginTime = 1262304000,
                SellEndTime = 4102444800,
                JobRequirement = MeretMarketJobRequirement.All,
            };
            market.Add(arlanoharmonica);

            MeretMarketMetadata arlanoharpsichord = new MeretMarketMetadata()
            {
                Category = MeretMarketCategory.Lifestyle,
                MarketItemId = 140600070,
                ItemId = 34000041,
                Rarity = 3,
                Quantity = 1,
                TokenType = MeretMarketCurrencyType.Meret,
                Price = 500,
                SalePrice = 500,
                SellBeginTime = 1262304000,
                SellEndTime = 4102444800,
                JobRequirement = MeretMarketJobRequirement.All,
            };
            market.Add(arlanoharpsichord);

            MeretMarketMetadata arlanopizzicato = new MeretMarketMetadata()
            {
                Category = MeretMarketCategory.Lifestyle,
                MarketItemId = 140600080,
                ItemId = 34000040,
                Rarity = 3,
                Quantity = 1,
                TokenType = MeretMarketCurrencyType.Meret,
                Price = 500,
                SalePrice = 500,
                SellBeginTime = 1262304000,
                SellEndTime = 4102444800,
                JobRequirement = MeretMarketJobRequirement.All,
            };
            market.Add(arlanopizzicato);

            MeretMarketMetadata arlanooboe = new MeretMarketMetadata()
            {
                Category = MeretMarketCategory.Lifestyle,
                MarketItemId = 140600090,
                ItemId = 34000039,
                Rarity = 3,
                Quantity = 1,
                TokenType = MeretMarketCurrencyType.Meret,
                Price = 500,
                SalePrice = 500,
                SellBeginTime = 1262304000,
                SellEndTime = 4102444800,
                JobRequirement = MeretMarketJobRequirement.All,
            };
            market.Add(arlanooboe);

            MeretMarketMetadata arlanoelectricpiano = new MeretMarketMetadata()
            {
                Category = MeretMarketCategory.Lifestyle,
                MarketItemId = 140600100,
                ItemId = 34000035,
                Rarity = 3,
                Quantity = 1,
                TokenType = MeretMarketCurrencyType.Meret,
                Price = 500,
                SalePrice = 500,
                SellBeginTime = 1262304000,
                SellEndTime = 4102444800,
                JobRequirement = MeretMarketJobRequirement.All,
            };
            market.Add(arlanoelectricpiano);

            MeretMarketMetadata puriopickbass = new MeretMarketMetadata()
            {
                Category = MeretMarketCategory.Lifestyle,
                MarketItemId = 140600110,
                ItemId = 34000036,
                Rarity = 3,
                Quantity = 1,
                TokenType = MeretMarketCurrencyType.Meret,
                Price = 500,
                SalePrice = 500,
                SellBeginTime = 1262304000,
                SellEndTime = 4102444800,
                JobRequirement = MeretMarketJobRequirement.All,
            };
            market.Add(puriopickbass);

            MeretMarketMetadata puriosteeldrum = new MeretMarketMetadata()
            {
                Category = MeretMarketCategory.Lifestyle,
                MarketItemId = 140600120,
                ItemId = 34000037,
                Rarity = 3,
                Quantity = 1,
                TokenType = MeretMarketCurrencyType.Meret,
                Price = 500,
                SalePrice = 500,
                SellBeginTime = 1262304000,
                SellEndTime = 4102444800,
                JobRequirement = MeretMarketJobRequirement.All,
            };
            market.Add(puriosteeldrum);

            MeretMarketMetadata arlanoacousticbass = new MeretMarketMetadata()
            {
                Category = MeretMarketCategory.Lifestyle,
                MarketItemId = 140600130,
                ItemId = 34000028,
                Rarity = 3,
                Quantity = 1,
                TokenType = MeretMarketCurrencyType.Meret,
                Price = 500,
                SalePrice = 500,
                SellBeginTime = 1262304000,
                SellEndTime = 4102444800,
                JobRequirement = MeretMarketJobRequirement.All,
            };
            market.Add(arlanoacousticbass);

            MeretMarketMetadata arlanoocarina = new MeretMarketMetadata()
            {
                Category = MeretMarketCategory.Lifestyle,
                MarketItemId = 140600140,
                ItemId = 34000027,
                Rarity = 3,
                Quantity = 1,
                TokenType = MeretMarketCurrencyType.Meret,
                Price = 500,
                SalePrice = 500,
                SellBeginTime = 1262304000,
                SellEndTime = 4102444800,
                JobRequirement = MeretMarketJobRequirement.All,
            };
            market.Add(arlanoocarina);

            MeretMarketMetadata arlanovibraphone = new MeretMarketMetadata()
            {
                Category = MeretMarketCategory.Lifestyle,
                MarketItemId = 140600150,
                ItemId = 34000029,
                Rarity = 3,
                Quantity = 1,
                TokenType = MeretMarketCurrencyType.Meret,
                Price = 500,
                SalePrice = 500,
                SellBeginTime = 1262304000,
                SellEndTime = 4102444800,
                JobRequirement = MeretMarketJobRequirement.All,
            };
            market.Add(arlanovibraphone);

            MeretMarketMetadata arlanosaxophone = new MeretMarketMetadata()
            {
                Category = MeretMarketCategory.Lifestyle,
                MarketItemId = 140600160,
                ItemId = 34000024,
                Rarity = 3,
                Quantity = 1,
                TokenType = MeretMarketCurrencyType.Meret,
                Price = 500,
                SalePrice = 500,
                SellBeginTime = 1262304000,
                SellEndTime = 4102444800,
                JobRequirement = MeretMarketJobRequirement.All,
            };
            market.Add(arlanosaxophone);

            MeretMarketMetadata arlanotrombone = new MeretMarketMetadata()
            {
                Category = MeretMarketCategory.Lifestyle,
                MarketItemId = 140600170,
                ItemId = 34000025,
                Rarity = 3,
                Quantity = 1,
                TokenType = MeretMarketCurrencyType.Meret,
                Price = 500,
                SalePrice = 500,
                SellBeginTime = 1262304000,
                SellEndTime = 4102444800,
                JobRequirement = MeretMarketJobRequirement.All,
            };
            market.Add(arlanotrombone);

            MeretMarketMetadata arlanotrumpet = new MeretMarketMetadata()
            {
                Category = MeretMarketCategory.Lifestyle,
                MarketItemId = 140600180,
                ItemId = 34000026,
                Rarity = 3,
                Quantity = 1,
                TokenType = MeretMarketCurrencyType.Meret,
                Price = 500,
                SalePrice = 500,
                SellBeginTime = 1262304000,
                SellEndTime = 4102444800,
                JobRequirement = MeretMarketJobRequirement.All,
            };
            market.Add(arlanotrumpet);

            MeretMarketMetadata arlanoviolin = new MeretMarketMetadata()
            {
                Category = MeretMarketCategory.Lifestyle,
                MarketItemId = 140600190,
                ItemId = 34000021,
                Rarity = 3,
                Quantity = 1,
                TokenType = MeretMarketCurrencyType.Meret,
                Price = 500,
                SalePrice = 500,
                SellBeginTime = 1262304000,
                SellEndTime = 4102444800,
                JobRequirement = MeretMarketJobRequirement.All,
            };
            market.Add(arlanoviolin);

            MeretMarketMetadata arlanocello = new MeretMarketMetadata()
            {
                Category = MeretMarketCategory.Lifestyle,
                MarketItemId = 140600200,
                ItemId = 34000022,
                Rarity = 3,
                Quantity = 1,
                TokenType = MeretMarketCurrencyType.Meret,
                Price = 500,
                SalePrice = 500,
                SellBeginTime = 1262304000,
                SellEndTime = 4102444800,
                JobRequirement = MeretMarketJobRequirement.All,
            };
            market.Add(arlanocello);

            MeretMarketMetadata arlanopanflute = new MeretMarketMetadata()
            {
                Category = MeretMarketCategory.Lifestyle,
                MarketItemId = 140600210,
                ItemId = 34000023,
                Rarity = 3,
                Quantity = 1,
                TokenType = MeretMarketCurrencyType.Meret,
                Price = 500,
                SalePrice = 500,
                SellBeginTime = 1262304000,
                SellEndTime = 4102444800,
                JobRequirement = MeretMarketJobRequirement.All,
            };
            market.Add(arlanopanflute);

            MeretMarketMetadata purioelectricguitar = new MeretMarketMetadata()
            {
                Category = MeretMarketCategory.Lifestyle,
                MarketItemId = 140600220,
                ItemId = 34000013,
                Rarity = 3,
                Quantity = 1,
                TokenType = MeretMarketCurrencyType.Meret,
                Price = 500,
                SalePrice = 500,
                SellBeginTime = 1262304000,
                SellEndTime = 4102444800,
                JobRequirement = MeretMarketJobRequirement.All,
            };
            market.Add(purioelectricguitar);

            MeretMarketMetadata puriobassguitar = new MeretMarketMetadata()
            {
                Category = MeretMarketCategory.Lifestyle,
                MarketItemId = 140600230,
                ItemId = 34000014,
                Rarity = 3,
                Quantity = 1,
                TokenType = MeretMarketCurrencyType.Meret,
                Price = 500,
                SalePrice = 500,
                SellBeginTime = 1262304000,
                SellEndTime = 4102444800,
                JobRequirement = MeretMarketJobRequirement.All,
            };
            market.Add(puriobassguitar);

            MeretMarketMetadata puriotomtoms = new MeretMarketMetadata()
            {
                Category = MeretMarketCategory.Lifestyle,
                MarketItemId = 140600240,
                ItemId = 34000015,
                Rarity = 3,
                Quantity = 1,
                TokenType = MeretMarketCurrencyType.Meret,
                Price = 500,
                SalePrice = 500,
                SellBeginTime = 1262304000,
                SellEndTime = 4102444800,
                JobRequirement = MeretMarketJobRequirement.All,
            };
            market.Add(puriotomtoms);

            MeretMarketMetadata arlanotimpani = new MeretMarketMetadata()
            {
                Category = MeretMarketCategory.Lifestyle,
                MarketItemId = 140600250,
                ItemId = 34000012,
                Rarity = 3,
                Quantity = 1,
                TokenType = MeretMarketCurrencyType.Meret,
                Price = 500,
                SalePrice = 500,
                SellBeginTime = 1262304000,
                SellEndTime = 4102444800,
                JobRequirement = MeretMarketJobRequirement.All,
            };
            market.Add(arlanotimpani);

            MeretMarketMetadata arlanoharp = new MeretMarketMetadata()
            {
                Category = MeretMarketCategory.Lifestyle,
                MarketItemId = 140600260,
                ItemId = 34000011,
                Rarity = 3,
                Quantity = 1,
                TokenType = MeretMarketCurrencyType.Meret,
                Price = 500,
                SalePrice = 500,
                SellBeginTime = 1262304000,
                SellEndTime = 4102444800,
                JobRequirement = MeretMarketJobRequirement.All,
            };
            market.Add(arlanoharp);

            MeretMarketMetadata arlanoclarinet = new MeretMarketMetadata()
            {
                Category = MeretMarketCategory.Lifestyle,
                MarketItemId = 140600270,
                ItemId = 34000010,
                Rarity = 3,
                Quantity = 1,
                TokenType = MeretMarketCurrencyType.Meret,
                Price = 500,
                SalePrice = 500,
                SellBeginTime = 1262304000,
                SellEndTime = 4102444800,
                JobRequirement = MeretMarketJobRequirement.All,
            };
            market.Add(arlanoclarinet);

            MeretMarketMetadata arlanoguitar = new MeretMarketMetadata()
            {
                Category = MeretMarketCategory.Lifestyle,
                MarketItemId = 140600280,
                ItemId = 34000009,
                Rarity = 3,
                Quantity = 1,
                TokenType = MeretMarketCurrencyType.Meret,
                Price = 500,
                SalePrice = 500,
                SellBeginTime = 1262304000,
                SellEndTime = 4102444800,
                JobRequirement = MeretMarketJobRequirement.All,
            };
            market.Add(arlanoguitar);

            MeretMarketMetadata arlanopiano = new MeretMarketMetadata()
            {
                Category = MeretMarketCategory.Lifestyle,
                MarketItemId = 140600290,
                ItemId = 34000008,
                Rarity = 3,
                Quantity = 1,
                TokenType = MeretMarketCurrencyType.Meret,
                Price = 500,
                SalePrice = 500,
                SellBeginTime = 1262304000,
                SellEndTime = 4102444800,
                JobRequirement = MeretMarketJobRequirement.All,
            };
            market.Add(arlanopiano);

            MeretMarketMetadata slimecapsule = new MeretMarketMetadata()
            {
                Category = MeretMarketCategory.Promo,
                MarketItemId = 10000000,
                ItemId = 22002001,
                Rarity = 4,
                Quantity = 1,
                TokenType = MeretMarketCurrencyType.Meret,
                Price = 120,
                SalePrice = 100,
                SellBeginTime = 1262304000,
                SellEndTime = 4102444800,
                PromoBannerBeginTime = 1262304000,
                PromoBannerEndTime = 4102444800,
                ShowSaleTime = true,
                PromoFlag = MeretMarketPromoFlag.BlueGift,
                JobRequirement = MeretMarketJobRequirement.All,
                AdditionalQuantities = new List<MeretMarketMetadata>()
                {
                    new MeretMarketMetadata()
                    {
                         MarketItemId = 10000001,
                         ParentMarketItemId = 10000000,
                         ItemId = 22002001,
                         Rarity = 4,
                         Quantity = 10,
                         TokenType = MeretMarketCurrencyType.Meret,
                         SellBeginTime = 1262304000,
                         SellEndTime = 4102444800,
                         JobRequirement = MeretMarketJobRequirement.All,
                         Price = 1200,
                         SalePrice = 900,
                    },
                    new MeretMarketMetadata()
                    {
                         MarketItemId = 10000002,
                         ParentMarketItemId = 10000000,
                         ItemId = 22002001,
                         Rarity = 4,
                         Quantity = 50,
                         TokenType = MeretMarketCurrencyType.Meret,
                         SellBeginTime = 1262304000,
                         SellEndTime = 4102444800,
                         JobRequirement = MeretMarketJobRequirement.All,
                         Price = 6000,
                         SalePrice = 5000,
                    }
                }
            };
            market.Add(slimecapsule);

            MeretMarketMetadata duckycapsules7 = new MeretMarketMetadata()
            {
                Category = MeretMarketCategory.Promo,
                MarketItemId = 10000010,
                ItemId = 22001010,
                Rarity = 4,
                Quantity = 1,
                TokenType = MeretMarketCurrencyType.Meret,
                Price = 130,
                SalePrice = 100,
                SellBeginTime = 1262304000,
                SellEndTime = 4102444800,
                PromoBannerBeginTime = 1262304000,
                PromoBannerEndTime = 4102444800,
                ShowSaleTime = false,
                PromoFlag = MeretMarketPromoFlag.PinkGift,
                JobRequirement = MeretMarketJobRequirement.All,
                AdditionalQuantities = new List<MeretMarketMetadata>()
                {
                    new MeretMarketMetadata()
                    {
                        MarketItemId = 10000011,
                        ParentMarketItemId = 10000010,
                        ItemId = 22001010,
                        Rarity = 4,
                        Quantity = 10,
                        TokenType = MeretMarketCurrencyType.Meret,
                        SellBeginTime = 1262304000,
                        SellEndTime = 4102444800,
                        JobRequirement = MeretMarketJobRequirement.All,
                        Price = 1300,
                        SalePrice = 950,
                    },
                    new MeretMarketMetadata()
                    {
                         MarketItemId = 10000012,
                         ParentMarketItemId = 10000010,
                         ItemId = 22001010,
                         Rarity = 4,
                         Quantity = 50,
                         TokenType = MeretMarketCurrencyType.Meret,
                         SellBeginTime = 1262304000,
                         SellEndTime = 4102444800,
                         JobRequirement = MeretMarketJobRequirement.All,
                         Price = 6500,
                         SalePrice = 5200,
                    }
                }
            };
            market.Add(duckycapsules7);

            return market;
        }
    }
}
