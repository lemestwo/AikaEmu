using System;
using System.Collections;
using System.Collections.Generic;
using AikaEmu.GameServer.Managers;
using AikaEmu.GameServer.Models.Unit;

namespace AikaEmu.GameServer.Models.Base
{
    public abstract class BaseUnit
    {
        public virtual uint Id { get; set; }
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
                WorldId = world,
                CoordX = x,
                CoordY = y
            };

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
    }
}