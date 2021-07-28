using System.Collections.Generic;
using System.Linq;
using Maple2.Trigger;
using Maple2.Trigger.Enum;
using MapleServer2.Packets;
using MapleServer2.Types;

namespace MapleServer2.Triggers
{
    public partial class TriggerContext
    {
        public void CreateWidget(WidgetType type)
        {
            Widget widget = new Widget(type);
            List<IFieldObject<Player>> players = Field.State.Players.Values.ToList();
            foreach (IFieldObject<Player> player in players)
            {
                player.Value.Widgets.Add(widget);
            }
        }

        public void WidgetAction(WidgetType type, string name, string args, int widgetArgNum)
        {
            List<IFieldObject<Player>> players = Field.State.Players.Values.ToList();
            foreach (IFieldObject<Player> player in players)
            {
                Widget widget = player.Value.Widgets.FirstOrDefault(x => x.Type == type);
                if (widget == null)
                {
                    continue;
                }
                widget.State = name;
            }
        }

        public void GuideEvent(int eventId)
        {
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

        public void PlaySystemSoundInBox(int[] boxId, string sound)
        {
            if (boxId.Length == 0)
            {
                Field.BroadcastPacket(SystemSoundPacket.Play(sound));
                return;
            }
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

        public void SetEventUI(byte arg1, string script, int arg3, string boxId)
        {

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
        }

        public void ShowEventResult(EventResultType type, string text, int duration, int userTagId, int triggerBoxId, bool isOutSide)
        {
        }

        public void SetCinematicUI(byte type, string script, bool arg3)
        {
            Field.BroadcastPacket(NoticePacket.Notice($"Setting cinematic UI type: {type}", Enums.NoticeType.Chat));
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
        }

        public void SetSkip(TriggerState state)
        {
        }
    }
}
