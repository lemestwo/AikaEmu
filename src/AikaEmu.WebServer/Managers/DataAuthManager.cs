using System;
using System.Collections.Generic;
using AikaEmu.Shared.Model;
using AikaEmu.WebServer.Utils;

namespace AikaEmu.WebServer.Managers
{
    public class DataAuthManager : Database<DataAuthManager>
    {
        public string AuthAndUpdateAccount(string user, string pass)
        {
            using (var connection = GetConnection())
            {
                uint accountId;
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM `accounts` WHERE `user` = @user AND `pass` = @pass; ";
                    command.Parameters.AddWithValue("@user", user);
                    command.Parameters.AddWithValue("@pass", pass);
                    command.Prepare();
                    using (var reader = command.ExecuteReader())
                    {
                        // Wrong user or pass
                        if (!reader.Read()) return "-1";

                        // TODO - FIND BLOCK ID
                        var isBlock = reader.GetByte("is_block");
                        if (isBlock > 0) return "-1";

                        // Hash session still up
                        var lastHash = reader.GetString("session_hash");
                        if (lastHash != string.Empty)
                        {
                            var lastSession = reader.GetDateTime("session_time");
                            if (lastSession > DateTime.UtcNow) return lastHash;
                        }

                        accountId = reader.GetUInt32("id");
                    }
                }

                if (accountId > 0)
                {
                    var newHash = WebUtils.GenerateRandomHash();
                    var parameters = new Dictionary<string, object>
                    {
                        {"session_hash", newHash}, {"session_time", DateTime.UtcNow.AddMinutes(1)},
                    };
                    var wheres = new Dictionary<string, object>
                    {
                        {"id", accountId}
                    };
                    if (MySqlCommand("accounts", parameters, wheres, connection) > 0) return newHash;
                }
            }

            return "-1";
        }
    }
}