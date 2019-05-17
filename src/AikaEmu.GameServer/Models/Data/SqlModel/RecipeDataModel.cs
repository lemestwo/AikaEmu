using System.Collections.Generic;

namespace AikaEmu.GameServer.Models.Data.SqlModel
{
    public class RecipeDataModel
    {
        public ushort Id { get; set; }
        public ushort ItemId { get; set; }
        public ushort ResultItemId { get; set; }
        public ushort ResultItemId2 { get; set; }
        public ulong Price { get; set; }
        public ushort Quantity { get; set; }
        public byte Level { get; set; }
        public uint Rate { get; set; }
        public uint RateSup { get; set; }
        public uint RateDouble { get; set; }
        public Dictionary<ushort, RecipeIngredients> Ingredients { get; set; }
    }

    public class RecipeIngredients
    {
        public ushort Quantity { get; set; }
        public bool IsRateItem { get; set; }
        public bool IncreasedChance { get; set; }
        public ushort[] Rate { get; set; }
    }
}