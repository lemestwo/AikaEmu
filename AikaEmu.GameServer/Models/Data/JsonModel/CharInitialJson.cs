using System.Collections.Generic;

namespace AikaEmu.GameServer.Models.Data.JsonModel
{
    public class CharInitialJson
    {
        public string NameRegex { get; set; }

        public StartPosition[] StartPosition { get; set; }

        public List<Class> Classes { get; set; }
    }

    public class Class
    {
        public ushort ClassClass { get; set; }

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