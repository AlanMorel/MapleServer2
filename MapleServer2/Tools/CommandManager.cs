using System.Data;
using System.Reflection;
using MapleServer2.Commands.Core;
using NLog;
using Pastel;

namespace MapleServer2.Tools
{
    public class CommandManager
    {
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static Dictionary<string, CommandBase> CommandsByAlias { get; set; }

        public CommandManager()
        {
            CommandsByAlias = new Dictionary<string, CommandBase>();
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
            Logger.Info("Commands loaded.".Pastel("#aced66"));
        }

        private void RegisterCommand(Type commandType)
        {
            if (Activator.CreateInstance(commandType) is not CommandBase instanceCommand)
            {
                Logger.Error("Cannot create a new instance of {0}", commandType);
                return;
            }
            if (instanceCommand.Aliases == null)
            {
                Logger.Error("Cannot register Command {0}, Aliases is null", commandType.Name);
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
                    Logger.Error("Found two Commands with Alias \"{0}\": {1} and {2}", alias, command, instanceCommand);
                }
            }
        }

        public void HandleCommand(CommandTrigger trigger)
        {
            if (trigger == null)
            {
                Logger.Error("No CommandTrigger were pass.");
                return;
            }
            // Assuming the first argument is the command:
            CommandBase command = GetCommand(trigger.Args[0]);

            if (command == null)
            {
                Logger.Error("No Command were found with the current alias.");
                return;
            }
            if (!trigger.DefinedParametersCommand(command))
            {
                Logger.Error("No Parameters to defined in this command.");
                return;
            }
            command.Execute(trigger);
        }

        public static CommandBase GetCommand(string alias)
        {
            CommandsByAlias.TryGetValue(alias, out CommandBase command);
            return command;
        }
    }
}
