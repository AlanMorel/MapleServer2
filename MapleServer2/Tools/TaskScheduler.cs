namespace MapleServer2.Tools;

public class TaskScheduler
{
    private static TaskScheduler _instance;
    private readonly List<Timer> Timers = new();

    private TaskScheduler() { }

    public static TaskScheduler Instance => _instance ??= new();

    public void ScheduleTask(int hour, int min, double intervalInMinutes, Action task)
    {
        DateTime now = DateTime.Now;
        DateTime firstRun = new(now.Year, now.Month, now.Day, hour, min, 0, 0);
        while (now > firstRun)
        {
            firstRun = firstRun.AddMinutes(intervalInMinutes);
        }

        TimeSpan timeToGo = firstRun - now;
        if (timeToGo <= TimeSpan.Zero)
        {
            timeToGo = TimeSpan.Zero;
        }

        Timer timer = new(x =>
        {
            task.Invoke();
        }, null, timeToGo, TimeSpan.FromMinutes(intervalInMinutes));

        Timers.Add(timer);
    }
}
