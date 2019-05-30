using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using AikaEmu.GameServer.Managers;
using AikaEmu.GameServer.Models.Chat;
using AikaEmu.GameServer.Models.Item;
using AikaEmu.GameServer.Models.Item.Const;
using AikaEmu.GameServer.Models.Units.Character.Const;
using AikaEmu.GameServer.Network.Packets.Game;
using AikaEmu.GameServer.Utils;
using AikaEmu.Shared.Model;
using MySql.Data.MySqlClient;
using NLog;

namespace AikaEmu.GameServer.Models.Units.Character
{
    public enum SlotType : byte
    {
        Equipments = 0,
        Inventory = 1,
        Bank = 2,

        PranEquipments = 5,
        PranInventory = 6,

        //        Unk7 = 7,
        //        Unk10 = 10, // 0xA
    }

    public class Inventory
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();
        private readonly Character _character;
        private readonly Dictionary<SlotType, Item.Item[]> _items;
        private readonly List<uint> _removedItems;
        private readonly object _lockObject = new object();

        public Inventory(Character character)
        {
            _character = character;
            _removedItems = new List<uint>();

            _items = new Dictionary<SlotType, Item.Item[]>();
            foreach (SlotType type in Enum.GetValues(typeof(SlotType)))
            {
                switch (type)
                {
                    case SlotType.Equipments:
                        _items.Add(type, new Item.Item[16]);
                        break;
                    case SlotType.Inventory:
                        _items.Add(type, new Item.Item[84]);
                        break;
                    case SlotType.Bank:
                        _items.Add(type, new Item.Item[86]);
                        break;
                    case SlotType.PranEquipments:
                        _items.Add(type, new Item.Item[16]);
                        break;
                    case SlotType.PranInventory:
                        _items.Add(type, new Item.Item[42]);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public void MoveMoney(SlotType slotType, long amount)
        {
            // TODO - Guild bank use same function?
            if (slotType != SlotType.Bank) return;
            var maxMoney = DataManager.Instance.CharacterData.Data.MaxGold;

            var isDeposit = amount > 0;
            if (isDeposit && _character.BankMoney + (ulong) amount > maxMoney)
            {
                _character.SendPacket(new SendMessage(new Message($"Can't have more than {maxMoney:#.0} gold.")));
                return;
            }

            if (!isDeposit && _character.Money + (ulong) (amount * -1) > maxMoney)
            {
                _character.SendPacket(new SendMessage(new Message($"Can't have more than {maxMoney:#.0} gold.")));
                return;
            }

            if (isDeposit && _character.Money < (ulong) amount)
            {
                _character.SendPacket(new SendMessage(new Message($"You don't have that much money.")));
                return;
            }

            if (!isDeposit && _character.BankMoney < (ulong) (amount * -1))
            {
                _character.SendPacket(new SendMessage(new Message($"You don't have that much money.")));
                return;
            }

            lock (_lockObject)
            {
                _character.Money -= (ulong) amount;
                _character.BankMoney += (ulong) amount;

                _character.SendPacket(new UpdateCharGold(_character));
                _character.Save(SaveType.OnlyCharacter);
            }
        }

        public bool UpdateItem(SlotType slotType, ushort slot, ushort newItemId, bool save = true)
        {
            var item = GetItem(slotType, slot);
            if (item == null) return false;
            lock (_lockObject)
            {
                item.ItemId = newItemId;
                _items[slotType][slot] = item;
                if (save)
                    _character.Save(SaveType.Inventory);
            }

            _character.SendPacket(new UpdateItem(item, false));
            return true;
        }
        
        public bool UpdateItem(SlotType slotType, ushort slot, byte newQuantity, bool save = true)
        {
            var item = GetItem(slotType, slot);
            if (item == null) return false;
            lock (_lockObject)
            {
                item.Quantity = newQuantity;
                _items[slotType][slot] = item;
                if (save)
                    _character.Save(SaveType.Inventory);
            }

            _character.SendPacket(new UpdateItem(item, false));
            return true;
        }

        public void MergeItems(ushort slotFrom, ushort slotTo)
        {
            lock (_lockObject)
            {
                var itemFrom = GetItem(SlotType.Inventory, slotFrom);
                var itemTo = GetItem(SlotType.Inventory, slotTo);
                if (itemFrom == null || itemTo == null || !itemFrom.ItemId.Equals(itemTo.ItemId) || !itemFrom.ItemData.IsStackable ||
                    !itemTo.ItemData.IsStackable) return;

                if (itemFrom.Quantity + itemTo.Quantity > DataManager.Instance.CharacterData.Data.ItemStack)
                {
                    SwapItems(SlotType.Inventory, SlotType.Inventory, slotFrom, slotTo);
                    return;
                }

                itemTo.Quantity += itemFrom.Quantity;
                RemoveItem(SlotType.Inventory, slotFrom, 0, false);
                _items[itemTo.SlotType][itemTo.Slot] = itemTo;
                _character.SendPacket(new UpdateItem(itemTo, false));

                _character.Save(SaveType.Inventory);
            }
        }

        public void SplitItem(SlotType slotType, ushort slot, uint quantity)
        {
            if (quantity >= DataManager.Instance.CharacterData.Data.ItemStack ||
                slotType != SlotType.Inventory && slotType != SlotType.Bank && slotType != SlotType.PranInventory) return;

            lock (_lockObject)
            {
                var item = GetItem(slotType, slot);
                if (!item.ItemData.IsStackable || item.Quantity <= quantity) return;

                if (AddItem(slotType, quantity, item.ItemId, false))
                {
                    item.Quantity -= (byte) quantity;
                    _items[item.SlotType][item.Slot] = item;
                    _character.SendPacket(new UpdateItem(item, false));
                }

                _character.Save(SaveType.Inventory);
            }
        }

        public void SendBankData()
        {
            var items = GetItemsBySlotType(SlotType.Bank);
            _character.SendPacket(new UpdateBank(_character.BankMoney, items, 0));
        }

        public bool RemoveItem(SlotType slotType, ushort slot, uint qty = 0, bool save = true)
        {
            lock (_lockObject)
            {
                if (slotType != SlotType.Inventory && slotType != SlotType.Bank && slotType != SlotType.PranInventory) return false;

                var item = GetItem(slotType, slot);
                if (item == null) return false;

                if (qty == 0 || qty >= item.Quantity)
                {
                    _removedItems.Add(item.DbId);
                    _items[slotType][slot] = null;
                    _character.SendPacket(new UpdateItem(new Item.Item(slotType, slot, 0, false), false));
                }
                else
                {
                    item.Quantity -= (byte) (qty & 0xFF);
                    _items[slotType][slot] = item;
                    _character.SendPacket(new UpdateItem(item, false));
                }

                if (save)
                    _character.Save(SaveType.Inventory);

                return true;
            }
        }

        public bool RemoveItems(SlotType slotType, ushort itemId, uint qty, bool save)
        {
            lock (_lockObject)
            {
                var (items, itemQty) = GetItems(slotType, itemId);
                if (itemQty < qty) return false;

                foreach (var item in items)
                {
                    if (qty == 0) continue;

                    RemoveItem(slotType, item.Slot, qty, save);
                    qty = item.Quantity <= qty ? qty - item.Quantity : 0;
                }

                return true;
            }
        }

        public bool RemoveItems(SlotType slotType, Dictionary<ushort, uint> items, bool save)
        {
            foreach (var (itemId, qty) in items)
            {
                if (!RemoveItems(slotType, itemId, qty, save)) return false;
            }

            return true;
        }

        public (List<Item.Item> items, uint count) GetItems(SlotType slotType, ushort itemId)
        {
            lock (_lockObject)
            {
                var list = _items[slotType].Where(item => item?.ItemId == itemId).ToList();

                var count = 0u;
                foreach (var item in list)
                    count += item.Quantity;

                return (list, count);
            }
        }

        public ConcurrentDictionary<ushort, Item.Item> GetItemsBySlotType(SlotType slot)
        {
            lock (_lockObject)
            {
                var list = new ConcurrentDictionary<ushort, Item.Item>();

                foreach (var item in _items[slot])
                    if (item?.ItemId > 0)
                        list.TryAdd(item.Slot, item);

                return list;
            }
        }

        public Item.Item GetItem(SlotType slotType, ushort slot)
        {
            lock (_lockObject)
            {
                foreach (var item in _items[slotType])
                {
                    if (item?.Slot == slot) return item;
                }

                return null;
            }
        }

        public bool AddItem(SlotType slotType, uint quantity, ushort itemId, bool isNotice = true)
        {
            lock (_lockObject)
            {
                var newItems = new List<Item.Item>();
                var freeSlots = GetFreeSlots(slotType);
                var itemData = DataManager.Instance.ItemsData.GetData(itemId);
                if (itemData == null) return false;

                // If item is stackable
                if (itemData.IsStackable)
                {
                    var maxStack = DataManager.Instance.CharacterData.Data.ItemStack;
                    var stacks = Math.DivRem(quantity, maxStack, out var remainder);
                    if (remainder != 0) stacks++;
                    if (stacks <= 0) return false;

                    // TODO - MSG ERROR NOT ENOUGH SPACE
                    if (freeSlots < stacks) return false;
                    for (var i = 0; i < stacks; i++)
                    {
                        var freeSlot = GetFirstFreeSlot(slotType);
                        var item = new Item.Item(slotType, freeSlot, itemId)
                        {
                            Quantity = (byte) (quantity > maxStack ? maxStack : quantity),
                            Durability = (byte) itemData.Durability,
                            DurMax = (byte) itemData.Durability,
                        };
                        quantity -= item.Quantity;
                        newItems.Add(item);
                        _items[slotType][freeSlot] = item;
                    }
                }
                else if (GlobalUtils.IsEquipment(itemData.ItemType))
                {
                    var freeSlot = GetFirstFreeSlot(slotType);
                    var item = new Item.Item(slotType, freeSlot, itemId)
                    {
                        // TODO - FIX THIS WHEN NOT DEVELOP
                        // POSSIBLE EXPLOIT
                        Quantity = (byte) quantity,
                        Durability = (byte) itemData.Durability,
                        DurMax = (byte) itemData.Durability,
                    };
                    newItems.Add(item);
                    _items[slotType][freeSlot] = item;
                }
                else
                {
                    // If item don't stack
                    if (freeSlots < quantity) return false;

                    for (var i = 0; i < quantity; i++)
                    {
                        var freeSlot = GetFirstFreeSlot(slotType);
                        var item = new Item.Item(slotType, freeSlot, itemId)
                        {
                            Quantity = 1,
                            Durability = (byte) itemData.Durability,
                            DurMax = (byte) itemData.Durability,
                        };
                        newItems.Add(item);
                        _items[slotType][freeSlot] = item;
                    }
                }

                newItems = DatabaseManager.Instance.InsertItems(newItems, _character);
                foreach (var newItem in newItems)
                {
                    _items[newItem.SlotType][newItem.Slot] = newItem;
                    _character.SendPacket(new UpdateItem(newItem, isNotice));
                }

                return true;
            }
        }

        private ushort GetFirstFreeSlot(SlotType slotType)
        {
            lock (_lockObject)
            {
                var bags = GetBags(slotType);
                for (ushort i = 0; i < bags * 20; i++)
                {
                    if (_items[slotType][i] == null || _items[slotType][i].ItemId <= 0) return i;
                }

                return 0;
            }
        }

        public int GetFreeSlots(SlotType slotType)
        {
            lock (_lockObject)
            {
                var i = 0;
                if (slotType == SlotType.Inventory || slotType == SlotType.Bank || slotType == SlotType.PranInventory)
                {
                    var bags = GetBags(slotType);
                    for (ushort j = 0; j < bags * 20; j++)
                        if (_items[slotType][j] == null || _items[slotType][j].ItemId <= 0)
                            i++;
                }

                return i;
            }
        }

        private int GetBags(SlotType slotType)
        {
            lock (_lockObject)
            {
                var bags = 0;
                if (slotType == SlotType.Inventory || slotType == SlotType.Bank || slotType == SlotType.PranInventory)
                {
                    for (var i = 0; i < 4; i++)
                        if (_items[slotType][80 + i] != null && _items[slotType][80 + i].ItemData.ItemType == ItemType.Bag)
                            bags++;
                }

                return bags;
            }
        }

        public void UseItem(SlotType slotType, ushort slot, int data)
        {
            lock (_lockObject)
            {
                var item = GetItem(slotType, slot);
                if (item == null) return;

                var itemType = item.ItemData.ItemType;
                if (itemType <= 0) return;

                var type = Type.GetType("AikaEmu.GameServer.Models.Item.UseItem." + itemType);
                if (type == null)
                {
                    _log.Error("UseItem {0} not implemented.", itemType);
                    return;
                }

                var useItem = (IUseItem) Activator.CreateInstance(type);
                useItem.Execute(_character, item, data);
            }
        }

        public void SwapItems(SlotType typeFrom, SlotType typeTo, ushort slotFrom, ushort slotTo)
        {
            lock (_lockObject)
            {
                var item1 = GetItem(typeFrom, slotFrom);
                var item2 = GetItem(typeTo, slotTo);

                // TODO - Check if can swap
                _items[typeFrom][slotFrom] = item2;
                _items[typeTo][slotTo] = item1;
                if (item1 != null)
                {
                    _items[typeTo][slotTo].Slot = slotTo;
                    _items[typeTo][slotTo].SlotType = typeTo;
                    _character.SendPacket(new UpdateItem(_items[typeTo][slotTo], false));
                }
                else
                {
                    _character.SendPacket(new UpdateItem(new Item.Item(typeTo, slotTo, 0, false), false));
                }

                if (item2 != null)
                {
                    _items[typeFrom][slotFrom].Slot = slotFrom;
                    _items[typeFrom][slotFrom].SlotType = typeFrom;
                    _character.SendPacket(new UpdateItem(_items[typeFrom][slotFrom], false));
                }
                else
                {
                    _character.SendPacket(new UpdateItem(new Item.Item(typeFrom, slotFrom, 0, false), false));
                }

                _character.Save(SaveType.Inventory);
            }
        }

        public void Init(MySqlConnection connection, SlotType slot)
        {
            using (var command = connection.CreateCommand())
            {
                switch (slot)
                {
                    case SlotType.Inventory:
                    case SlotType.Equipments:
                        command.CommandText = "SELECT * FROM items WHERE acc_id=@acc_id AND char_id=@char_id";
                        command.Parameters.AddWithValue("@acc_id", _character.Account.DbId);
                        command.Parameters.AddWithValue("@char_id", _character.DbId);
                        break;
                    case SlotType.Bank:
                        command.CommandText = "SELECT * FROM items WHERE acc_id=@acc_id AND char_id=0";
                        command.Parameters.AddWithValue("@acc_id", _character.Account.DbId);
                        break;
                    case SlotType.PranInventory:
                    case SlotType.PranEquipments:
                        command.CommandText = "SELECT * FROM items WHERE acc_id=@acc_id AND pran_id=@pran_id";
                        command.Parameters.AddWithValue("@acc_id", _character.Account.DbId);
                        command.Parameters.AddWithValue("@pran_id", _character.ActivePran.DbId);
                        break;
                }

                command.Prepare();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var item = new Item.Item(reader.GetUInt16("item_id"))
                        {
                            DbId = reader.GetUInt32("id"),
                            AccId = reader.GetUInt32("acc_id"),
                            CharId = reader.GetUInt32("char_id"),
                            PranId = reader.GetUInt32("pran_id"),
                            SlotType = (SlotType) reader.GetByte("slot_type"),
                            Slot = reader.GetUInt16("slot"),
                            Effect1 = reader.GetByte("effect1"),
                            Effect2 = reader.GetByte("effect2"),
                            Effect3 = reader.GetByte("effect3"),
                            Effect1Value = reader.GetByte("effect1value"),
                            Effect2Value = reader.GetByte("effect2value"),
                            Effect3Value = reader.GetByte("effect3value"),
                            Durability = reader.GetByte("dur"),
                            DurMax = reader.GetByte("dur_max"),
                            Quantity = reader.GetByte("quantity"),
                            ItemTime = reader.GetUInt16("time")
                        };

                        // Check if item exists json data
                        if (item.ItemData == null) continue;

                        // Check if in-range of the array
                        if (item.SlotType == SlotType.Equipments && item.Slot < 16 ||
                            item.SlotType == SlotType.Inventory && item.Slot < 84 ||
                            item.SlotType == SlotType.Bank && item.Slot < 86 ||
                            item.SlotType == SlotType.PranInventory && item.Slot < 42 ||
                            item.SlotType == SlotType.PranEquipments && item.Slot < 16)
                        {
                            _items[item.SlotType][item.Slot] = item;
                        }
                    }
                }
            }
        }

        public void Save(MySqlConnection connection, MySqlTransaction transaction)
        {
            if (_removedItems.Count > 0)
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM items WHERE acc_id=@acc_id AND id IN(" + string.Join(",", _removedItems) + ")";
                    command.Prepare();
                    command.Parameters.AddWithValue("@acc_id", _character.Account.DbId);
                    command.ExecuteNonQuery();
                }

                _removedItems.Clear();
            }

