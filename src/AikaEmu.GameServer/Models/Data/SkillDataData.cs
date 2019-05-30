using System.Collections.Generic;
using AikaEmu.GameServer.Models.Data.JsonModel;
using AikaEmu.Shared.Utils;

namespace AikaEmu.GameServer.Models.Data
{
    public class SkillDataData : BaseData<SkillDataJson>
    {
        public SkillDataData(string path)
        {
            JsonUtil.DeserializeFile(path, out List<SkillDataJson> skillData);
            foreach (var skillList in skillData)
                Objects.Add(skillList.Id, skillList);
        }
    }
}