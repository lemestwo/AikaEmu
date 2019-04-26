using System;
using System.Collections.Generic;
using AikaEmu.GameServer.Managers;

namespace AikaEmu.GameServer.Models.Unit
{
    public abstract class BaseUnit
    {
        public uint Id { get; set; }
        public ushort Level { get; set; }
        public string Name { get; set; }
        public Position Position { get; set; }
        public BodyTemplate BodyTemplate { get; set; }
        public int MaxHp { get; set; }
        public int Hp { get; set; }
        public int MaxMp { get; set; }
        public int Mp { get; set; }
        public bool isActive { get; set; } = false;
        public Dictionary<uint, BaseUnit> VisibleUnits { get; set; } = new Dictionary<uint, BaseUnit>();

        public virtual void SetPosition(float x, float y)
        {
            Position.CoordX = x;
            Position.CoordY = y;

            WorldManager.Instance.ShowVisibleUnits(this);
        }

        public virtual void SetPosition(byte world, float x, float y)
        {
            Position = new Position
            {
                NationId = world,
                CoordX = x,
                CoordY = y,
                Rotation = 0
            };

            WorldManager.Instance.ShowVisibleUnits(this);
        }

        public virtual void SetPosition(Position pos)
        {
            Position = pos;
            WorldManager.Instance.ShowVisibleUnits(this);
        }

        public virtual void SetRotation(int rotation)
        {
            Position.Rotation = rotation;
            WorldManager.Instance.ShowVisibleUnits(this);
        }

        public virtual void Spawn()
        {
            isActive = true;
            WorldManager.Instance.Spawn(this);
        }

        public bool IsAround(Position pos, int distance)
        {
            return Math.Pow(Position.CoordX - pos.CoordX, 2) + Math.Pow(Position.CoordY - pos.CoordY, 2) <= Math.Pow(distance, 2);
        }

        public float AbosoluteDistance(float a, float b)
        {
            return a > b ? (a - b) * 2 : b - a;
        }
    }
}