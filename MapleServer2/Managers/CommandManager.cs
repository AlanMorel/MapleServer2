using System.Reflection;
using MapleServer2.Commands.Core;
using Serilog;

namespace MapleServer2.Managers;

public class CommandManager
{
    private readonly ILogger Logger = Log.Logger.ForContext<CommandManager>();

    public static Dictionary<string, CommandBase> CommandsByAlias { get; set; }

    public CommandManager()
    {
        CommandsByAlias = new();
    }

    public void RegisterAll(Assembly assembly)
    {
        if (assembly == null)
        {
            Logger.Error("Current Assembly was null.");
            return;
        }
        IEnumerable<Type> callTypes = assembly.GetTypes().Where(entry => !entry.IsAbstract);

        foreach (Type commandType in callTypes)
        {
            if (commandType.IsSubclassOf(typeof(CommandBase)))
            {
                RegisterCommand(commandType);
            }
        }
    }

    private void RegisterCommand(Type commandType)
    {
        if (Activator.CreateInstance(commandType) is not CommandBase instanceCommand)
        {
            Logger.Error("Cannot create a new instance of {commandType}", commandType);
            return;
        }

        if (instanceCommand.Aliases == null)
        {
            Logger.Error("Cannot register Command {name}, Aliases is null", commandType.Name);
            return;
        }

        foreach (string alias in instanceCommand.Aliases)
        {
            if (!CommandsByAlias.TryGetValue(alias, out CommandBase command))
            {
                CommandsByAlias[alias.ToLower()] = instanceCommand;
            }
            else
            {
                Logger.Error("Found two Commands with Alias \"{alias}\": {command} and {instanceCommand}", alias, command, instanceCommand);
            }
        }
    }

    public bool HandleCommand(CommandTrigger trigger)
    {
        if (trigger == null)
        {
            Logger.Warning("No CommandTrigger were pass.");
            return false;
        }

        if (!CommandsByAlias.TryGetValue(trigger.Args[0].ToLower(), out CommandBase command))
        {
            return false;
        }

        if (!trigger.DefinedParametersCommand(command))
        {
            Logger.Information("No Parameters to defined in this command.");
            return false;
        }
        command.Execute(trigger);
        return true;
    }
}
