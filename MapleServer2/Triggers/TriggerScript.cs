using Maple2.Trigger;

namespace MapleServer2.Triggers
{
    public class TriggerScript
    {
        private readonly TriggerContext Context;

        private TriggerState State;
        private TriggerState NextState;

        public TriggerScript(TriggerContext context, TriggerState start)
        {
            Context = context;
            NextState = start;
        }

        public void Next()
        {
            if (NextState != null)
            {
                State?.OnExit();
                Context.NextTick = 0;
                State = NextState;

                State.OnEnter();
                NextState = null;
            }

            NextState = State.Execute();
        }
    }
}
