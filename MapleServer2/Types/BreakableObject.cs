using MapleServer2.Managers;
using MapleServer2.Packets;

namespace MapleServer2.Types
{
    public class BreakableObject
    {
        public string Id;
        public BreakableState State;
        public bool IsEnabled;
        public int HideDuration;
        public int ResetDuration;

        public BreakableObject(string id, bool isEnabled, int hideDuration, int resetDuration)
        {
            Id = id;
            IsEnabled = isEnabled;
            State = BreakableState.Spawn;
            HideDuration = hideDuration;
            ResetDuration = resetDuration;
        }

        public Task BreakObject(FieldManager field)
        {
            return Task.Run(async () =>
            {
                State = BreakableState.Break;
                field.BroadcastPacket(BreakablePacket.Interact(this));
                await Task.Delay(HideDuration);
                State = BreakableState.Despawn;
                field.BroadcastPacket(BreakablePacket.Interact(this));
                await Task.Delay(ResetDuration);
                State = BreakableState.Spawn;
                field.BroadcastPacket(BreakablePacket.Interact(this));
            });
        }
    }

    public class BreakableNifObject : BreakableObject
    {
        public int TriggerId;

        public BreakableNifObject(string id, bool isEnabled, int triggerId, int hideDuration, int resetDuration) : base(id, isEnabled, hideDuration, resetDuration)
        {
            TriggerId = triggerId;
        }
    }

    public class BreakableActorObject : BreakableObject
    {
        public BreakableActorObject(string id, bool isEnabled, int hideDuration, int resetDuration) : base(id, isEnabled, hideDuration, resetDuration) { }
    }

    public enum BreakableState : byte
    {
        Spawn = 2,
        Break = 3,
        Despawn = 4
    }
}
