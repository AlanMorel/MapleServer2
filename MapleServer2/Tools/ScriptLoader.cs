using Maple2Storage.Types;
using MapleServer2.Managers;
using MapleServer2.Servers.Game;
using MoonSharp.Interpreter;
using Serilog;

namespace MapleServer2.Tools;

public class ScriptLoader
{
    private readonly ILogger Logger = Log.Logger.ForContext<ScriptLoader>();

    public Script Script;
    private readonly string ScriptName;

    /// <summary>
    /// Loads an script from the Scripts folder.
    /// </summary>
    public ScriptLoader(string scriptName, GameSession session = null)
    {
        string scriptPath = $"{Paths.SCRIPTS_DIR}/{scriptName}.lua";
        if (!File.Exists(scriptPath))
        {
            return;
        }

        ScriptName = scriptName;
        Script = new();

        if (session != null)
        {
            // Register script manager as an proxy object
            // Documentation: https://www.moonsharp.org/proxy.html
            UserData.RegisterProxyType<ScriptManager, GameSession>(r => new(r));
            Script.Globals["PlayerHelper"] = new ScriptManager(session);
        }

        try
        {
            Script.DoFile(scriptPath);
        }
        catch (Exception ex)
        {
            Logger.Error(ex.Message);
        }
    }

    /// <summary>
    /// Calls the specified function.
    /// </summary>
    /// <exception cref="ArgumentException"></exception>
    public DynValue Call(string functionName, params object[] args)
    {
        if (Script.Globals[functionName] == null)
        {
            return null;
        }

        try
        {
            return Script.Call(Script.Globals[functionName], args);
        }
        catch (ArgumentException ex)
        {
            Logger.Error("Error on script {name}. {message}", ScriptName, ex.Message);
            return null;
        }
        catch (Exception ex)
        {
            Logger.Error("Error on script {name}. {message}", ScriptName, ex.Message);
            return null;
        }
    }
}
