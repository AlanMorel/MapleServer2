using Maple2.Trigger;
using Maple2.Trigger.Enum;

namespace MapleServer2.Triggers
{
    public partial class TriggerContext
    {
        public void CreateWidget(WidgetType type)
        {
        }
        
        public void WidgetAction(WidgetType type, string name, string args, int widgetArgNum)
        {
        }
        
        public void GuideEvent(int eventId)
        {
        }

        public void HideGuideSummary(int entityId, int textId)
        {
        }
        
        public void Notice(bool arg1, string arg2, bool arg3)
        {
        }

        public void PlaySystemSoundByUserTag(int userTagId, string soundKey)
        {
        }

        public void PlaySystemSoundInBox(int[] arg1, string arg2)
        {
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
        
        public void SetEventUI(byte arg1, string script, int arg3, string arg4)
        {
        }
        
        public void SetVisibleUI(string uiName, bool visible)
        {
        }

        public void ShowCountUI(string text, byte stage, byte count, byte soundType)
        {
        }
        
        public void ShowRoundUI(byte round, int duration)
        {
        }
        
        public void ShowGuideSummary(int entityId, int textId, int duration)
        {
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
        }
        
        public void SetSceneSkip(TriggerState state, string arg2)
        {
        }

        public void SetSkip(TriggerState state)
        {
        }
    }
}
