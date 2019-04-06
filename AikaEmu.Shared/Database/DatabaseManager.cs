using System;
using MySql.Data.MySqlClient;
using NLog;

namespace AikaEmu.Shared.Database
{
    public class DatabaseManager
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();
        private string _connectionString;

        public void Initialize(string host, string user, string pass, string db, string port)
        {
            _connectionString =
                $"server={host}; port={port}; database={db}; uid={user}; password={pass}; charset=utf8; pooling=true; min pool size=0; max pool size=100;";
            using (var sql = GetConnection())
            {
                sql.Close();
            }
        }

        public MySqlConnection GetConnection()
        {
            var connection = new MySqlConnection(_connectionString);
            try
            {
                connection.Open();
            }
            catch (Exception e)
            {
                _log.Error(e.Message);
                return null;
            }

            return connection;
        }
    }
}