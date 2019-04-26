using AikaEmu.Shared.Model.Configuration;
using AikaEmu.Shared.Utils;
using MySql.Data.MySqlClient;

namespace AikaEmu.Shared.Model
{
	public class DatabaseModel<T> : Singleton<T> where T : class
	{
		private string _connectionString;

		public virtual void Init(SqlConnection config)
		{
			_connectionString =
				$"server={config.Host}; port={config.Port}; database={config.Database}; uid={config.User}; password={config.Pass}; " +
				"charset=utf8; pooling=true; min pool size=0; max pool size=100;";

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

		public virtual MySqlConnection GetConnection()
		{
			var connection = new MySqlConnection(_connectionString);
			connection.Open();
			return connection;
		}
	}
}