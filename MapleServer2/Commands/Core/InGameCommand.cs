namespace MapleServer2.Commands.Core;

public abstract class InGameCommand : CommandBase
{
    public override void Execute(CommandTrigger trigger)
    {
        if (trigger is not GameCommandTrigger commandTrigger)
        {
            Logger.Error("This command can only be executed in game.");
            return;
        }
        Execute(commandTrigger);
    }

    public abstract void Execute(GameCommandTrigger trigger);
}
