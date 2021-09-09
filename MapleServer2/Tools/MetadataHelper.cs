using System.Reflection;
using MapleServer2.Extensions;
using NLog;

namespace MapleServer2.Tools
{
    public static class MetadataHelper
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static void InitializeAll()
        {
            Logger.Warn("Initializing Data...Please Wait");

            List<Type> callingTypes = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsAbstract && t.IsClass && t.Namespace == "MapleServer2.Data.Static").ToList();
            callingTypes.ForEach(type => type.GetMethod("Init")?.Invoke(null, null));

            Logger.Info("Initializing Data...Complete!".ColorGreen());
        }
    }
}
