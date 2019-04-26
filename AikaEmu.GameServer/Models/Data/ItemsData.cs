using System;
using System.Collections.Generic;
using System.Linq;
using AikaEmu.GameServer.Models.Data.JsonModel;
using AikaEmu.Shared.Utils;

namespace AikaEmu.GameServer.Models.Data
{
    public class ItemsData
    {
        private readonly Dictionary<ushort, ItemsJson> _items;

        public int Count => _items.Count;

        public ItemsData(string path)
        {
            _items = new Dictionary<ushort, ItemsJson>();
            JsonUtil.DeserializeFile(path, out List<ItemsJson> itemData);
            foreach (var itemsList in itemData)
                _items.Add(itemsList.LoopId, itemsList);
        }

        public bool Exists(ushort id)
        {
            return _items.ContainsKey(id);
        }

        public ushort GetItemSlot(ushort id)
        {
            return _items.ContainsKey(id) ? _items[id].ItemType : ushort.MaxValue;
        }

        public ItemsJson GetItemData(ushort id)
        {
            return _items.ContainsKey(id) ? _items[id] : null;
        }
    }
}