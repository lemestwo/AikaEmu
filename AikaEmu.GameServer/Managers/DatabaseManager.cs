using System;
using System.Collections.Generic;
using AikaEmu.GameServer.Models.Item;
using AikaEmu.GameServer.Models.Units.Character;
using AikaEmu.Shared.Model;

namespace AikaEmu.GameServer.Managers
{
    public class DatabaseManager : Database<DatabaseManager>
    {
        public List<Item> AddItemInventory(List<Item> items, Character character)
        {
            using (var connection = GetConnection())
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    foreach (var item in items)
                    {
                        var parametters = new Dictionary<string, object>
                        {
                            {"item_id", item.ItemId},
                            {"char_id", item.SlotType == SlotType.Inventory || item.SlotType == SlotType.Equipments ? character.Id : 0},
                            {"acc_id", character.Account.Id},
                            {
                                "pran_id",
                                item.SlotType == SlotType.PranInventory || item.SlotType == SlotType.PranEquipments ? character.ActivePran?.DbId ?? 0 : 0
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
                            {"refinement", item.Quantity},
                            {"time", item.ItemTime}
                        };
                        item.DbId = InsertCommand("items", parametters, connection, transaction);
                    }

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    character.Connection.Close();
                    Log.Error(e);
                }
            }

            return items;
        }
    }
}