using System;
using System.Collections.Generic;
using System.Linq;
using AikaEmu.GameServer.Models.CharacterM;
using AikaEmu.GameServer.Models.Data.JsonModel;
using AikaEmu.GameServer.Models.ItemM;
using AikaEmu.Shared.Utils;

namespace AikaEmu.GameServer.Models.Data
{
    public class ItemsData : BaseData<ItemsJson>
    {
        public ItemsData(string path)
        {
            JsonUtil.DeserializeFile(path, out List<ItemsJson> itemData);
            foreach (var itemsList in itemData)
                Objects.Add(itemsList.LoopId, itemsList);
        }

        public ItemType GetItemSlot(ushort id)
        {
            return Objects.ContainsKey(id) ? Objects[id].ItemType : ItemType.Default;
        }

        public ItemsJson GetItemData(ushort id)
        {
            return Objects.ContainsKey(id) ? Objects[id] : null;
        }
    }
}