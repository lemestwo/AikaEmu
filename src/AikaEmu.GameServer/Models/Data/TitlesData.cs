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
                            Level = reader.GetByte("level"),
                            UnkId1 = reader.GetUInt16("unk_id1"),
                            UnkId2 = reader.GetUInt16("unk_id2"),
                            Requires = reader.GetUInt32("requires"),
                            Effect1 = new Effect(reader.GetUInt16("eff1"), reader.GetUInt16("eff1_value")),
                            Effect2 = new Effect(reader.GetUInt16("eff2"), reader.GetUInt16("eff2_value")),
                            Effect3 = new Effect(reader.GetUInt16("eff3"), reader.GetUInt16("eff3_value")),
                            Desc = reader.GetString("desc"),
                            Unk = reader.GetUInt16("unk"),
                            Color = GlobalUtils.GetColorFromString(reader.GetString("color"))
                        };

                        Objects.Add(title.Id, title);
                    }
                }
            }
        }

        public TitleDataModel GetTitleData(ushort id, byte level)
        {
            foreach (var title in Objects.Values)
                if (title.Idx == id && title.Level == level)
                    return title;

            return null;
        }
    }
}