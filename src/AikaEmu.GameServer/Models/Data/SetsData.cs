using System.Collections.Generic;
using AikaEmu.GameServer.Models.Data.SqlModel;
using MySql.Data.MySqlClient;

namespace AikaEmu.GameServer.Models.Data
{
    public class SetsData : BaseData<SetDataModel>
    {
        public SetsData(MySqlConnection connection)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM `data_sets`";
                command.Prepare();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var set = new SetDataModel
                        {
                            Id = reader.GetUInt16("id"),
                            Name = reader.GetString("name"),
                            Name2 = reader.GetString("name2"),
                            Unk = reader.GetUInt16("unk"),
                            Items = new HashSet<ushort>(),
                            Effects = new List<SetEffectData>()
                        };
                        Objects.Add(set.Id, set);
                    }
                }
            }

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM `data_set_items`";
                command.Prepare();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var id = reader.GetUInt16("id");
                        var itemId = reader.GetUInt16("item_id");
                        if (Objects.ContainsKey(id))
                            Objects[id].Items.Add(itemId);
                    }
                }
            }

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM `data_set_effects`";
                command.Prepare();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var id = reader.GetUInt16("id");
                        if (!Objects.ContainsKey(id)) continue;

                        var effect = new SetEffectData
                        {
                            Count = reader.GetByte("count"),
                            IsSkill = reader.GetBoolean("is_skill"),
                            Chance = reader.GetByte("chance"),
                            EffectId = reader.GetUInt16("eff_id"),
                            EffectValue = reader.GetUInt16("eff_value")
                        };
                        Objects[id].Effects.Add(effect);
                    }
                }
            }
        }
    }
}