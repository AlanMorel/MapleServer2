using MapleServer2.Managers;

namespace MapleServer2.Types;

public class TickingTaskScheduler
{
    private long CurrentTick64 = 0;
    public long CurrentTick { get => (int) CurrentTick64; }
    public FieldManager FieldManager;
    private PriorityQueue<TriggerTask, long> QueuedTasks = new();
    private List<TriggerTask> BufferedTasks = new();

    public TickingTaskScheduler(FieldManager fieldManager)
    {
        FieldManager = fieldManager;
    }

    private long GetFieldTickCount()
    {
        long currentTick = FieldManager.TickCount64;

        if (LastFieldTick == 0)
        {
            LastFieldTick = FieldManager.TickCount64;
        }

        if (currentTick == 0)
        {
            currentTick = LastFieldTick;
        }

        return currentTick;
    }

    private bool MakeTask(TriggerTaskParameters parameters, Func<long, TriggerTask, long> callback, Action<long, TriggerTask>? taskFinishedCallback, out TriggerTask? newTask)
    {
        newTask = null;

        if (FieldManager is null)
        {
            return false;
        }

        if (parameters.Interval == 0 && parameters.Executions == -1)
        {
            return false;
        }

        long currentTick = GetFieldTickCount();

        newTask = new(callback, parameters.Origin)
        {
            LastExecutionTick = currentTick,
            Interval = parameters.Interval,
            RemainingDuration = parameters.Duration,
            Delay = parameters.Delay,
            RemainingExecutions = parameters.Executions,
            Subject = parameters.Subject,
            TaskFinishedCallback = taskFinishedCallback,
            FinishAfterDuration = parameters.FinishAfterDuration,
        };

        return true;
    }

    public void QueueTask(TriggerTaskParameters parameters, Func<long, TriggerTask, long> callback, Action<long, TriggerTask>? taskFinishedCallback = null)
    {
        if (!MakeTask(parameters, callback, taskFinishedCallback, out TriggerTask? newTask) || newTask is null)
        {
            return;
        }

        QueuedTasks.Enqueue(newTask, newTask.NextExecutionTick);
    }

    public void QueueBufferedTask(TriggerTaskParameters parameters, Func<long, TriggerTask, long> callback, Action<long, TriggerTask>? taskFinishedCallback = null)
    {
        lock (BufferedTasks)
        {
            if (!MakeTask(parameters, callback, taskFinishedCallback, out TriggerTask? newTask) || newTask is null)
            {
                return;
            }

            BufferedTasks.Add(newTask);
        }
    }

    public void QueueBufferedTask(Action bufferedTaskCallback)
    {
        QueueBufferedTask(new(0)
        {
            Executions = 1
        }, (currentTick, task) => { bufferedTaskCallback(); return -1; });
    }

    public void RemoveTasksFromOrigin(IFieldActor? origin)
    {
        foreach ((TriggerTask task, long nextExecutionTick) in QueuedTasks.UnorderedItems)
        {
            if (task.Origin == origin)
            {
                task.Alive = false;
            }
        }
    }

    public void RemoveTasksFromSubject(Object subject)
    {
        foreach ((TriggerTask task, long nextExecutionTick) in QueuedTasks.UnorderedItems)
        {
            if (task.Subject == subject)
            {
                task.Alive = false;
            }
        }
    }

    public void RemoveTasks(IFieldActor? origin, Object? subject)
    {
        foreach ((TriggerTask task, long nextExecutionTick) in QueuedTasks.UnorderedItems)
        {
            if (task is not null && task.Origin == origin && task.Subject == subject)
            {
                task.Alive = false;
            }
        }
    }

    private long LastFieldTick = 0;
    private bool NeedsToRequeue = false;

    public void Update(long delta)
    {
        if (FieldManager is null)
        {
            return;
        }

        if (NeedsToRequeue)
        {
            RequeueTasks();
        }

        lock (BufferedTasks)
        {
            foreach (TriggerTask task in BufferedTasks)
            {
                QueuedTasks.Enqueue(task, task.NextExecutionTick);
            }

            BufferedTasks.Clear();
        }

        long currentTick = GetFieldTickCount();

        LastFieldTick = currentTick;

        while (QueuedTasks.Count > 0)
        {
            TriggerTask nextTask = QueuedTasks.Peek();

            if (!(nextTask?.Alive ?? false))
            {
                QueuedTasks.Dequeue();

                continue;
            }

            long timeToNextTick = nextTask.NextExecutionTick - currentTick;
            long desiredDelta = nextTask.GetNextExecutionTick(0);

            if (timeToNextTick > desiredDelta)
            {
                nextTask.LastExecutionTick = nextTask.GetNextExecutionTick(currentTick) - desiredDelta;
                timeToNextTick = nextTask.NextExecutionTick - currentTick;
            }

            if (timeToNextTick > 0)
            {
                break;
            }

            if (nextTask.Delay == 0 && nextTask.Interval == 0)
            {
                if (nextTask.TaskFinishedCallback is not null)
                {
                    nextTask.TaskFinishedCallback(currentTick, nextTask);
                }

                for (int i = 0; i < nextTask.RemainingExecutions; ++i)
                {
                    if (nextTask.Callback(currentTick, nextTask) == -1)
                    {
                        break;
                    }
                }

                QueuedTasks.Dequeue();

                continue;
            }

            if (nextTask.RemainingExecutions > 0)
            {
                nextTask.RemainingExecutions--;
            }

            nextTask.LastExecutionTick += (nextTask.Delay != 0 ? nextTask.Delay : nextTask.Interval);

            if (nextTask.RemainingDuration != -1 && nextTask.Delay == 0)
            {
                nextTask.RemainingDuration = Math.Max(0, nextTask.RemainingDuration - nextTask.Interval);
            }

            long nextInterval = -1;

            nextTask.Delay = 0;

            if (nextTask.RemainingDuration != 0)
            {
                nextInterval = nextTask.Callback(currentTick, nextTask);
            }

            if (nextInterval > 0)
            {
                nextTask.Interval = nextInterval;
            }

            if (nextInterval == -1 || nextTask.RemainingExecutions == 0 || nextTask.RemainingDuration == 0)
            {
                QueuedTasks.Dequeue();

                if (nextTask.TaskFinishedCallback is not null)
                {
                    nextTask.TaskFinishedCallback(currentTick, nextTask);
                }

                continue;
            }
            
            QueuedTasks.EnqueueDequeue(nextTask, nextTask.NextExecutionTick);
        }
    }
    
    public void RequeueTasks()
    {
        long currentTick = GetFieldTickCount();

        List<(TriggerTask, long)> newTaskQueue = new(QueuedTasks.Count);

        foreach ((TriggerTask task, long nextExecutionTick) in QueuedTasks.UnorderedItems)
        {
            task.LastExecutionTick = task.LastExecutionTick - LastFieldTick + currentTick;

            newTaskQueue.Add((task, nextExecutionTick - LastFieldTick + currentTick));
        }

        QueuedTasks.Clear();
        QueuedTasks.EnqueueRange(newTaskQueue);

        LastFieldTick = currentTick;

        NeedsToRequeue = false;
    }

    public void OnFieldMoved()
    {
        NeedsToRequeue = true;
    }
}
