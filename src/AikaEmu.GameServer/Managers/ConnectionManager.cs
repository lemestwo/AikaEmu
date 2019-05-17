using System.Collections.Concurrent;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Utils;

namespace AikaEmu.GameServer.Managers
{
	public class ConnectionManager : Singleton<ConnectionManager>
	{
		private readonly ConcurrentDictionary<uint, GameConnection> _connections;

		public ConnectionManager()
		{
			_connections = new ConcurrentDictionary<uint, GameConnection>();
		}

		public void Add(GameConnection connection)
		{
			_connections.TryAdd(connection.SessionId, connection);
		}

		public void Remove(uint id)
		{
			_connections.TryRemove(id, out _);
		}

		public GameConnection GetConnection(uint id)
		{
			_connections.TryGetValue(id, out var connection);
			return connection;
		}
	}
}