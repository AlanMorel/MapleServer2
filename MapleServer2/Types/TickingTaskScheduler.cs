using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maple2.PathEngine;
using MapleServer2.Managers;

namespace MapleServer2.Types;

public enum SkillTaskType
{
    EffectUpdateLoop
}

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
    public Func<long, TriggerTask, long> Callback; // Return value int is next wait interval. Return -1 to end or 0 to use last interval.
    public Action<long, TriggerTask>? TaskFinishedCallback;

    public long GetNextExecutionTick()
    {
        if (Delay == 0 && Interval == 0)
        {
            return LastExecutionTick + RemainingDuration;
        }

        return LastExecutionTick + (Delay != 0 ? Delay : Interval);
    }

    public TriggerTask(Func<long, TriggerTask, long> callback, IFieldActor? origin)
    {
        Origin = origin;
        Callback = callback;
    }
}

public struct TriggerTaskParameters
{
    public IFieldActor? Origin = null; // Task is cancelled when this object requests for its referenced tasks to be cleaned up
    public Object? Subject = null; // Task is cancelled when this object requests for its referenced tasks to be cleaned up
    public long Interval;
    public long Delay = 0;
    public long Duration = -1;
    public int Executions = 1;

    public TriggerTaskParameters(int interval)
    {
        Interval = interval;
    }
}

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

        newTask = new(callback, parameters.Origin)
        {
            LastExecutionTick = FieldManager.TickCount64,
            Interval = parameters.Interval,
            RemainingDuration = parameters.Duration,
            Delay = parameters.Delay,
            RemainingExecutions = parameters.Executions,
            Subject = parameters.Subject,
            TaskFinishedCallback = taskFinishedCallback
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
        lock(BufferedTasks)
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

    public void Update(long delta)
    {
        if (FieldManager is null)
        {
            return;
        }

        lock (BufferedTasks)
        {
            foreach (TriggerTask task in BufferedTasks)
            {
                QueuedTasks.Enqueue(task, task.NextExecutionTick);
            }

            BufferedTasks.Clear();
        }

        long currentTick = FieldManager.TickCount64;

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

    public void OnFieldMoved()
    {
        long currentTick = FieldManager.TickCount64;

        List<(TriggerTask, long)> newTaskQueue = new(QueuedTasks.Count);

        foreach ((TriggerTask task, long nextExecutionTick) in QueuedTasks.UnorderedItems)
        {
            task.LastExecutionTick = task.LastExecutionTick - LastFieldTick + currentTick;

            newTaskQueue.Add((task, nextExecutionTick - LastFieldTick + currentTick));
        }

        QueuedTasks.Clear();
        QueuedTasks.EnqueueRange(newTaskQueue);

        LastFieldTick = currentTick;
    }
}
