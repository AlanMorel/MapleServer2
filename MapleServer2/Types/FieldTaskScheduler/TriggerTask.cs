namespace MapleServer2.Types;

public class TriggerTask
{
    public const long End = -1;
    public const long SameInterval = 0;

    public bool Alive = true;
    public IFieldActor? Origin;
    public Object? Subject = null;
    public int RemainingExecutions = -1;
    public long LastExecutionTick = 0;
    public long Interval = 0;
    public long Delay = 0;
    public long RemainingDuration = -1;
    public long NextExecutionTick { get => GetNextExecutionTick(); }
    public bool FinishAfterDuration = false;
    public bool CanFinish { get => RemainingDuration <= 0 || !FinishAfterDuration || TaskFinishedCallback == null; }
    public Func<long, TriggerTask, long> Callback; // Return value int is next wait interval. Return -1 to end or 0 to use last interval.
    public Action<long, TriggerTask>? TaskFinishedCallback;

    public long GetNextExecutionTick(long baseTime = -1)
    {
        if (baseTime == -1)
        {
            baseTime = LastExecutionTick;
        }

        if (Delay == 0 && Interval == 0)
        {
            return baseTime + RemainingDuration;
        }

        return baseTime + (Delay != 0 ? Delay : Interval);
    }

    public TriggerTask(Func<long, TriggerTask, long> callback, IFieldActor? origin)
    {
        Origin = origin;
        Callback = callback;
    }
}
