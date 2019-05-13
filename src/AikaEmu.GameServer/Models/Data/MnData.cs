using System.Collections.Generic;
using AikaEmu.GameServer.Models.Data.JsonModel;
using AikaEmu.Shared.Utils;

namespace AikaEmu.GameServer.Models.Data
{
    public class MnData : BaseData<string>
    {
        public MnData(string path)
        {
            JsonUtil.DeserializeFile(path, out List<MnJson> mnData);
            foreach (var mn in mnData)
                Objects.Add(mn.Id, mn.Name);
        }

        public string GetUnitName(ushort id)
        {
            return Objects.ContainsKey(id) ? Objects[id] : string.Empty;
        }
    }
}