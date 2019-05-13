namespace AikaEmu.GameServer.Models.Data.JsonModel
{
    public class SPositionJson
    {
        public ushort LoopId { get; set; }
        public int Unk1 { get; set; }
        public int[] Coord { get; set; }
        public short Map { get; set; }
        public short Location { get; set; }
        public TpLevel TpLevel { get; set; }
        public string MapName { get; set; }
        public string RegionName { get; set; }
    }
}