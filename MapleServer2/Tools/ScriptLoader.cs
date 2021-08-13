using System;
using MoonSharp.Interpreter;
using NLog;

namespace MapleServer2.Tools
{
    public class ScriptLoader
    {
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public Script Script;
        private readonly string ScriptName;

        /// <summary>
        /// Loads an script from the Scripts folder.
        /// </summary>
        public ScriptLoader(string scriptName)
        {
            ScriptName = scriptName;
            Script = new Script();
            try
            {
                Script.DoFile($"Scripts/{scriptName}.lua");
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
        }

        /// <summary>
        /// Calls the function with the same name as the script.
        /// </summary>
        /// <returns>Returns DynValue or null if function was not found</returns>
        public DynValue Call(params object[] args)
        {
            try
            {
                return Script.Call(Script.Globals[ScriptName], args);
            }
            catch (ArgumentException ex)
            {
                Logger.Error(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Calls the specified function.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public DynValue Call(string functionName, params object[] args)
        {
            try
            {
                return Script.Call(Script.Globals[functionName], args);
            }
            catch (ArgumentException ex)
            {
                Logger.Error(ex.Message);
                return null;
            }
        }
    }
}
