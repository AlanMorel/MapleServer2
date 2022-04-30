using Serilog;

namespace MapleServer2.Tools;

public static class TaskUtils
{
    public static void WaitForTask(Task task)
    {
        if (task is null)
        {
            return;
        }

        try
        {
            task.Wait();
        }
        catch (AggregateException) { } // CancellationToken.Cancel(), then task.Wait() always throws AggregateException
        catch (Exception e)
        {
            Log.Logger.Error(e.ToString());
        }
    }
}
