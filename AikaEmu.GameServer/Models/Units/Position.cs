using System;

namespace AikaEmu.GameServer.Models.Units
{
    public class Position : ICloneable
    {
        public byte NationId { get; set; }
        public float CoordX { get; set; }
        public float CoordY { get; set; }
        public int Rotation { get; set; }

        public Position()
        {
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}