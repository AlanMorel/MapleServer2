using Maple2.Trigger;
using Maple2.Trigger.Enum;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.Triggers
{
    public partial class TriggerContext
    {
        public void CreateWidget(WidgetType type)
        {
            Widget widget = new Widget(type);
            Field.AddWidget(widget);
        }

        public void WidgetAction(WidgetType type, string name, string args, int widgetArgNum)
        {
            Widget widget = Field.GetWidget(type);
            if (widget == null)
            {
                return;
            }

            widget.State = name;
        }

        public void GuideEvent(int eventId)
        {
            Field.BroadcastPacket(TriggerPacket.Guide(eventId));
        }

        public void HideGuideSummary(int entityId, int textId)
        {
            Field.BroadcastPacket(TriggerPacket.Banner(03, entityId, textId));
        }

        public void Notice(bool arg1, string arg2, bool arg3)
        {
        }

        public void PlaySystemSoundByUserTag(int userTagId, string soundKey)
        {
        }

        public void PlaySystemSoundInBox(int[] boxIds, string sound)
        {
            if (boxIds != null)
            {
                foreach (int boxId in boxIds)
                {
                    MapTriggerBox box = MapEntityStorage.GetTriggerBox(Field.MapId, boxId);

                    foreach (IFieldObject<Player> player in Field.State.Players.Values)
                    {
                        if (FieldManager.IsPlayerInBox(box, player))
                        {
                            player.Value.Session.Send(SystemSoundPacket.Play(sound));
                        }
                    }
                }
                return;
            }
            Field.BroadcastPacket(SystemSoundPacket.Play(sound));
        }

        public void ScoreBoardCreate(string type, int maxScore)
        {
        }

        public void ScoreBoardRemove()
        {
        }

        public void ScoreBoardSetScore(bool score)
        {
        }

        public void SetEventUI(byte typeId, string script, int duration, string box)
        {
            EventBannerType type = EventBannerType.None;
            switch (typeId)
            {
                case 1:
                    type = EventBannerType.None;
                    break;
                case 3:
                    type = EventBannerType.Winner;
                    break;
                case 6:
                    type = EventBannerType.Bonus;
                    break;
            }

            if (box == "0")
            {
                Field.BroadcastPacket(MassiveEventPacket.TextBanner(type, script, duration));
                return;
            }

            if (box.Contains('!'))
            {
                box = box[1..];
                int boxId = int.Parse(box);

                List<IFieldObject<Player>> players = Field.State.Players.Values.ToList();
                MapTriggerBox triggerBox = MapEntityStorage.GetTriggerBox(Field.MapId, boxId);

                foreach (IFieldObject<Player> player in players)
                {
                    if (FieldManager.IsPlayerInBox(triggerBox, player))
                    {
                        players.Remove(player);
                    }
                }
                foreach (IFieldObject<Player> player in players)
                {
                    player.Value.Session.Send(MassiveEventPacket.TextBanner(type, script, duration));
                }

                return;
            }

            int triggerBoxId = int.Parse(box);
            List<IFieldObject<Player>> fieldPlayers = new List<IFieldObject<Player>>();
            MapTriggerBox mapTriggerBox = MapEntityStorage.GetTriggerBox(Field.MapId, triggerBoxId);

            foreach (IFieldObject<Player> player in Field.State.Players.Values)
            {
                if (FieldManager.IsPlayerInBox(mapTriggerBox, player))
                {
                    fieldPlayers.Add(player);
                }
            }
            foreach (IFieldObject<Player> player in fieldPlayers)
            {
                player.Value.Session.Send(MassiveEventPacket.TextBanner(type, script, duration));
            }
        }

        public void SetVisibleUI(string uiName, bool visible)
        {
        }

        public void ShowCountUI(string text, byte stage, byte count, byte soundType)
        {
            Field.BroadcastPacket(MassiveEventPacket.Round(text, stage, count, soundType));
        }

        public void ShowRoundUI(byte round, int duration)
        {
        }

        public void ShowGuideSummary(int entityId, int textId, int duration)
        {
            Field.BroadcastPacket(TriggerPacket.Banner(02, entityId, textId, duration));
        }

        public void SideNpcTalk(int npcId, string illust, int duration, string script, string voice, SideNpcTalkType type, string usm)
        {
        }

        public void ShowCaption(CaptionType type, string title, string script, Align align, float offsetRateX, float offsetRateY, int duration, float scale)
        {
            string captionAlign = align.ToString().Replace(" ", "").Replace(",", "");
            captionAlign = captionAlign.First().ToString().ToLower() + captionAlign[1..];
            Field.BroadcastPacket(CinematicPacket.Caption(type, title, script, captionAlign, offsetRateX, offsetRateY, duration, scale));
        }

        public void ShowEventResult(EventResultType type, string text, int duration, int userTagId, int triggerBoxId, bool isOutSide)
        {
        }

        public void SetCinematicUI(byte type, string script, bool arg3)
        {
            switch (type)
            {
                case 0:
                    Field.BroadcastPacket(CinematicPacket.HideUi(false));
                    break;
                case 1:
                    Field.BroadcastPacket(CinematicPacket.HideUi(true));
                    break;
                case 2:
                case 4:
                    Field.BroadcastPacket(CinematicPacket.View(type));
                    break;
            }
        }

        public void SetCinematicIntro(string text)
        {
        }

        public void CloseCinematic()
        {
        }

        public void RemoveCinematicTalk()
        {
        }

        public void PlaySceneMovie(string fileName, int movieId, string skipType)
        {
            Field.BroadcastPacket(TriggerPacket.StartCutscene(fileName, movieId));
        }

        public void SetSceneSkip(TriggerState state, string arg2)
        {
            // TODO: Properly handle the trigger state
            SkipSceneState = state;
            Field.BroadcastPacket(CinematicPacket.SetSceneSkip(arg2));
        }

        public void SetSkip(TriggerState state)
        {
        }
    }
}
