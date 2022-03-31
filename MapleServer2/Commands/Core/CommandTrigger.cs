using Maple2Storage.Types;
using Serilog;

namespace MapleServer2.Commands.Core;

public abstract class CommandTrigger
{
    protected static readonly ILogger Logger = Log.Logger.ForContext<CommandTrigger>();

    public string[] Args { get; private set; }
    public CommandBase Command { get; private set; }
    internal Dictionary<string, IParameter> CommandsParametersByName { get; private set; }

    public CommandTrigger(string[] args)
    {
        CommandsParametersByName = new();
        Args = args;
    }

    public virtual T Get<T>(string name)
    {
        if (CommandsParametersByName.ContainsKey(name))
        {
            return (T) CommandsParametersByName[name].DefaultValue;
        }
        Logger.Error("{name} is not an existing parameter.", name);
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
        List<IParameter> definedParam = new(Command.Parameters);
        int index = 0;

        if (definedParam.Count != 0)
        {
            Command.Parameters.ForEach(x => x.DefaultValue = default);
        }

        foreach (string arg in Args)
        {
            try
            {
                if (command.Aliases.Any(x => string.Equals(x, arg, StringComparison.CurrentCultureIgnoreCase)) || definedParam.Count < index)
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
                Logger.Warning("Error parsing argument => {arg}\n Make sure you have either defined the parameter or the argument is correct.", arg);
            }
        }
        CommandsParametersByName = Command.Parameters.ToDictionary(entry => entry.Name);
        return true;
    }
}
