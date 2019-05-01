using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using AikaEmu.GameServer.Managers;
using AikaEmu.GameServer.Managers.Configuration;
using AikaEmu.GameServer.Managers.Id;
using AikaEmu.GameServer.Models.Chat;
using AikaEmu.GameServer.Models.ItemM;
using AikaEmu.GameServer.Models.PranM;
using AikaEmu.GameServer.Network.Packets.Game;
using MySql.Data.MySqlClient;
using NLog;

namespace AikaEmu.GameServer.Models.CharacterM
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
        private readonly Dictionary<SlotType, Item[]> _items;
        private readonly List<uint> _removedItems;
        private readonly object _lockObject = new object();

        public Inventory(Character character)
        {
            _character = character;
            _removedItems = new List<uint>();

            _items = new Dictionary<SlotType, Item[]>();
            foreach (SlotType type in Enum.GetValues(typeof(SlotType)))
            {
                switch (type)
                {
                    case SlotType.Equipments:
                        _items.Add(type, new Item[16]);
                        break;
                    case SlotType.Inventory:
                        _items.Add(type, new Item[84]);
                        break;
                    case SlotType.Bank:
                        _items.Add(type, new Item[86]);
                        break;
                    case SlotType.PranEquipments:
                        _items.Add(type, new Item[16]);
                        break;
                    case SlotType.PranInventory:
                        _items.Add(type, new Item[42]);
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
            var maxMoney = DataManager.Instance.CharInitial.Data.MaxGold;
            if (amount > maxMoney || amount < maxMoney * -1 || amount == 0)
            {
                _character.SendPacket(new SendMessage(new Message(MessageSender.System, MessageType.Normal, $"Can't have more than {maxMoney} gold.")));
                return;
            }

            var isDeposit = amount > 0;
            lock (_lockObject)
            {
                if (isDeposit && _character.Money < (ulong) amount)
                {
                    _character.SendPacket(new SendMessage(new Message(MessageSender.System, MessageType.Normal, $"You don't have that much money.")));
                    return;
                }

                if (!isDeposit && _character.BankMoney < (ulong) (amount * -1))
                {
                    _character.SendPacket(new SendMessage(new Message(MessageSender.System, MessageType.Normal, $"You don't have that much money.")));
                    return;
                }

                _character.Money -= (ulong) amount;
                _character.BankMoney += (ulong) amount;

                _character.SendPacket(new UpdateCharGold(_character));
                _character.Save(PartialSave.OnlyMoney);
            }
        }

        public void MergeItems(ushort slotFrom, ushort slotTo)
        {
            lock (_lockObject)
            {
                var itemFrom = GetItem(SlotType.Inventory, slotFrom);
                var itemTo = GetItem(SlotType.Inventory, slotTo);
                if (itemFrom == null || itemTo == null || !itemFrom.ItemId.Equals(itemTo.ItemId) || !itemFrom.ItemData.IsLootBox ||
                    !itemTo.ItemData.IsLootBox) return;

                if (itemFrom.Quantity + itemTo.Quantity > DataManager.Instance.CharInitial.Data.ItemStack)
                {
                    SwapItems(SlotType.Inventory, SlotType.Inventory, slotFrom, slotTo);
                    return;
                }

                itemTo.Quantity += itemFrom.Quantity;
                DeleteItem(SlotType.Inventory, slotFrom, false);
                _items[itemTo.SlotType][itemTo.Slot] = itemTo;
                _character.SendPacket(new UpdateItem(itemTo, false));

                _character.Save(PartialSave.Inventory);
            }
        }

        public void SplitItem(SlotType slotType, ushort slot, uint quantity)
        {
            if (quantity >= DataManager.Instance.CharInitial.Data.ItemStack ||
                slotType != SlotType.Inventory && slotType != SlotType.Bank && slotType != SlotType.PranInventory) return;

            lock (_lockObject)
            {
                var item = GetItem(slotType, slot);
                if (!item.ItemData.IsLootBox || item.Quantity <= quantity) return;

                if (AddItem(slotType, quantity, item.ItemId, false))
                {
                    item.Quantity -= (byte) quantity;
                    _items[item.SlotType][item.Slot] = item;
                    _character.SendPacket(new UpdateItem(item, false));
                }

                _character.Save(PartialSave.Inventory);
            }
        }

        public void SendBankData()
        {
            var items = GetItemsBySlotType(SlotType.Bank);
            _character.SendPacket(new UpdateBank(_character.BankMoney, items, 0));
        }

        public void DeleteItem(SlotType slotType, ushort slot, bool save = true)
        {
            lock (_lockObject)
            {
                if (slotType != SlotType.Inventory && slotType != SlotType.Bank && slotType != SlotType.PranInventory) return;

                var item = GetItem(slotType, slot);
                if (item == null) return;

                _removedItems.Add(item.DbId);
                _items[slotType][slot] = null;
                IdItemManager.Instance.ReleaseId(item.DbId);
                _character.SendPacket(new UpdateItem(new Item(slotType, slot, 0, false), false));

                if (save)
                    _character.Save(PartialSave.Inventory);
            }
        }

        public ConcurrentDictionary<ushort, Item> GetItemsBySlotType(SlotType slot)
        {
            lock (_lockObject)
            {
                var list = new ConcurrentDictionary<ushort, Item>();

                foreach (var item in _items[slot])
                    if (item?.ItemId > 0)
                        list.TryAdd(item.Slot, item);

                return list;
            }
        }

        public Item GetItem(SlotType slotType, ushort slot)
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
                var freeSlots = GetFreeSlots(slotType);
                var itemData = DataManager.Instance.ItemsData.GetItemData(itemId);
                if (itemData == null) return false;

                // If item is stackable
                if (itemData.IsLootBox)
                {
                    var maxStack = DataManager.Instance.CharInitial.Data.ItemStack;
                    var stacks = Math.DivRem(quantity, maxStack, out var remainder);
                    if (remainder != 0) stacks++;
                    if (stacks <= 0) return false;

                    // TODO - MSG ERROR NOT ENOUGH SPACE
                    if (freeSlots < stacks) return false;
                    for (var i = 0; i < stacks; i++)
                    {
                        var freeSlot = GetFirstFreeSlot(slotType);
                        var item = new Item(slotType, freeSlot, itemId)
                        {
                            DbId = IdItemManager.Instance.GetNextId(),
                            Quantity = (byte) (quantity > maxStack ? maxStack : quantity),
                            Durability = (byte) itemData.Durability,
                            DurMax = (byte) itemData.Durability,
                        };
                        quantity -= item.Quantity;
                        _items[slotType][freeSlot] = item;
                        _character.SendPacket(new UpdateItem(item, isNotice));
                    }

                    return true;
                }

                // If item don't stack
                // TODO - MSG ERROR NOT ENOUGH SPACE
                if (freeSlots < quantity) return false;

                for (var i = 0; i < quantity; i++)
                {
                    var freeSlot = GetFirstFreeSlot(slotType);
                    var item = new Item(slotType, freeSlot, itemId)
                    {
                        DbId = IdItemManager.Instance.GetNextId(),
                        Quantity = 0,
                        Durability = (byte) itemData.Durability,
                        DurMax = (byte) itemData.Durability,
                    };
                    _items[slotType][freeSlot] = item;
                    _character.SendPacket(new UpdateItem(item, isNotice));
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

                var type = Type.GetType("AikaEmu.GameServer.Models.ItemM.UseItem." + itemType);
                if (type == null)
                {
                    _log.Error("UseItem {0} not implemented.", itemType);
                    return;
                }

                var useItem = (IUseItem) Activator.CreateInstance(type);
                useItem.Init(_character, item, data);
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
                    _character.SendPacket(new UpdateItem(new Item(typeTo, slotTo, 0, false), false));
                }

                if (item2 != null)
                {
                    _items[typeFrom][slotFrom].Slot = slotFrom;
                    _items[typeFrom][slotFrom].SlotType = typeFrom;
                    _character.SendPacket(new UpdateItem(_items[typeFrom][slotFrom], false));
                }
                else
                {
                    _character.SendPacket(new UpdateItem(new Item(typeFrom, slotFrom, 0, false), false));
                }

                _character.Save(PartialSave.Inventory);
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
                        command.Parameters.AddWithValue("@acc_id", _character.Account.Id);
                        command.Parameters.AddWithValue("@char_id", _character.Id);
                        break;
                    case SlotType.Bank:
                        command.CommandText = "SELECT * FROM items WHERE acc_id=@acc_id AND char_id=0";
                        command.Parameters.AddWithValue("@acc_id", _character.Account.Id);
                        break;
                    case SlotType.PranInventory:
                    case SlotType.PranEquipments:
                        command.CommandText = "SELECT * FROM items WHERE acc_id=@acc_id AND pran_id=@pran_id";
                        command.Parameters.AddWithValue("@acc_id", _character.Account.Id);
                        command.Parameters.AddWithValue("@pran_id", _character.ActivePran.DbId);
                        break;
                }

                command.Prepare();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var item = new Item(reader.GetUInt16("item_id"))
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
                            Quantity = reader.GetByte("refinement"),
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
                    command.Parameters.AddWithValue("@acc_id", _character.Account.Id);
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

                        command.CommandText =
                            "REPLACE INTO `items`" +
                            "(`id`, `item_id`, `char_id`, `acc_id`,`pran_id`,`slot_type`, `slot`, `effect1`, `effect2`, `effect3`, `effect1value`, `effect2value`, `effect3value`, `dur`, `dur_max`, `refinement`, `time`)" +
                            "VALUES (@id, @item_id, @char_id, @acc_id, @pran_id, @slot_type, @slot, @effect1, @effect2, @effect3, @effect1value, @effect2value, @effect3value, @dur, @dur_max, @refinement, @time);";

                        command.Parameters.AddWithValue("@id", item.DbId);
                        command.Parameters.AddWithValue("@item_id", item.ItemId);
                        command.Parameters.AddWithValue("@char_id",
                            item.SlotType == SlotType.Inventory || item.SlotType == SlotType.Equipments
                                ? _character.Id
                                : 0);
                        command.Parameters.AddWithValue("@acc_id", _character.Account.Id);
                        command.Parameters.AddWithValue("@pran_id",
                            item.SlotType == SlotType.PranInventory || item.SlotType == SlotType.PranEquipments
                                ? _character.ActivePran?.DbId ?? 0
                                : 0);
                        command.Parameters.AddWithValue("@slot_type", (byte) item.SlotType);
                        command.Parameters.AddWithValue("@slot", item.Slot);
                        command.Parameters.AddWithValue("@effect1", item.Effect1);
                        command.Parameters.AddWithValue("@effect2", item.Effect2);
                        command.Parameters.AddWithValue("@effect3", item.Effect3);
                        command.Parameters.AddWithValue("@effect1value", item.Effect1Value);
                        command.Parameters.AddWithValue("@effect2value", item.Effect2Value);
                        command.Parameters.AddWithValue("@effect3value", item.Effect3Value);
                        command.Parameters.AddWithValue("@dur", item.Durability);
                        command.Parameters.AddWithValue("@dur_max", item.DurMax);
                        command.Parameters.AddWithValue("@refinement", item.Quantity);
                        command.Parameters.AddWithValue("@time", item.ItemTime);
                        command.ExecuteNonQuery();
                        command.Parameters.Clear();
                    }
                }
            }
        }
    }
}