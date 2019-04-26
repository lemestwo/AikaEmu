using System;
using AikaEmu.GameServer.Models.Unit;

namespace AikaEmu.GameServer.Utils
{
    public static class MathUtils
    {
        public static Position CalculateNextFollowPosition(float distance, Position position)
        {
            var newPos = (Position) position.Clone();
            newPos.CoordX = distance * (float) Math.Cos(position.Rotation) + position.CoordX;
            newPos.CoordY = distance * (float) Math.Sin(position.Rotation) + position.CoordY;
            return newPos;
        }
    }
}