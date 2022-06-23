using MapleServer2.Managers;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;

namespace MapleServer2.Types;

public class MushkingRoyaleQueue
{
    public List<Player> Players;
    private Task MatchMaking;
    private int LastJoinTick;

    public MushkingRoyaleQueue()
    {
        Players = new();
    }

    public void AddPlayer(Player player)
    {
        Players.Add(player);
        MatchMaking ??= MatchMakeLoop();
        LastJoinTick = Environment.TickCount;
    }

    public void RemovePlayer(Player player)
    {
        Players.Remove(player);
        if (Players.Count <= 0)
        {
            MatchMaking = null;
        }
    }

    private Task MatchMakeLoop()
    {
        return Task.Run(async () =>
        {
            while (Players.Count > 0)
            {
                // a match needs minimum of 20 to start. doing 25 to be safe incase of drop outs
                // TODO: Handle getting more players in while it's still open
                // TODO: Maybe royale level matching?
                if (Environment.TickCount > LastJoinTick + 60000 && Players.Count >= 25)
                {
                    StartMatch();
                }

                if (Players.Count >= 50)
                {
                    StartMatch();
                }
                await Task.Delay(10000); // 10 second delay
            }
        });
    }

    private void StartMatch()
    {
        List<Player> players = Players.Take(50).ToList();
        MushkingRoyaleSession session = new(82000001);
        foreach (Player player in players)
        {
            Players.Remove(player);
            player.MushkingRoyaleSession = session.SessionId;
            player.Session?.Send(MushkingRoyaleSystemPacket.MatchFound(session.SessionId));
        }
    }
}
