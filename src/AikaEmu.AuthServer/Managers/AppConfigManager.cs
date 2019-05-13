using System;
using System.IO;
using AikaEmu.AuthServer.Models;
using AikaEmu.Shared.Utils;
using Microsoft.Extensions.Configuration;

namespace AikaEmu.GameServer.Managers.Configuration
{
	public class AppConfigManager : Singleton<AppConfigManager>
	{
		public AuthServerConfig AuthServerConfig { get; }

		public AppConfigManager()
		{
			AuthServerConfig = new AuthServerConfig();
		}

		public void Init()
		{
			var filePath = Directory.GetCurrentDirectory() + "\\Config.json";

			if (File.Exists(filePath))
			{
				var configurationBuilder = new ConfigurationBuilder().AddJsonFile(filePath).Build();
				configurationBuilder.Bind(AuthServerConfig);
			}
			else
			{
				throw new Exception("Config.json file not found.");
			}
		}
	}
}