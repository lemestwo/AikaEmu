using AikaEmu.GameServer.Models.Data.SqlModel;
using MySql.Data.MySqlClient;
using NLog;

namespace AikaEmu.GameServer.Models.Data
{
    public class ReinforceWData : BaseData<ReinforceEquipModel>
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();

        public ReinforceWData(MySqlConnection connection)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM `data_reinforce_w`";
                command.Prepare();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var reinforce = new ReinforceEquipModel()
                        {
                            Id = reader.GetUInt16("id"),
                            ReagentQty = reader.GetUInt16("reagent_qty"),
                            Price = reader.GetUInt32("price"),
                            Chance = new ushort[16]
                        };
                        for (var i = 0; i < 16; i++)
                        {
                            reinforce.Chance[i] = reader.GetUInt16("c" + (i + 1));
                        }

                        Objects.Add(reinforce.Id, reinforce);
                    }
                }
            }
        }
    }
}