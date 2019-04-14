using AikaEmu.GameServer.Models.Unit;

namespace AikaEmu.GameServer.Models.Base
{
    public abstract class BaseUnit
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public Position Position { get; set; }
        public BodyTemplate BodyTemplate { get; set; }
        public int MaxHp { get; set; }
        public int Hp { get; set; }
        public int MaxMp { get; set; }
        public int Mp { get; set; }

        public virtual void SetPosition(float x, float y)
        {
            Position.CoordX = x;
            Position.CoordY = y;
        }

        public virtual void SetPosition(byte world, float x, float y)
        {
            Position = new Position
            {
                WorldId = world,
                CoordX = x,
                CoordY = y
            };
        }
    }
}