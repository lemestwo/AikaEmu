using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using AikaEmu.GameServer.Managers;
using AikaEmu.GameServer.Managers.Configuration;
using AikaEmu.GameServer.Managers.Id;
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
        private readonly object _lockObject = new object();

        public Inventory(Character character)
        {
            _character = character;

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

        public ConcurrentDictionary<ushort, Item> GetItemsBySlotType(SlotType slot)
        {
            var list = new ConcurrentDictionary<ushort, Item>();

            foreach (var item in _items[slot])
                if (item?.ItemId > 0)
                    list.TryAdd(item.Slot, item);

            return list;
        }

        public Item GetItem(SlotType slotType, ushort slot)
        {
            foreach (var item in _items[slotType])
            {
                if (item?.Slot == slot) return item;
            }

            return null;
        }

        public void AddItem(SlotType slotType, byte quantity, ushort itemId)
        {
            var freeSlot = GetFreeSlot(slotType);
            if (freeSlot <= 0) return;

            var item = new Item(slotType, freeSlot, itemId)
            {
                Id = IdItemManager.Instance.GetNextId(),
                Quantity = quantity,
                Durability = 100,
            };
            _items[slotType][freeSlot] = item;
            _character.SendPacket(new UpdateItem(item, true));
        }

        public ushort GetFreeSlot(SlotType slotType)
        {
            for (ushort i = 0; i < _items[slotType].Length; i++)
            {
                if (_items[slotType][i] == null || _items[slotType][i].ItemId <= 0) return i;
            }

            return 0;
        }

        public int GetFreeSlots(SlotType slotType)
        {
            var i = 0;
            lock (_lockObject)
            {
                i += _items[slotType].Count(item => item == null);
            }

            return i;
        }

        public void UseItem(SlotType slotType, ushort slot, int data)
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
            useItem.Init(data);
        }

        public void SwapItems(SlotType typeFrom, SlotType typeTo, ushort slotFrom, ushort slotTo)
        {
            var item1 = GetItem(typeFrom, slotFrom);
            var item2 = GetItem(typeTo, slotTo);

            // TODO - Check if can swap
            lock (_lockObject)
            {
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
                    _character.SendPacket(new UpdateItem(new Item(typeTo, slotTo, 0), false));
                }

                if (item2 != null)
                {
                    _items[typeFrom][slotFrom].Slot = slotFrom;
                    _items[typeFrom][slotFrom].SlotType = typeFrom;
                    _character.SendPacket(new UpdateItem(_items[typeFrom][slotFrom], false));
                }
                else
                {
                    _character.SendPacket(new UpdateItem(new Item(typeFrom, slotFrom, 0), false));
                }
            }

            PartialSave();
        }

        public void Init(SlotType slot)
        {
            using (var sql = DatabaseManager.Instance.GetConnection())
            {
                using (var command = sql.CreateCommand())
                {
                    if (slot == SlotType.Inventory || slot == SlotType.Equipments)
                    {
                        command.CommandText = "SELECT * FROM items WHERE acc_id=@acc_id AND char_id=@char_id";
                        command.Parameters.AddWithValue("@acc_id", _character.Account.Id);
                        command.Parameters.AddWithValue("@char_id", _character.Id);
                    }
                    else if (slot == SlotType.Bank)
                    {
                        command.CommandText = "SELECT * FROM items WHERE acc_id=@acc_id AND char_id=0";
                        command.Parameters.AddWithValue("@acc_id", _character.Account.Id);
                    }
                    else if (slot == SlotType.PranInventory || slot == SlotType.PranEquipments)
                    {
                        command.CommandText = "SELECT * FROM items WHERE acc_id=@acc_id AND pran_id=@pran_id";
                        command.Parameters.AddWithValue("@acc_id", _character.Account.Id);
                        command.Parameters.AddWithValue("@pran_id", _character.ActivePran.DbId);
                    }

                    command.Prepare();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = new Item(reader.GetUInt16("item_id"))
                            {
                                Id = reader.GetUInt32("id"),
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
        }

        public void PartialSave()
        {
            using (var connection = DatabaseManager.Instance.GetConnection())
            using (var transaction = connection.BeginTransaction())
            {
                Save(connection, transaction);
                try
                {
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    _log.Error(e);
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception exception)
                    {
                        _log.Error(exception);
                        _character.Connection.Close();
                    }

                    _character.Connection.Close();
                }
            }
        }

        public void Save(MySqlConnection connection, MySqlTransaction transaction)
        {
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

                        command.Parameters.AddWithValue("@id", item.Id);
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