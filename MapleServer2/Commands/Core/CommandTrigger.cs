using Maple2Storage.Types;
using NLog;

namespace MapleServer2.Commands.Core
{
    public abstract class CommandTrigger
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public string[] Args { get; private set; }
        public CommandBase Command { get; private set; }
        internal Dictionary<string, IParameter> CommandsParametersByName { get; private set; }

        public CommandTrigger(string[] args)
        {
            CommandsParametersByName = new Dictionary<string, IParameter>();
            Args = args;
        }

        public virtual T Get<T>(string name)
        {
            if (CommandsParametersByName.ContainsKey(name))
            {
                return (T) CommandsParametersByName[name].DefaultValue;
            }
            Logger.Error("{0} is not an existing parameter.", name);
            return default;
        }

        public bool DefinedParametersCommand(CommandBase command)
        {
            if (Args.Length == 0)
            {
                Logger.Error("No args provided in this trigger.");
                return false;
            }

            Command = command;
            List<IParameter> definedParam = new List<IParameter>(Command.Parameters);
            int index = 0;

            if (definedParam.Count != 0)
            {
                Command.Parameters.ForEach(x => x.DefaultValue = default);
            }

            foreach (string arg in Args)
            {
                try
                {
                    if (command.Aliases.Any(x => x == arg) || definedParam.Count < index)
                    {
                        continue;
                    }
                    if (string.IsNullOrEmpty(arg))
                    {
                        definedParam[index].SetDefaultValue();
                        CommandsParametersByName = Command.Parameters.ToDictionary(entry => entry.Name);
                        return true;
                    }
                    if (definedParam[index].ValueType.IsArray)
                    {
                        definedParam[index].SetValue(Args[index..]);
                        CommandsParametersByName = Command.Parameters.ToDictionary(entry => entry.Name);
                        return true;
                    }
                    if (definedParam[index].ValueType == typeof(CoordF))
                    {
                        CommandHelpers.TryParseCoord(index, Args, out CoordF coord);
                        definedParam[index].SetValue(coord);
                        index += 3;
                        continue;
                    }
                    definedParam[index].SetValue(arg);
                    index++;
                }
                catch (Exception)
                {
                    Logger.Warn($"Error parsing argument => {arg}\n Make sure you have either defined the parameter or the argument is correct.");
                    continue;
                }
            }
            CommandsParametersByName = Command.Parameters.ToDictionary(entry => entry.Name);
            return true;
        }
    }
}
