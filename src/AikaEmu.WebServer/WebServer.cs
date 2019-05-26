using System;
using System.IO;
using System.Reflection;
using AikaEmu.WebServer.Managers;
using Microsoft.AspNetCore.Hosting;
using NLog;
using NLog.Config;

namespace AikaEmu.WebServer
{
    public static class WebServer
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private static string _defaultDir;

        public static void Run()
        {
            _defaultDir = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location));

            Console.Title = "AikaEmu WebServer";

            SetupLogManager();
            AppConfigManager.Instance.Init();
            DataAuthManager.Instance.Init(AppConfigManager.Instance.WebServerConfig.DatabaseAuth);
            DataGameManager.Instance.Init(AppConfigManager.Instance.WebServerConfig.DatabaseGame);

            CreateWebHostBuilder().Build().Run();
        }

        private static IWebHostBuilder CreateWebHostBuilder() =>
            new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(_defaultDir)
                .UseIISIntegration()
                .UseSetting("https_port", "8090")
                .UseStartup<Startup>()
                .UseUrls("https://*:8090/");

        private static void SetupLogManager()
        {
            var filePath = Directory.GetCurrentDirectory() + "\\NLog.config";
            if (File.Exists(filePath))
            {
                LogManager.Configuration = new XmlLoggingConfiguration(filePath, false);
            }
            else
            {
                throw new Exception("NLog.config not found.");
            }
        }
    }
}