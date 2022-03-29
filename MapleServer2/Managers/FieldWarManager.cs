using MapleServer2.Types;
using TaskScheduler = MapleServer2.Tools.TaskScheduler;

namespace MapleServer2.Managers;

public class FieldWarManager
{
    private readonly int[] FieldWarList = new int[] { 10000001, 10000002, 10000005, 10000003 };
    private int LastFieldWarIndex;
    public FieldWar CurrentFieldWar;

    public FieldWarManager()
    {
        ScheduleEvents();
    }

    public void ScheduleEvents()
    {
        // Schedule the FieldWar event at xx:50 every 1 hour
        TaskScheduler.Instance.ScheduleTask(0, 50, 60, () =>
            {
                int fieldWarId = FieldWarList[LastFieldWarIndex];
                CurrentFieldWar = new FieldWar(fieldWarId);
                LastFieldWarIndex++;
                if (LastFieldWarIndex >= FieldWarList.Length)
                {
                    LastFieldWarIndex = 0;
                }

                Task.Delay(TimeSpan.FromMinutes(5)).Wait();
                CurrentFieldWar = null;
            });
    }
}
