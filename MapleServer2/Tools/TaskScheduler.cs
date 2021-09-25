namespace MapleServer2.Tools
{
    public class TaskScheduler
    {
        private static TaskScheduler _instance;
        private readonly List<Timer> Timers = new List<Timer>();

        private TaskScheduler() { }

        public static TaskScheduler Instance => _instance ??= new TaskScheduler();

        public void ScheduleTask(int hour, int min, double intervalInHour, Action task)
        {
            DateTime now = DateTime.Now;
            DateTime firstRun = new DateTime(now.Year, now.Month, now.Day, hour, min, 0, 0);
            if (now > firstRun)
            {
                firstRun = firstRun.AddDays(1);
            }

            TimeSpan timeToGo = firstRun - now;
            if (timeToGo <= TimeSpan.Zero)
            {
                timeToGo = TimeSpan.Zero;
            }

            Timer timer = new Timer(x =>
            {
                task.Invoke();
            }, null, timeToGo, TimeSpan.FromHours(intervalInHour));

            Timers.Add(timer);
        }
    }
}
