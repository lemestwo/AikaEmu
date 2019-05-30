using System.Collections.Generic;
using AikaEmu.GameServer.Models.Data.JsonModel;
using AikaEmu.Shared.Utils;

namespace AikaEmu.GameServer.Models.Data
{
    public class QuestData : BaseData<QuestJson>
    {
        public QuestData(string path)
        {
            JsonUtil.DeserializeFile(path, out List<QuestJson> questData);
            foreach (var quest in questData)
                Objects.Add(quest.Id, quest);
        }

        public List<QuestJson> GetQuestByNpc(ushort npcId)
        {
            var list = new List<QuestJson>();
            if (npcId <= 0) return list;

            foreach (var quest in Objects.Values)
            {
                if (quest.StartNpc == npcId) list.Add(quest);
            }

            return list;
        }
    }
}