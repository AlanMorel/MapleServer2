namespace MapleServer2.Types;

public struct TriggerTaskParameters
{
    public IFieldActor? Origin = null; // Task is cancelled when this object requests for its referenced tasks to be cleaned up
    public Object? Subject = null; // Task is cancelled when this object requests for its referenced tasks to be cleaned up
    public long Interval;
    public long Delay = 0;
    public long Duration = -1;
    public int Executions = 1;
    public bool FinishAfterDuration = false;

    public TriggerTaskParameters(int interval)
    {
        Interval = interval;
    }
}
