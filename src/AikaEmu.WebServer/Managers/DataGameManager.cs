using System;
using System.Text;
using AikaEmu.Shared.Model;

namespace AikaEmu.WebServer.Managers
{
    public class DataGameManager : Database<DataGameManager>
    {
        public string GetCharactersInfo(string user, string hash)
        {
            var characters = 0;
            var nation = 0;
            var accountId = DataAuthManager.Instance.GetAccountId(user, hash);
            if (accountId > 0)
            {
                using (var connection = GetConnection())
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT COUNT(*) FROM `characters` WHERE `acc_id` = @acc_id; ";
                        command.Parameters.AddWithValue("@acc_id", accountId);
                        command.Prepare();
                        characters = int.Parse(command.ExecuteScalar().ToString());
                    }

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT * FROM `account_nation` WHERE `acc_id` = @acc_id; ";
                        command.Parameters.AddWithValue("@acc_id", accountId);
                        command.Prepare();
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                                nation = reader.GetByte("nation_id");
                        }
                    }
                }
            }

            var infos = new StringBuilder();
            infos.AppendLine("CNT " + characters + " 0 0 0");
            infos.AppendLine("<br>");
            infos.AppendLine(nation + " 0 0 0");
            return infos.ToString();
        }
    }
}