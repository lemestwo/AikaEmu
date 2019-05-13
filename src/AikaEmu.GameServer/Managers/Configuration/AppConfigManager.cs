using System;
using System.IO;
using AikaEmu.GameServer.Models.Data;
using AikaEmu.Shared.Utils;
using Microsoft.Extensions.Configuration;

namespace AikaEmu.GameServer.Managers.Configuration
{
	public class AppConfigManager : Singleton<AppConfigManager>
	{
		public GameServerConfig GameServerConfig { get; }

		public AppConfigManager()
		{
			GameServerConfig = new GameServerConfig();
		}

		public void Init()
		{
			var filePath = Directory.GetCurrentDirectory() + "\\Config.json";

			if (File.Exists(filePath))
			{
				var configurationBuilder = new ConfigurationBuilder().AddJsonFile(filePath).Build();
				configurationBuilder.Bind(GameServerConfig);
			}
			else
			{
				throw new Exception("Config.json file not found.");
			}
		}
	}
}