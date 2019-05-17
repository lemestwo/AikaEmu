using System.Collections.Generic;
using AikaEmu.GameServer.Models.Data.SqlModel;
using AikaEmu.GameServer.Utils;
using MySql.Data.MySqlClient;

namespace AikaEmu.GameServer.Models.Data
{
    public class TitlesData : BaseData<TitleDataModel>
    {
        public TitlesData(MySqlConnection connection)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM `data_titles`";
                command.Prepare();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var title = new TitleDataModel
                        {
                            Id = reader.GetUInt16("id"),
                            Idx = reader.GetUInt16("idx"),
                            UnkId = reader.GetUInt16("unk_id"),
                            Requires = reader.GetUInt32("requires"),
                            Effects = new List<(ushort EffId, ushort EffValue)>(),
                            Desc = reader.GetString("desc"),
                            Unk = reader.GetUInt16("unk"),
                            Color = GlobalUtils.GetColorFromString(reader.GetString("color"))
                        };
                        for (var i = 1; i <= 3; i++)
                        {
                            var eff = (reader.GetUInt16("eff" + i), reader.GetUInt16("eff" + i + "_value"));
                            if (eff.Item1 > 0)
                                title.Effects.Add(eff);
                        }

                        Objects.Add(title.Id, title);
                    }
                }
            }
        }
    }
}