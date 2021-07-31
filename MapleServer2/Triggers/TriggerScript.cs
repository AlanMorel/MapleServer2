using System;
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
            //if (Environment.TickCount < Context.NextTick)
            //{
            //    return;
            //}

            //Context.NextTick = Environment.TickCount + 200; // Wait 200ms between execution

            if (NextState != null)
            {
                State?.OnExit();
                Context.NextTick = 0; //ADDED
                State = NextState;
                State.OnEnter();
                NextState = null;
            }

            NextState = State.Execute();
        }
    }
}
