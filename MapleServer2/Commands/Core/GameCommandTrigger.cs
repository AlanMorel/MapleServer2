using MapleServer2.Enums;
using MapleServer2.Servers.Game;

namespace MapleServer2.Commands.Core;

public class GameCommandTrigger : CommandTrigger
{
    public GameSession Session { get; }
    public ChatType ChatType { get; }

    public GameCommandTrigger(string[] args, GameSession session, ChatType chatType) : base(args)
    {
        Session = session;
        ChatType = chatType;
    }
}
