using System;
using System.IO;
using AikaEmu.Shared.Utils;
using AikaEmu.WebServer.Models;
using Microsoft.Extensions.Configuration;

namespace AikaEmu.WebServer.Managers
{
    public class AppConfigManager : Singleton<AppConfigManager>
    {
        public WebServerConfig WebServerConfig { get; }

        public AppConfigManager()
        {
            WebServerConfig = new WebServerConfig();
        }

        public void Init()
        {
            var filePath = Directory.GetCurrentDirectory() + "\\Config.json";

            if (File.Exists(filePath))
            {
                var configurationBuilder = new ConfigurationBuilder().AddJsonFile(filePath).Build();
                configurationBuilder.Bind(WebServerConfig);
            }
            else
            {
                throw new Exception("Config.json file not found.");
            }
        }
    }
}