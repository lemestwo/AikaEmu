using System.Collections.Generic;

namespace AikaEmu.GameServer.Models.Data.SqlModel
{
    public class MakeItemDataModel
    {
        public ushort ResultItemId { get; set; }
        public ushort ResultSupItemId { get; set; }
        public byte Level { get; set; }
        public ulong Price { get; set; }
        public ushort Quantity { get; set; }
        public uint Rate { get; set; }
        public uint RateSup { get; set; }
        public uint RateDouble { get; set; }
        public List<MakeItemIngredients> Ingredients { get; set; }
    }

    public class MakeItemIngredients
    {
        public ushort ItemId { get; set; }
        public byte Quantity { get; set; }
    }
}