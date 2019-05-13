using System.Collections.Generic;

namespace AikaEmu.GameServer.Models.Data.Mobs
{
    public class MobPosJson
    {
        public ushort MobId { get; set; }
        public List<MobPosition> Position { get; set; }
    }

    public class MobPosition
    {
        public ushort CoordX { get; set; }
        public ushort CoordY { get; set; }
    }
}