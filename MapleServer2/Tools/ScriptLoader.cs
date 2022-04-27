using System.Collections.Concurrent;
using Maple2Storage.Types;
using MapleServer2.Managers;
using MapleServer2.Servers.Game;
using MoonSharp.Interpreter;
using Serilog;

namespace MapleServer2.Tools;

public static class ScriptLoader
{
    private static readonly ConcurrentDictionary<string, Script> Scripts = new();
    private static readonly ILogger Logger = Log.Logger.ForContext(typeof(ScriptLoader));

    /// <summary>
    /// Get the script from the cache or load it if it's not in the cache.
    /// If a session is provided, it'll create a new script every time.
    /// </summary>
    /// <returns><see cref="Script"/></returns>
    public static Script GetScript(string scriptName, GameSession session = null)
    {
        // If session is not null, create a new script every time.
        if (session is not null)
        {
            NewScript(scriptName, out Script newScript, session);
            return newScript;
        }

        // If session is null, use the cached script.
        if (Scripts.TryGetValue(scriptName, out Script script))
        {
            return script;
        }

        // If the script is not in the cache, create a new script.
        if (!NewScript(scriptName, out script))
        {
            return null;
        }

        Scripts.TryAdd(scriptName, script);
        return script;
    }

    /// <summary>
    /// Calls the specified function.
    /// </summary>
    /// <exception cref="ArgumentException"></exception>
    public static DynValue RunFunction(this Script script, string functionName, params object[] args)
    {
        if (script.Globals[functionName] == null)
        {
            return null;
        }

        try
        {
            return script.Call(script.Globals[functionName], args);
        }
        catch (ArgumentException ex)
        {
            Logger.Error("Exception while running function {functionName}. {message}", functionName, ex.Message);
            return null;
        }
        catch (Exception ex)
        {
            Logger.Error("Exception while running function {functionName}. {message}", functionName, ex.Message);
            return null;
        }
    }

    private static bool NewScript(string scriptName, out Script script, GameSession session = null)
    {
        script = null;
        string scriptPath = $"{Paths.SCRIPTS_DIR}/{scriptName}.lua";
        if (!File.Exists(scriptPath))
        {
            Logger.Information("Script {scriptName} does not exist.", scriptName);
            return false;
        }

        script = new();

        if (session is not null)
        {
            // Register script manager as an proxy object
            // Documentation: https://www.moonsharp.org/proxy.html
            UserData.RegisterProxyType<ScriptManager, GameSession>(r => new(r));
            script.Globals["ScriptManager"] = new ScriptManager(session);
        }

        try
        {
            script.DoFile(scriptPath);
        }
        catch (Exception ex)
        {
            Logger.Error("Error on script {scriptName}. {message}", scriptName, ex.Message);
            return false;
        }

        return true;
    }
}
