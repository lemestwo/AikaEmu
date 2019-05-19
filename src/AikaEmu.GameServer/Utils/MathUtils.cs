using System;
using AikaEmu.GameServer.Models.Units;

namespace AikaEmu.GameServer.Utils
{
    public static class MathUtils
    {
        public static Position CalculateNextFollowPosition(float distance, Position position)
        {
            // BUG - not working properly
            var newPos = (Position) position.Clone();
            newPos.CoordX = distance * (float) Math.Cos(position.Rotation) + position.CoordX;
            newPos.CoordY = distance * (float) Math.Sin(position.Rotation) + position.CoordY;
            return newPos;
        }

        public static bool CheckInRange(Position a, Position b, int dist)
        {
            return (a.CoordX - b.CoordX) * (a.CoordX - b.CoordX) + (a.CoordY - b.CoordY) * (a.CoordY - b.CoordY) < dist * dist;
        }

        public static float AbosoluteDistance(float a, float b)
        {
            return a > b ? (a - b) * 2 : b - a;
        }
        
        
    }
}