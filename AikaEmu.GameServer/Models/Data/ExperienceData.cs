using System.Collections.Generic;
using AikaEmu.GameServer.Models.Data.JsonModel;
using AikaEmu.Shared.Utils;

namespace AikaEmu.GameServer.Models.Data
{
    public class ExperienceData : BaseData<ulong>
    {
        public ExperienceData(string path)
        {
            JsonUtil.DeserializeFile(path, out List<ExperienceJson> xpData);
            foreach (var xp in xpData)
                Objects.Add(xp.Level, xp.Experience);
        }

        public ulong GetXpFromLevel(byte level)
        {
            return Objects.ContainsKey(level) ? Objects[level] : 0;
        }
    }
}