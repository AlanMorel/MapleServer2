using System.Net;
using NLog;
namespace MapleServer2.Tools;

public class VersionChecker
{
    private const string Version = "0.0.1";
    private const string DownloadUrl = "https://github.com/darkvergus/MapleServer2/version.txt";
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public static void CheckUpdate()
    {
        try
        {
            WebRequest wr = WebRequest.Create(new Uri(DownloadUrl));
            WebResponse ws = wr.GetResponse();
            StreamReader sr = new(ws.GetResponseStream());

            string newVersion = sr.ReadToEnd();

            if (Version.Contains(newVersion))
            {
                Logger.Info("Server is up to date!");
            }
            else
            {
                Logger.Warn("Server is out of date. Please consider downloading the new version at https://github.com/AlanMorel/MapleServer2/.");
            }
        }
        catch (WebException e)
        {
            Logger.Error("The file couldn't be found in the repository!");
        }
    }
}