            using (var command = connection.CreateCommand())
            {
                command.Connection = connection;
                command.Transaction = transaction;

                foreach (var items in _items.Values)
                {
                    foreach (var item in items)
                    {
                        if (item == null || item.ItemId == 0) continue;

                        var parameters = new Dictionary<string, object>
                        {
                            {"id", item.DbId},
                            {"item_id", item.ItemId},
                            {"char_id", item.SlotType == SlotType.Inventory || item.SlotType == SlotType.Equipments ? _character.DbId : 0},
                            {"acc_id", _character.Account.DbId},
                            {
                                "pran_id",
                                item.SlotType == SlotType.PranInventory || item.SlotType == SlotType.PranEquipments ? _character.ActivePran?.DbId ?? 0 : 0
                            },
                            {"slot_type", (byte) item.SlotType},
                            {"slot", item.Slot},
                            {"effect1", item.Effect1},
                            {"effect2", item.Effect2},
                            {"effect3", item.Effect3},
                            {"effect1value", item.Effect1Value},
                            {"effect2value", item.Effect2Value},
                            {"effect3value", item.Effect3Value},
                            {"dur", item.Durability},
                            {"dur_max", item.DurMax},
                            {"quantity", item.Quantity},
                            {"time", item.ItemTime}
                        };
                        DatabaseManager.Instance.MySqlCommand(SqlCommandType.Replace, "items", parameters, connection, transaction);
                    }
                }
            }
        }
    }
}