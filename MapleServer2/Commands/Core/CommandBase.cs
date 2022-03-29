using Serilog;

namespace MapleServer2.Commands.Core;

public abstract class CommandBase
{
    protected static readonly ILogger Logger = Log.Logger.ForContext<CommandBase>();

    public List<string> Aliases { get; protected set; }
    public string Description { get; set; }
    public List<IParameter> Parameters { get; protected set; }
    public string Usage { get; protected set; }

    public CommandBase()
    {
        Parameters = new();
    }

    public abstract void Execute(CommandTrigger trigger);

    public override string ToString()
    {
        return GetType().Name;
    }
}
