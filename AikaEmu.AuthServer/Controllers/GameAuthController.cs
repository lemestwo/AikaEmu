using System.Collections.Generic;
using AikaEmu.AuthServer.Models;
using AikaEmu.AuthServer.Network.GameServer;
using AikaEmu.AuthServer.Packets;
using AikaEmu.AuthServer.Packets.Auth;
using AikaEmu.Shared.Utils;
using NLog;

namespace AikaEmu.AuthServer.Controllers
{
    public class GameAuthController : Singleton<GameAuthController>
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();
        private readonly Dictionary<byte, GameServer> _gameServers;

        protected GameAuthController()
        {
            _gameServers = new Dictionary<byte, GameServer>();
        }

        public void Init()
        {
            using (var con = AuthServer.Instance.DatabaseManager.GetConnection())
            {
                using (var query = con.CreateCommand())
                {
                    query.CommandText = "SELECT * FROM game_servers";
                    query.Prepare();
                    using (var reader = query.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var temp = new GameServer
                            {
                                Id = reader.GetByte("id"),
                                Name = reader.GetString("name"),
                                Ip = reader.GetString("ip"),
                                Port = reader.GetInt16("port")
                            };
                            _gameServers.Add(temp.Id, temp);
                        }
                    }
                }
            }

            _log.Info("GameServers loaded: {0}", _gameServers.Count);
        }

        public void Add(byte gsId, GameAuthConnection connection)
        {
            if (!_gameServers.ContainsKey(gsId))
            {
                connection.SendPacket(new RegisterGSResult(false));
                return;
            }

            var gameServer = _gameServers[gsId];
            gameServer.Connection = connection;
            connection.GameServer = gameServer;
            connection.AddAttribute("gsId", gameServer.Id);
            gameServer.Connection.SendPacket(new RegisterGSResult(true));

            _log.Info("GameServer {0} registered with success.", gameServer.Id);
        }

        public void Remove(byte gsId)
        {
            if (!_gameServers.ContainsKey(gsId))
                return;

            var gameServer = _gameServers[gsId];
            gameServer.Connection = null;
        }
    }
}