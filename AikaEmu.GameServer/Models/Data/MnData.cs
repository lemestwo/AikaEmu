using System.Collections.Generic;
using AikaEmu.Shared.Utils;
using Aika_BinToJson.Models;

namespace AikaEmu.GameServer.Models.Data
{
    public class MnData
    {
        private readonly Dictionary<ushort, string> _npcs;
        public int Count => _npcs.Count;

        public MnData(string path)
        {
            _npcs = new Dictionary<ushort, string>();
            JsonUtil.DeserializeFile(path, out List<MnJson> mnData);
            foreach (var mn in mnData)
                _npcs.Add(mn.Id, mn.Name);
        }

        public string GetUnitName(ushort id)
        {
            return _npcs.ContainsKey(id) ? _npcs[id] : string.Empty;
        }
    }
}