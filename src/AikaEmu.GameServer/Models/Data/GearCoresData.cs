using AikaEmu.GameServer.Models.Data.SqlModel;
using MySql.Data.MySqlClient;

namespace AikaEmu.GameServer.Models.Data
{
    public class GearCoresData : BaseData<GearCoreDataModel>
    {
        public GearCoresData(MySqlConnection connection)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM `data_gearcores`";
                command.Prepare();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var cores = new GearCoreDataModel
                        {
                            Id = reader.GetUInt16("id"),
                            Idx = reader.GetUInt16("idx"),
                            Unk = reader.GetUInt16("unk"),
                            ItemId = reader.GetUInt16("item_id"),
                            ResultItemId = reader.GetUInt16("result_item_id"),
                            Chance = reader.GetByte("chance"),
                            ExtChance = reader.GetByte("ext_chance"),
                            ConcExtChance = reader.GetByte("conc_ext_chance")
                        };
                        Objects.Add(cores.Id, cores);
                    }
                }
            }
        }

        public GearCoreDataModel GetData(ushort itemId, ushort idx)
        {
            foreach (var core in Objects.Values)
            {
                if (core.Idx == idx && core.ItemId == itemId) return core;
            }

            return null;
        }
    }
}