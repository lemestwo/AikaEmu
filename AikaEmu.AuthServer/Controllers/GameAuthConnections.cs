using System.Collections.Concurrent;
using AikaEmu.AuthServer.Network.GameServer;
using AikaEmu.Shared.Utils;

namespace AikaEmu.AuthServer.Controllers
{
    public class GameAuthConnections: Singleton<GameAuthConnections>
    {
        private readonly ConcurrentDictionary<uint, GameAuthConnection> _connections;

        public GameAuthConnections()
        {
            _connections = new ConcurrentDictionary<uint, GameAuthConnection>();
        }

        public void Add(GameAuthConnection connection)
        {
            _connections.TryAdd(connection.SessionId, connection);
        }

        public void Remove(uint id)
        {
            _connections.TryRemove(id, out _);
        }

        public GameAuthConnection GetConnection(uint id)
        {
            _connections.TryGetValue(id, out var connection);
            return connection;
        }
    }
}