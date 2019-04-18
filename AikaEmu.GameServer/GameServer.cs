using System;
using System.Net;
using AikaEmu.GameServer.Managers;
using AikaEmu.GameServer.Managers.Configuration;
using AikaEmu.GameServer.Managers.Id;
using AikaEmu.GameServer.Models.Data;
using AikaEmu.GameServer.Network;
using AikaEmu.GameServer.Network.AuthServer;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared;
using AikaEmu.Shared.Database;
using AikaEmu.Shared.Network;
using AikaEmu.Shared.Network.Type;

namespace AikaEmu.GameServer
{
    public class GameServer : BaseProgram
    {
        public static readonly GameServer Instance = new GameServer();

        public DatabaseManager DatabaseManager { get; private set; }
        public GameConfig GameConfigs = new GameConfig();

        public Server GameNetServer;
        public Client AuthGameServer;
        public AuthGameConnection AuthGameConnection;

        public ushort SystemSenderMsg = 30000;
        public ushort SystemSender = 30005;

        public override void Start()
        {
            Console.Title = "AikaEmu GameServer (LOADING)";
            base.Start();

            // Basic Setup
            SetupConfig(ref GameConfigs);
            SetupDatabase(DatabaseManager = new DatabaseManager(), GameConfigs.Database);

            // Managers
            DataManager.Instance.Init();

            // IdManagers
            IdCharacterManager.Instance.Init();
            IdConnectionManager.Instance.Init();
            IdItemManager.Instance.Init();
            IdUnitSpawnManager.Instance.Init();
            IdMobSpawnManager.Instance.Init();
            
            // Spawn
            WorldManager.InitBasicSpawn();

            // Server Setup
            var cNetwork = GameConfigs.Network;
            var networkAddress = new IPEndPoint(cNetwork.Host.Equals("*") ? IPAddress.Any : IPAddress.Parse(cNetwork.Host), cNetwork.Port);
            GameNetServer = new Server(networkAddress, 10, new GameProtocol());
            GameNetServer.Start();
            Log.Info("GameServer listening at {0}:{1}.", cNetwork.Host.Equals("*") ? "0.0.0.0" : cNetwork.Host, cNetwork.Port);

            // Internal Server Setup
            var cIntNetwork = GameConfigs.AuthNetwork;
            var authNetAddress = new IPEndPoint(IPAddress.Parse(cIntNetwork.Host), cIntNetwork.Port);
            AuthGameServer = new Client(authNetAddress, new AuthGameProtocol());
            AuthGameServer.Start();
            Log.Info("AuthGameServer listening at {0}:{1}.", cIntNetwork.Host, cIntNetwork.Port);

            // Console Setup
            Console.Title = "AikaEmu GameServer (RUNNING)";
            Log.Info("GameServer started with success.");
            Console.ReadLine();
        }
    }
}