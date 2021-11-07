using MapleServer2.Servers.Game;

namespace MapleServer2.Commands.Core;

public class GameCommandTrigger : CommandTrigger
{
    public GameSession Session { get; }

    public GameCommandTrigger(string[] args, GameSession session) : base(args)
    {
        Session = session;
    }
}
