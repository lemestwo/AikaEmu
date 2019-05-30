using System.Collections.Generic;
using AikaEmu.GameServer.Models.Units.Character;

namespace AikaEmu.GameServer.Models.Data.JsonModel
{
    public class CharacterConfigJson
    {
        public uint MaxGold { get; set; }
        public byte MaxLevel { get; set; }
        public byte ItemStack { get; set; }
        public byte MaxEnchant { get; set; }
        public string NameRegex { get; set; }
        public StartPosition[] StartPosition { get; set; }
        public List<Item> StartItems { get; set; }
        public List<Classes> Classes { get; set; }
    }

    public class Classes
    {
        public ushort Class { get; set; }
        public byte[] Body { get; set; }
        public List<Item> Items { get; set; }
        public ushort[] Attributes { get; set; }
        public int[] HpMp { get; set; }
    }

    public class Item
    {
        public SlotType SlotType { get; set; }
        public byte Slot { get; set; }
        public ushort ItemId { get; set; }
        public byte Quantity { get; set; }
        public byte Durability { get; set; }
    }

    public class StartPosition
    {
        public float X { get; set; }
        public float Y { get; set; }
    }
}