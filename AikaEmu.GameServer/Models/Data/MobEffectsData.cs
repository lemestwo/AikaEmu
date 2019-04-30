using System.Collections.Generic;
using AikaEmu.GameServer.Models.Data.JsonModel;
using AikaEmu.Shared.Utils;

namespace AikaEmu.GameServer.Models.Data
{
    public class MobEffectsData : BaseData<MobEffectsJson>
    {
        public MobEffectsData(string path)
        {
            JsonUtil.DeserializeFile(path, out List<MobEffectsJson> mobEffectsData);
            foreach (var effectsList in mobEffectsData)
                Objects.Add(effectsList.Id, effectsList);
        }

        public ushort GetFace(ushort id)
        {
            return Objects.ContainsKey(id) ? Objects[id].Face : (ushort) 0;
        }
    }
}