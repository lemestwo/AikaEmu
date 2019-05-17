using System;
using System.Collections.Generic;
using AikaEmu.GameServer.Managers;

namespace AikaEmu.GameServer.Models.Units
{
    public abstract class BaseUnit
    {
        public ushort Id { get; set; }
        public ushort Level { get; set; }
        public string Name { get; set; }
        public Position Position { get; set; }
        public BodyTemplate BodyTemplate { get; set; }
        public int MaxHp { get; set; }
        public int Hp { get; set; }
        public int MaxMp { get; set; }
        public int Mp { get; set; }
        public Dictionary<uint, BaseUnit> VisibleUnits { get; set; } = new Dictionary<uint, BaseUnit>();

        public virtual void SetPosition(float x, float y)
        {
            var pos = (Position) Position.Clone();
            pos.CoordX = x;
            pos.CoordY = y;

            SetPosition(pos);
        }

        public virtual void SetPosition(byte world, float x, float y)
        {
            var pos = new Position
            {
                NationId = world,
                CoordX = x,
                CoordY = y,
                Rotation = 0
            };

            SetPosition(pos);
        }

        public virtual void SetPosition(Position pos)
        {
            Position = pos;

            WorldManager.Instance.ShowVisibleUnits(this);
        }

        public virtual void SetRotation(int rotation)
        {
            var pos = (Position) Position.Clone();
            pos.Rotation = rotation;

            SetPosition(pos);
        }

        public virtual void Spawn()
        {
            WorldManager.Instance.Spawn(this);
        }

        public bool IsAround(Position pos, int distance)
        {
            return Math.Pow(Position.CoordX - pos.CoordX, 2) + Math.Pow(Position.CoordY - pos.CoordY, 2) <= Math.Pow(distance, 2);
        }
    }
}