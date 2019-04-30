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

    public class SPositionData : BaseData<SPositionJson>
    {
        public SPositionData(string path)
        {
            JsonUtil.DeserializeFile(path, out List<SPositionJson> sPositionJson);
            foreach (var pos in sPositionJson)
                Objects.Add(pos.LoopId, pos);
        }

        public (float x, float y) GetPosition(ushort id, TpLevel tpLevel)
        {
            return Objects.ContainsKey(id) && Objects[id].TpLevel <= tpLevel ? (Objects[id].Coord[0], Objects[id].Coord[1]) : (0, 0);
        }
    }
}