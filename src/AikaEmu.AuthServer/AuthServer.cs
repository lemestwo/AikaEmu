using System;
using System.IO;
using System.Net;
using System.Reflection;
using AikaEmu.AuthServer.Managers;
using AikaEmu.AuthServer.Managers.Id;
using AikaEmu.AuthServer.Network.AuthServer;
using AikaEmu.AuthServer.Network.GameServer;
using AikaEmu.GameServer.Managers.Configuration;
using AikaEmu.Shared.Network.Type;
using NLog;
using NLog.Config;

namespace AikaEmu.AuthServer
{
    public static class AuthServer
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private static Server _authServer;
        private static Server _gameAuthServer;

        public static void Run()
        {
            Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location));

            Console.Title = "AikaEmu AuthServer (LOADING)";

            // LogManager
            SetupLogManager();

            // Data Setup
            AppConfigManager.Instance.Init();
            DatabaseManager.Instance.Init(AppConfigManager.Instance.AuthServerConfig.Database);
            AuthGameManager.Instance.Init();

            // IdFactory
            IdSerialManager.Instance.Init(56500u);

            // AuthServer Setup
            var cNetwork = AppConfigManager.Instance.AuthServerConfig.Network;
            var networkAddress = new IPEndPoint(cNetwork.Host.Equals("*") ? IPAddress.Any : IPAddress.Parse(cNetwork.Host), cNetwork.Port);
            _authServer = new Server(networkAddress, 10, new AuthProtocol());
            _authServer.Start();
            Log.Debug("AuthServer listening at {0}:{1}.", cNetwork.Host.Equals("*") ? "0.0.0.0" : cNetwork.Host, cNetwork.Port);

            // InternalServer Setup
            var cIntNetwork = AppConfigManager.Instance.AuthServerConfig.InternalNetwork;
            var intNetAddress = new IPEndPoint(cIntNetwork.Host.Equals("*") ? IPAddress.Any : IPAddress.Parse(cIntNetwork.Host), cIntNetwork.Port);
            _gameAuthServer = new Server(intNetAddress, 10, new GameAuthProtocol());
            _gameAuthServer.Start();
            Log.Debug("InternalServer listening at {0}:{1}.", cIntNetwork.Host.Equals("*") ? "0.0.0.0" : cIntNetwork.Host, cIntNetwork.Port);

            // Console Setup
            Console.Title = "AikaEmu AuthServer (RUNNING)";
            Log.Info("AikaEmu Authentication Server started with success.");

            // Wait for input
            Console.ReadLine();

            Log.Info("Starting shutdown proccess...");
            // Stop Server
            if (_authServer.IsStarted)
            {
                _authServer.Stop();
                Log.Info("AuthServer stopped.");
            }

            if (_gameAuthServer.IsStarted)
            {
                _gameAuthServer.Stop();
                Log.Info("GameAuthServer stopped.");
            }

            // TODO - PROPER SHUTDOWN
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