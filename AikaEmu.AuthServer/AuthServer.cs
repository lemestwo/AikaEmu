using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using AikaEmu.AuthServer.Configuration;
using AikaEmu.AuthServer.Controllers;
using AikaEmu.AuthServer.Models;
using AikaEmu.AuthServer.Network;
using AikaEmu.AuthServer.Network.AuthServer;
using AikaEmu.AuthServer.Network.GameServer;
using AikaEmu.Shared;
using AikaEmu.Shared.Database;
using AikaEmu.Shared.Network;
using AikaEmu.Shared.Network.Type;
using Microsoft.Extensions.Configuration;

namespace AikaEmu.AuthServer
{
    public class AuthServer : BaseProgram
    {
        public static readonly AuthServer Instance = new AuthServer();
        public DatabaseManager DatabaseManager { get; private set; }
        private AuthConfig _authConfig = new AuthConfig();

        private Server _authServer;
        private Server _gameAuthServer;

        public override void Start()
        {
            Console.Title = "AikaEmu AuthServer (LOADING)";
            base.Start();

            // Basic Setup
            SetupConfig(ref _authConfig);
            SetupDatabase(DatabaseManager = new DatabaseManager(), _authConfig.Database);
            GameAuthController.Instance.Init();

            // Auth Server Setup
            var cNetwork = _authConfig.Network;
            var networkAddress = new IPEndPoint(cNetwork.Host.Equals("*") ? IPAddress.Any : IPAddress.Parse(cNetwork.Host), cNetwork.Port);
            _authServer = new Server(networkAddress, 10, new AuthProtocol());
            _authServer.Start();
            Log.Info("AuthServer listening at {0}:{1}.", cNetwork.Host.Equals("*") ? "0.0.0.0" : cNetwork.Host, cNetwork.Port);

            // Game-Auth Server Setup
            var cIntNetwork = _authConfig.InternalNetwork;
            var intNetAddress = new IPEndPoint(cIntNetwork.Host.Equals("*") ? IPAddress.Any : IPAddress.Parse(cIntNetwork.Host), cIntNetwork.Port);
            _gameAuthServer = new Server(intNetAddress, 10, new GameAuthProtocol());
            _gameAuthServer.Start();
            Log.Info("GameAuthServer listening at {0}:{1}.", cIntNetwork.Host.Equals("*") ? "0.0.0.0" : cIntNetwork.Host, cIntNetwork.Port);

            // Console Setup
            Console.Title = "AikaEmu AuthServer (RUNNING)";
            Log.Warn("AikaEmu Authentication Server started with success.");

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
    }
}