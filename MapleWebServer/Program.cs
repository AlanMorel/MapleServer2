using Maple2Storage.Tools;
using MapleWebServer.Constants;

namespace MapleWebServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Load .env file
            string dotenv = Path.Combine(Paths.SOLUTION_DIR, ".env");

            if (!File.Exists(dotenv))
            {
                throw new ArgumentException(".env file not found!");
            }
            DotEnv.Load(dotenv);

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                    .UseUrls(urls: $"http://*:{Environment.GetEnvironmentVariable("WEB_PORT")}");
                });
    }
}
