using AikaEmu.GameServer.Managers.Configuration;
using AikaEmu.Shared.Utils;
using MySql.Data.MySqlClient;

namespace AikaEmu.GameServer.Managers
{
	public class DatabaseManager : Singleton<DatabaseManager>
	{
		private readonly string _connectionString;

		public DatabaseManager()
		{
			var config = AppConfigManager.Instance.GameServerConfig.Database;
			_connectionString =
				$"server={config.Host}; port={config.Port}; database={config.Database}; uid={config.User}; password={config.Pass}; " +
				"charset=utf8; pooling=true; min pool size=0; max pool size=100;";
		}

		public void Init()
		{
			MySqlConnection connection = null;
			try
			{
				connection = GetConnection();
			}
			finally
			{
				connection?.Close();
			}
		}

		public MySqlConnection GetConnection()
		{
			var connection = new MySqlConnection(_connectionString);
			connection.Open();
			return connection;
		}
	}
}