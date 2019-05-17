using System.Collections.Generic;
using AikaEmu.GameServer.Models.Data.SqlModel.Const;

namespace AikaEmu.GameServer.Models.Data.SqlModel
{
    public class MapDataModel
    {
        public ushort Id { get; set; }
        public string Name { get; set; }
        public ushort X1 { get; set; }
        public ushort Y1 { get; set; }
        public ushort X2 { get; set; }
        public ushort Y2 { get; set; }
        public ushort Unk1 { get; set; }
        public ushort Unk2 { get; set; }
        public ushort Unk3 { get; set; }
        public ushort Unk4 { get; set; }
        public Dictionary<ushort, RegionDataModel> Regions { get; set; }
    }

    public class RegionDataModel
    {
        public uint TpX { get; set; }
        public uint TpY { get; set; }
        public ushort Location { get; set; }
        public TpLevel TpLevel { get; set; }
        public ushort Unk { get; set; }
        public string Name { get; set; }
    }
}