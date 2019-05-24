using System.Collections.Generic;
using AikaEmu.GameServer.Models.Data.SqlModel;
using MySql.Data.MySqlClient;

namespace AikaEmu.GameServer.Models.Data
{
    public class ConvertCoreData : BaseData<ConvertCoreDataModel>
    {
        public ConvertCoreData(MySqlConnection connection)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM `data_gearconverts`";
                command.Prepare();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var cores = new ConvertCoreDataModel
                        {
                            Id = reader.GetUInt16("id"),
                            ResultItemId = reader.GetUInt16("result_item_id"),
                            ItemId = new List<ushort>(),
                            GearLevel = reader.GetByte("gear_level"),
                            Chance = reader.GetByte("chance"),
                            ExtChance = reader.GetByte("ext_chance"),
                            ConcExtChance = reader.GetByte("conc_ext_chance")
                        };
                        for (var i = 1; i <= 5; i++)
                            cores.ItemId.Add(reader.GetUInt16("item" + i + "_id"));

                        Objects.Add(cores.Id, cores);
                    }
                }
            }
        }

        public ConvertCoreDataModel GetData(ushort itemId, ushort gearLevel)
        {
            foreach (var core in Objects.Values)
                if (core.GearLevel == gearLevel && core.ItemId.Contains(itemId))
                    return core;

            return null;
        }
    }
}