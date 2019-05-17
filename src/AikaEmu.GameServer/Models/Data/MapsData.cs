using System.Collections.Generic;
using AikaEmu.GameServer.Models.Data.SqlModel;
using AikaEmu.GameServer.Models.Data.SqlModel.Const;
using MySql.Data.MySqlClient;

namespace AikaEmu.GameServer.Models.Data
{
    public class MapsData : BaseData<MapDataModel>
    {
        public MapsData(MySqlConnection connection)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM `data_maps`";
                command.Prepare();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var map = new MapDataModel
                        {
                            Id = reader.GetUInt16("id"),
                            Name = reader.GetString("name"),
                            X1 = reader.GetUInt16("x1"),
                            Y1 = reader.GetUInt16("y1"),
                            X2 = reader.GetUInt16("x2"),
                            Y2 = reader.GetUInt16("y2"),
                            Unk1 = reader.GetUInt16("unk1"),
                            Unk2 = reader.GetUInt16("unk2"),
                            Unk3 = reader.GetUInt16("unk3"),
                            Unk4 = reader.GetUInt16("unk4"),
                            Regions = new Dictionary<ushort, RegionDataModel>()
                        };

                        Objects.Add(map.Id, map);
                    }
                }
            }

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM `data_map_regions`";
                command.Prepare();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var mapId = reader.GetUInt16("map_id");
                        if (!Objects.ContainsKey(mapId)) continue;

                        var id = reader.GetUInt16("id");
                        var region = new RegionDataModel
                        {
                            TpX = reader.GetUInt32("x"),
                            TpY = reader.GetUInt32("y"),
                            Location = reader.GetUInt16("location"),
                            TpLevel = (TpLevel) reader.GetUInt16("tp_level"),
                            Unk = reader.GetUInt16("unk1"),
                            Name = reader.GetString("region_name")
                        };
                        Objects[mapId].Regions.Add(id, region);
                    }
                }
            }
        }

        public RegionDataModel GetTeleportPosition(ushort regionId, TpLevel tpLevel)
        {
            foreach (var maps in Objects.Values)
            {
                if (maps.Regions.ContainsKey(regionId) && maps.Regions[regionId].TpLevel <= tpLevel)
                {
                    return maps.Regions[regionId];
                }
            }

            return null;
        }
    }
}