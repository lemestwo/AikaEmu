using System;
using System.Collections.Generic;
using AikaEmu.AuthServer.Models;
using AikaEmu.Shared.Model;

namespace AikaEmu.AuthServer.Managers
{
    public class DatabaseManager : Database<DatabaseManager>
    {
        public Account AuthAccount(string user, string hash)
        {
            using (var connection = GetConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM accounts WHERE user=@user";
                command.Parameters.AddWithValue("@user", user);
                command.Prepare();
                using (var reader = command.ExecuteReader())
                {
                    if (!reader.Read()) return null;

                    var template = new Account
                    {
                        Id = reader.GetUInt32("id"),
                        User = reader.GetString("user"),
                        Level = reader.GetByte("level"),
                        IsBlock = reader.GetByte("is_block"),
                        SessionHash = reader.GetString("session_hash"),
                        SessionTime = reader.GetDateTime("session_time")
                    };

                    if (template.SessionHash != hash || template.SessionTime < DateTime.UtcNow) return null;

                    return template;
                }
            }
        }

        public void UpdateAccount(uint id, Dictionary<string, object> parameters)
        {
            if (parameters.Count <= 0) return;
            
            using (var connection = GetConnection())
            {
                var wheres = new Dictionary<string, object>
                {
                    {"id", id}
                };
                MySqlCommand("accounts", parameters, wheres, connection);
            }
        }
    }
}