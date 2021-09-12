using System.Reflection;
using Maple2Storage.Extensions;
using Maple2Storage.Tools;
using NLog;

namespace MapleServer2.Tools
{
    public static class MetadataHelper
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static void InitializeAll()
        {
            Logger.Info("Initializing Data...Please Wait".ColorYellow());
            List<Type> callingTypes = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsAbstract && t.IsClass && t.Namespace == "MapleServer2.Data.Static").ToList();
            int count = 0;

            foreach (Type type in callingTypes)
            {
                type.GetMethod("Init")?.Invoke(null, null);
                count++;
                ConsoleUtility.WriteProgressBar((float) count / callingTypes.Count * 100);
            }

            Logger.Info("Initializing Data...Complete!".ColorGreen());
        }
    }
}
