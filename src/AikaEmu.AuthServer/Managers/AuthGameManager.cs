using System;
using System.Collections.Generic;
using AikaEmu.AuthServer.Models;
using AikaEmu.AuthServer.Network.AuthServer;
using AikaEmu.AuthServer.Network.GameServer;
using AikaEmu.AuthServer.Packets.Game;
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

		public void Authenticate(AuthConnection connection, string user, string hash)
		{
			using (var sql = DatabaseManager.Instance.GetConnection())
			using (var command = sql.CreateCommand())
			{
				command.CommandText = "SELECT * FROM accounts WHERE user=@user";
				command.Parameters.AddWithValue("@user", user);
				command.Prepare();
				using (var reader = command.ExecuteReader())
				{
					if (!reader.Read())
					{
						connection.SendPacket(new AuthSuccess(uint.MaxValue, 0));
						connection.Close();
						return;
					}

					var template = new Account
					{
						Id = reader.GetUInt32("id"),
						User = reader.GetString("user"),
						LastIp = connection.Ip,
						LastLogin = DateTime.Now,
						Level = reader.GetByte("level"),
						SessionHash = reader.GetString("session_hash"),
						SessionTime = reader.GetDateTime("session_time")
					};

					if (template.SessionHash != hash)
					{
						connection.SendPacket(new AuthSuccess(uint.MaxValue, 0));
						connection.Close();
						return;
					}

					// TODO - GENERATE KEY TO AUTH-GAME
					var generatedKey = 1;

					connection.Account = template;
					AuthAccountsManager.Instance.Add(template, generatedKey);
					connection.SendPacket(new AuthSuccess(template.Id, generatedKey));
				}
			}
		}

		public void SaveAccount(AuthConnection connection)
		{
			// TODO
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