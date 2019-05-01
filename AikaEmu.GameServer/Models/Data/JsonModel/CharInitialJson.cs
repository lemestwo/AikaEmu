using System.Collections.Generic;

namespace AikaEmu.GameServer.Models.Data.JsonModel
{
    public class CharInitialJson
    {
        public uint MaxGold { get; set; }
        public byte MaxLevel { get; set; }
        public byte ItemStack { get; set; }
        public string NameRegex { get; set; }
        public StartPosition[] StartPosition { get; set; }
        public List<Classes> Classes { get; set; }
    }

    public class Classes
    {
        public ushort Class { get; set; }
        public byte[] Body { get; set; }
        public ushort[] Items { get; set; }
        public ushort[] Attributes { get; set; }
        public int[] HpMp { get; set; }
    }

    public class StartPosition
    {
        public float CoordX { get; set; }
        public float CoordY { get; set; }
    }
}