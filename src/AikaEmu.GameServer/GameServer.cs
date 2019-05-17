using System;
using System.IO;
using System.Net;
using System.Reflection;
using AikaEmu.GameServer.Managers;
using AikaEmu.GameServer.Managers.Configuration;
using AikaEmu.GameServer.Managers.Id;
using AikaEmu.GameServer.Network.AuthServer;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network.Type;
using NLog;
using NLog.Config;

namespace AikaEmu.GameServer
{
    public static class GameServer
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private static Server _gameNetServer;
        private static Client _authGameServer;

        public static AuthGameConnection AuthGameConnection = null;
        public const ushort SystemSenderMsg = 30000;
        public const ushort SystemSender = 30005;

        public static void Run()
        {
            Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location));

            Console.Title = "AikaEmu GameServer (LOADING)";

            // LogManager
            SetupLogManager();

            // Managers
            AppConfigManager.Instance.Init();
            DatabaseManager.Instance.Init(AppConfigManager.Instance.GameServerConfig.Database);
            DataManager.Instance.Init();
            NationManager.Instance.Init();

            // IdFactory
            IdConnectionManager.Instance.Init();
            IdUnitSpawnManager.Instance.Init(3000);
            IdMobSpawnManager.Instance.Init(5000);
            IdTradeManager.Instance.Init();
            IdPartyManager.Instance.Init();

            // Spawn
            WorldManager.SpawnUnits();

            // GameServer Setup
            var cNetwork = AppConfigManager.Instance.GameServerConfig.Network;
            var networkAddress = new IPEndPoint(cNetwork.Host.Equals("*") ? IPAddress.Any : IPAddress.Parse(cNetwork.Host), cNetwork.Port);
            _gameNetServer = new Server(networkAddress, 2000, new GameProtocol());
            _gameNetServer.Start();
            Log.Debug("GameServer listening at {0}:{1}.", cNetwork.Host.Equals("*") ? "0.0.0.0" : cNetwork.Host, cNetwork.Port);

            // Internal Client Setup
            var cIntNetwork = AppConfigManager.Instance.GameServerConfig.AuthNetwork;
            var authNetAddress = new IPEndPoint(IPAddress.Parse(cIntNetwork.Host), cIntNetwork.Port);
            _authGameServer = new Client(authNetAddress, new AuthGameProtocol());
            _authGameServer.Start();
            Log.Debug("InternalServer listening at {0}:{1}.", cIntNetwork.Host, cIntNetwork.Port);

            // Console Setup
            Console.Title = "AikaEmu GameServer (RUNNING)";
            Log.Info("GameServer started with success.");
            Console.ReadLine();
        }

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