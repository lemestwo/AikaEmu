using System.Collections.Generic;
using AikaEmu.GameServer.Models.Data.JsonModel;
using AikaEmu.Shared.Utils;

namespace AikaEmu.GameServer.Models.Data
{
    public class MobPosData
    {
        private readonly Dictionary<ushort, MobPosJson> _mobs;
        public int Count => _mobs.Count;

        public MobPosData(string path)
        {
            _mobs = new Dictionary<ushort, MobPosJson>();
            JsonUtil.DeserializeFile(path, out List<MobPosJson> mobPosData);
            foreach (var mobPos in mobPosData)
                _mobs.Add(mobPos.LoopId, mobPos);
        }
    }
}