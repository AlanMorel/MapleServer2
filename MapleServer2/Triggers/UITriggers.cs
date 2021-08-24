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
            Console.WriteLine($"Doing Widget action on Widget Type: {type}. Action Name: {name}");
            Widget widget = Field.GetWidget(type);
            if (widget == null)
            {
                return;
            }

            switch (type)
            {
                case WidgetType.SceneMovie:
                    if (name == "Clear")
                    {
                        // TODO
                    }
                    break;
                case WidgetType.OxQuiz:
                    switch (name)
                    {
                        case "DevMode":
                            // TODO: Unknown
                            break;
                        case "PickQuiz":
                            // TODO: Use args to find a tier of a question
                            OXQuizMetadata question = OXQuizMetadataStorage.GetQuestion();
                            widget.OXQuizQuestion = new OXQuizQuestion(question.Category, question.QuestionText, question.AnswerText, question.Answer);
                            if (question.Answer)
                            {
                                Field.BroadcastPacket(NoticePacket.Notice("Answer is true", NoticeType.ChatAndFastText));

                            }
                            else
                            {
                                Field.BroadcastPacket(NoticePacket.Notice("Answer is false", NoticeType.ChatAndFastText));

                            }
                            break;
                        case "ShowQuiz":
                            Field.BroadcastPacket(QuizEventPacket.Question(widget.OXQuizQuestion.Category, widget.OXQuizQuestion.QuestionText, int.Parse(args)));
                            break;
                        case "PreJudge":
                            if (widget.OXQuizQuestion.Answer)
                            {
                                widget.State = "Correct";
                            }
                            else
                            {
                                widget.State = "Incorrect";
                            }
                            break;
                        case "ShowAnswer":
                            Field.BroadcastPacket(QuizEventPacket.Answer(widget.OXQuizQuestion.Answer, widget.OXQuizQuestion.AnswerText, int.Parse(args)));
                            break;
                        case "Judge":
                            break;
                    }
                    break;
                default:
                    Console.WriteLine($"Non implemented Widget Action. WidgetType: {type}");
                    break;
            }
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
            if (typeId != 0)
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
                    case 4:
                        type = EventBannerType.Lose;
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

                MapTriggerBox triggerBox;
                int boxId = 0;
                if (box.Contains('!'))
                {
                    box = box[1..];
                    boxId = int.Parse(box);
                    triggerBox = MapEntityStorage.GetTriggerBox(Field.MapId, boxId);
                    foreach (IFieldObject<Player> player in Field.State.Players.Values)
                    {
                        if (!FieldManager.IsPlayerInBox(triggerBox, player))
                        {
                            player.Value.Session.Send(MassiveEventPacket.TextBanner(type, script, duration));
                        }
                    }
                    return;
                }

                boxId = int.Parse(box);
                triggerBox = MapEntityStorage.GetTriggerBox(Field.MapId, boxId);
                foreach (IFieldObject<Player> player in Field.State.Players.Values)
                {
                    if (FieldManager.IsPlayerInBox(triggerBox, player))
                    {
                        player.Value.Session.Send(MassiveEventPacket.TextBanner(type, script, duration));
                    }
                }
                return;
            }

            // EventUI is a Round Bar UI
            string[] ids = script.Split(",");
            if (ids.Length == 2)
            {
                Field.BroadcastPacket(MassiveEventPacket.RoundBar(int.Parse(ids[0]), int.Parse(ids[1]), 1));
                return;
            }
            Field.BroadcastPacket(MassiveEventPacket.RoundBar(int.Parse(ids[0]), int.Parse(ids[1]), int.Parse(ids[2])));
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
                case 3:
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
