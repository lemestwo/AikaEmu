using System.Collections.Concurrent;
using AikaEmu.AuthServer.Network.AuthServer;
using AikaEmu.Shared.Utils;

namespace AikaEmu.AuthServer.Managers.Connections
{
    public class AuthConnectionsManager : Singleton<AuthConnectionsManager>
    {
        private readonly ConcurrentDictionary<uint, AuthConnection> _connections;

        public AuthConnectionsManager()
        {
            _connections = new ConcurrentDictionary<uint, AuthConnection>();
        }

        public void Add(AuthConnection connection)
        {
            _connections.TryAdd(connection.SessionId, connection);
        }

        public void Remove(uint id)
        {
            _connections.TryRemove(id, out _);
        }

        public AuthConnection GetConnection(uint id)
        {
            _connections.TryGetValue(id, out var connection);
            return connection;
        }
    }
}