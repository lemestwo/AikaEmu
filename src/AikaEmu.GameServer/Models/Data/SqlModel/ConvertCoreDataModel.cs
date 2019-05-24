using System.Collections.Generic;

namespace AikaEmu.GameServer.Models.Data.SqlModel
{
    public class ConvertCoreDataModel
    {
        public ushort Id { get; set; }
        public ushort ResultItemId { get; set; }
        public List<ushort> ItemId { get; set; }
        public byte GearLevel { get; set; }
        public byte Chance { get; set; }
        public byte ExtChance { get; set; }
        public byte ConcExtChance { get; set; }
    }
}