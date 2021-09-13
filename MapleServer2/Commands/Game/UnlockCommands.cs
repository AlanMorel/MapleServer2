using MapleServer2.Commands.Core;
using MapleServer2.Database;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Types;

namespace MapleServer2.Commands.Game
{
    public class UnlockAll : InGameCommand
    {
        public UnlockAll()
        {
            Aliases = new() { "unlock" };
            Description = "Unlocks a bunch of emotes, stickers, and titles!";
            Usage = "/unlock";
        }

        public override void Execute(GameCommandTrigger trigger)
        {
            Player player = trigger.Session.Player;
            // Stickers
            for (int i = 1; i < 7; i++)
            {
                if (player.ChatSticker.Any(x => x.GroupId == i))
                {
                    continue;
                }
                trigger.Session.Send(ChatStickerPacket.AddSticker(21100000 + i, i, 9223372036854775807));
                player.ChatSticker.Add(new ChatSticker((byte) i, 9223372036854775807));
            }

            // Emotes
            for (int i = 1; i < 146; i++)
            {
                int emoteId = 90200000 + i;
                if (player.Emotes.Contains(emoteId))
                {
                    continue;
                }
                player.Emotes.Add(emoteId);

                trigger.Session.Send(EmotePacket.LearnEmote(emoteId));
            }

            // Titles
            for (int i = 1; i < 854; i++)
            {
                int titleId = 10000000 + i;
                if (player.Titles.Contains(titleId))
                {
                    continue;
                }
                player.Titles.Add(titleId);

                trigger.Session.Send(UserEnvPacket.AddTitle(titleId));
            }

            DatabaseManager.Characters.Update(player);
            trigger.Session.Send(NoticePacket.Notice("Done!", NoticeType.Chat));
        }
    }

    public class UnlockTrophyCommand : InGameCommand
    {
        public UnlockTrophyCommand()
        {
            Aliases = new() { "trophy" };
            Description = "Unlock an trophy!";
            Parameters = new()
            {
                new Parameter<int>("trophyId", "The trophy id to unlock;", 0),
                new Parameter<int>("amount", "The amount of trophy goals.", 1),
            };
            Usage = "/trophy [trophyId] [amount]";
        }

        public override void Execute(GameCommandTrigger trigger)
        {
            int trophyId = trigger.Get<int>("trophyId");
            int amount = trigger.Get<int>("amount");
            if (trophyId == 0)
            {
                trigger.Session.Send(NoticePacket.Notice("Type an trophy id!", NoticeType.Chat));
                return;
            }
            trigger.Session.Player.TrophyUpdate(trophyId, amount);

            trigger.Session.Player.TrophyData.TryGetValue(trophyId, out Trophy trophy);
            DatabaseManager.Trophies.Update(trophy);
            trigger.Session.Send(NoticePacket.Notice("Done!", NoticeType.Chat));
        }
    }
}
