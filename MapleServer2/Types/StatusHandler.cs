using MapleServer2.Packets;
using MapleServer2.Servers.Game;

namespace MapleServer2.Types;

public static class StatusHandler
{
    // Public caller to handle status on Player
    // TODO: Handle Add Stacks to status.
    public static void Handle(GameSession session, Status status)
    {
        session.FieldManager.BroadcastPacket(BuffPacket.SendBuff(0, status));
        Remove(session, status);
    }

    private static Task Remove(GameSession session, Status status)
    {
        return Task.Run(async () =>
        {
            await Task.Delay(status.Duration);
            session.FieldManager.BroadcastPacket(BuffPacket.SendBuff(1, status));
        });
    }
}
