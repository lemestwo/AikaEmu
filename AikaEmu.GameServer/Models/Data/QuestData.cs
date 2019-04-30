using System;
using System.Collections.Generic;
using System.Linq;
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

        public QuestJson GetQuest(ushort id)
        {
            return Objects.ContainsKey(id) ? Objects[id] : null;
        }
    }
}