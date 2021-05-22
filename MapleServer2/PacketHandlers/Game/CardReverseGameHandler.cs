﻿using System;
using System.Collections.Generic;
using System.Linq;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Database;
using MapleServer2.Database.Types;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class CardReverseGameHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.CARD_REVERSE_GAME;

        public CardReverseGameHandler(ILogger<CardReverseGameHandler> logger) : base(logger) { }

        private enum CardReverseGameMode : byte
        {
            Open = 0x0,
            Mix = 0x1,
            Select = 0x2,
        }

        public override void Handle(GameSession session, PacketReader packet)
        {
            CardReverseGameMode mode = (CardReverseGameMode) packet.ReadByte();

            switch (mode)
            {
                case CardReverseGameMode.Open:
                    HandleOpen(session);
                    break;
                case CardReverseGameMode.Mix:
                    HandleMix(session);
                    break;
                case CardReverseGameMode.Select:
                    HandleSelect(session);
                    break;
                default:
                    IPacketHandler<GameSession>.LogUnknownMode(mode);
                    break;
            }
        }

        private static void HandleOpen(GameSession session)
        {
            List<CardReverseGame> cards = DatabaseManager.GetCardReverseGame();
            session.Send(CardReverseGamePacket.Open(cards));
        }

        private static void HandleMix(GameSession session)
        {
            Item token = session.Player.Inventory.Items.FirstOrDefault(x => x.Value.Id == CardReverseGame.TOKEN_ITEM_ID).Value;
            if (token == null || token.Amount < CardReverseGame.TOKEN_COST)
            {
                session.Send(CardReverseGamePacket.Notice());
                return;
            }
            InventoryController.Consume(session, token.Uid, CardReverseGame.TOKEN_COST);

            session.Send(CardReverseGamePacket.Mix());
        }

        private static void HandleSelect(GameSession session)
        {
            // Unknown how this game works as to whether it's weighted or not
            // Currently being handled by each item having an equal chance

            List<CardReverseGame> cards = DatabaseManager.GetCardReverseGame();

            Random random = new Random();
            int index = random.Next(cards.Count);

            CardReverseGame card = cards[index];
            Item item = new Item(card.ItemId)
            {
                Amount = card.ItemAmount,
                Rarity = card.ItemRarity
            };

            session.Send(CardReverseGamePacket.Select(index));
            InventoryController.Add(session, item, true);
        }
    }
}
