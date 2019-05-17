using AikaEmu.GameServer.Models.Data.SqlModel;
using MySql.Data.MySqlClient;

namespace AikaEmu.GameServer.Models.Data
{
    public class ExpData : BaseData<ExpDataModel>
    {
        public ExpData(MySqlConnection connection, bool isPran = false)
        {
            using (var command = connection.CreateCommand())
            {
                var table = isPran ? "`data_pran_exp`" : "`data_exp`";
                command.CommandText = "SELECT * FROM " + table;
                command.Prepare();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var exp = new ExpDataModel()
                        {
                            Level = reader.GetByte("level"),
                            Experience = reader.GetUInt64("exp")
                        };

                        Objects.Add(exp.Level, exp);
                    }
                }
            }
        }
    }
}