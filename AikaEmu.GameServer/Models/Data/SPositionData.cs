using System.Collections.Generic;
using AikaEmu.GameServer.Models.Data.JsonModel;
using AikaEmu.GameServer.Models.Unit;
using AikaEmu.Shared.Utils;

namespace AikaEmu.GameServer.Models.Data
{
    public enum TpLevel
    {
        BasicScroll = 0,
        Unk1 = 1,
        Unk2 = 2,
    }

    public class SPositionData
    {
        private readonly Dictionary<ushort, SPositionJson> _positions;
        public int Count => _positions.Count;

        public SPositionData(string path)
        {
            _positions = new Dictionary<ushort, SPositionJson>();
            JsonUtil.DeserializeFile(path, out List<SPositionJson> sPositionJson);
            foreach (var pos in sPositionJson)
                _positions.Add(pos.LoopId, pos);
        }

        public (float x, float y) GetPosition(ushort id, TpLevel tpLevel)
        {
            return _positions.ContainsKey(id) && _positions[id].TpLevel <= tpLevel ? (_positions[id].Coord[0], _positions[id].Coord[1]) : (0, 0);
        }
    }
}