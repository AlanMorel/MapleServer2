using System.IO.Pipelines;
using System.Threading.Tasks.Dataflow;

namespace MapleServer2.Network
{
    public class QueuedPipeScheduler : PipeScheduler
    {
        private readonly BufferBlock<(Action<object> Action, object State)> Queue;

        public QueuedPipeScheduler()
        {
            Queue = new BufferBlock<(Action<object> Action, object State)>();
        }

        public Task<bool> OutputAvailableAsync() => Queue.OutputAvailableAsync();
        public void Complete() => Queue.Complete();
        public Task Completion => Queue.Completion;

        public override void Schedule(Action<object> action, object state)
        {
            Queue.Post((action, state));
        }

        public void ProcessQueue()
        {
            while (Queue.TryReceive(out (Action<object> Action, object State) item))
            {
                item.Action(item.State);
            }
        }
    }
}
