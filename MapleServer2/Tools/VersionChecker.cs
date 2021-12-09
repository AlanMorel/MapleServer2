using System.Net;
using NLog;

namespace MapleServer2.Tools;

public static class VersionChecker
{
    private static readonly Version Version = new("0.1.0");
    private const string DownloadUrl = "https://github.com/darkvergus/MapleServer2/version.txt";
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public static void CheckUpdate()
    {
        try
        {
            WebRequest webRequest = WebRequest.Create(new Uri(DownloadUrl));
            WebResponse webResponse = webRequest.GetResponse();
            StreamReader streamReader = new(webResponse.GetResponseStream());

            Version newVersion = new(streamReader.ReadToEnd());

            if (Version.Major.CompareTo(newVersion.Major) != 0)
            {
                Logger.Error("The Server has a new Major Version. It is seriously recommened that you update it!");
            } 
            else if (Version.Minor.CompareTo(newVersion.Minor) != 0)
            {
                Logger.Warn("The Server has a new Minor Version. It is recommened that you update it!");
            } 
            else if (Version.Build.CompareTo(newVersion.Build) != 0)
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
