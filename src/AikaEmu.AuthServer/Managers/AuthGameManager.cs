using System.Collections.Generic;
using AikaEmu.AuthServer.Network.GameServer;
using AikaEmu.Shared.Utils;
using NLog;
using RegisterGs = AikaEmu.AuthServer.Packets.AG.RegisterGs;

namespace AikaEmu.AuthServer.Managers
{
	public class AuthGameManager : Singleton<AuthGameManager>
	{
		private readonly Logger _log = LogManager.GetCurrentClassLogger();
		private readonly Dictionary<byte, Models.GameServer> _gameServers;

		protected AuthGameManager()
		{
			_gameServers = new Dictionary<byte, Models.GameServer>();
		}

		public void Init()
		{
			using (var con = DatabaseManager.Instance.GetConnection())
			{
				using (var query = con.CreateCommand())
				{
					query.CommandText = "SELECT * FROM game_servers";
					query.Prepare();
					using (var reader = query.ExecuteReader())
					{
						while (reader.Read())
						{
							var temp = new Models.GameServer
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
				connection.SendPacket(new RegisterGs(false));
				return;
			}

			var gameServer = _gameServers[gsId];
			gameServer.Connection = connection;
			connection.GameServer = gameServer;
			connection.AddAttribute("gsId", gameServer.Id);
			gameServer.Connection.SendPacket(new RegisterGs(true));

			_log.Info("GameServer {0} registered with success.", gameServer.Id);
		}

		public void Remove(byte gsId)
		{
			if (!_gameServers.ContainsKey(gsId))
				return;

			var gameServer = _gameServers[gsId];
			gameServer.Connection = null;
		}

		public Models.GameServer GetGameServer(byte id)
		{
			return _gameServers.ContainsKey(id) ? _gameServers[id] : null;
		}
	}
}