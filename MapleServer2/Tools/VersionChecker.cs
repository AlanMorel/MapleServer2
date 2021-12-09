using System.Net;
using Maple2Storage.Types;
using NLog;

namespace MapleServer2.Tools;

public static class VersionChecker
{
    private const string DownloadUrl = "https://raw.githubusercontent.com/darkvergus/MapleServer2/Dev/version.txt";
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public static void CheckUpdate()
    {
        string versionString = File.ReadAllText(Paths.SOLUTION_DIR + "/version.txt");
        if (string.IsNullOrEmpty(versionString))
        {
            return;
        }
        
        try
        {
            WebRequest webRequest = WebRequest.Create(new Uri(DownloadUrl));
            WebResponse webResponse = webRequest.GetResponse();
            StreamReader streamReader = new(webResponse.GetResponseStream());
            
            Version newVersion = new(streamReader.ReadToEnd());

            Version currentVersion = new(versionString);
            
            if (currentVersion.Major.CompareTo(newVersion.Major) is not 0)
            {
                Logger.Error("The Server has a new Major Version. It is seriously recommened that you update it!");
            } 
            else if (currentVersion.Minor.CompareTo(newVersion.Minor) is not 0)
            {
                Logger.Warn("The Server has a new Minor Version. It is recommened that you update it!");
            } 
            else if (currentVersion.Build.CompareTo(newVersion.Build) is not 0)
            {
                Logger.Info("The Server has a new Build Version.");
            }
            else
            {
                Logger.Info("The Server is Up-to-date.");
            }
            
        }
        catch (WebException)
        {
            Logger.Error("The file couldn't be found in the repository!");
        }
    }
}
