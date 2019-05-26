using System;
using System.Collections.Generic;
using AikaEmu.GameServer.Models.Account;
using AikaEmu.GameServer.Models.Item;
using AikaEmu.GameServer.Models.Units;
using AikaEmu.GameServer.Models.Units.Character;
using AikaEmu.GameServer.Models.Units.Character.CharFriend;
using AikaEmu.GameServer.Models.Units.Const;
using AikaEmu.Shared.Model;

namespace AikaEmu.GameServer.Managers
{
    public class DatabaseManager : Database<DatabaseManager>
    {
        public Dictionary<byte, Character> GetCharactersFromAccount(Account account)
        {
            var dictionary = new Dictionary<byte, Character>();

            using (var sql = GetConnection())
            using (var command = sql.CreateCommand())
            {
                command.CommandText = "SELECT * FROM characters WHERE acc_id=@acc_id";
                command.Parameters.AddWithValue("@acc_id", account.DbId);
                command.Prepare();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var template = new Character
                        {
                            DbId = reader.GetUInt32("id"),
                            Slot = reader.GetByte("slot"),
                            Name = reader.GetString("name"),
                            Profession = (Profession) reader.GetUInt16("class"),
                            Level = reader.GetUInt16("level"),
                            SkillPoints = reader.GetUInt16("skill_points"),
                            AttrPoints = reader.GetUInt16("attr_points"),
                            BodyTemplate = new BodyTemplate
                            {
                                Width = reader.GetByte("width"),
                                Chest = reader.GetByte("chest"),
                                Leg = reader.GetByte("leg"),
                                Body = reader.GetByte("body"),
                            },
                            Position = new Position
                            {
                                NationId = 1,
                                CoordX = reader.GetFloat("x"),
                                CoordY = reader.GetFloat("y"),
                                Rotation = reader.GetInt16("rotation"),
                            },
                            Attributes = new Attributes(reader.GetUInt16("str"), reader.GetUInt16("agi"), reader.GetUInt16("int"),
                                reader.GetUInt16("const"), reader.GetUInt16("spi")),
                            Experience = reader.GetUInt64("exp"),
                            Money = reader.GetUInt64("money"),
                            HonorPoints = reader.GetInt32("honor_point"),
                            PvpPoints = reader.GetInt32("pvp_point"),
                            InfamyPoints = reader.GetInt32("infamy_point"),
                            Hp = reader.GetInt32("hp"),
                            Mp = reader.GetInt32("mp"),
                            Token = reader.GetString("token"),
                            Account = account
                        };
                        template.PartialInit();
                        dictionary.Add(template.Slot, template);
                    }
                }
            }

            return dictionary;
        }

        public uint InsertFriend(Character character, Friend friend)
        {
            var id = 0u;
            using (var connection = GetConnection())
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    var parameters = new Dictionary<string, object>
                    {
                        {"char_id", character.DbId},
                        {"friend_id", friend.FriendId},
                        {"name", friend.Name},
                        {"is_blocked", friend.IsBlocked}
                    };
                    id = MySqlCommand(SqlCommandType.Insert, "character_friends", parameters, connection, transaction);

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    character.Connection.Close();
                    Log.Error(e);
                }
            }

            return id;
        }

        public void RemoveOfflineFriend(Friend friend)
        {
            using (var connection = GetConnection())
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "DELETE FROM character_friends WHERE char_id=@char_id AND friend_id=@friend_id";
                        command.Parameters.AddWithValue("@char_id", friend.FriendId);
                        command.Parameters.AddWithValue("@friend_id", friend.CharacterId);
                        command.Prepare();
                        command.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    Log.Error(e);
                }
            }
        }

        public List<Item> InsertItems(List<Item> items, Character character)
        {
            using (var sqlConnection = GetConnection())
            using (var sqlTransaction = sqlConnection.BeginTransaction())
            {
                try
                {
                    foreach (var item in items)
                    {
                        var parameters = new Dictionary<string, object>
                        {
                            {"item_id", item.ItemId},
                            {"char_id", item.SlotType == SlotType.Inventory || item.SlotType == SlotType.Equipments ? character.DbId : 0},
                            {"acc_id", character.Account.DbId},
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
                            {"quantity", item.Quantity},
                            {"time", item.ItemTime}
                        };
                        item.DbId = MySqlCommand(SqlCommandType.Insert, "items", parameters, sqlConnection, sqlTransaction);
                    }

                    sqlTransaction.Commit();
                }
                catch (Exception e)
                {
                    sqlTransaction.Rollback();
                    character.Connection.Close();
                    Log.Error(e);
                }
            }

            return items;
        }

        public bool InsertCharacter(Character character, List<Item> items, Account account)
        {
            using (var connection = GetConnection())
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    var characterParameter = new Dictionary<string, object>
                    {
                        {"acc_id", account.DbId},
                        {"slot", character.Slot},
                        {"name", character.Name},
                        {"level", character.Level},
                        {"class", (ushort) character.Profession},
                        {"width", character.BodyTemplate.Width},
                        {"chest", character.BodyTemplate.Chest},
                        {"leg", character.BodyTemplate.Leg},
                        {"body", character.BodyTemplate.Body},
                        {"exp", character.Experience},
                        {"money", character.Money},
                        {"hp", character.Hp},
                        {"mp", character.Mp},
                        {"x", character.Position.CoordX},
                        {"y", character.Position.CoordY},
                        {"rotation", character.Position.Rotation},
                        {"honor_point", character.HonorPoints},
                        {"pvp_point", character.PvpPoints},
                        {"infamy_point", character.InfamyPoints},
                        {"str", character.Attributes.Strenght},
                        {"agi", character.Attributes.Agility},
                        {"int", character.Attributes.Intelligence},
                        {"const", character.Attributes.Constitution},
                        {"spi", character.Attributes.Spirit},
                        {"token", character.Token}
                    };
                    character.DbId = MySqlCommand(SqlCommandType.Insert, "characters", characterParameter, connection, transaction);
                    if (character.DbId > 0)
                    {
                        var resultItems = InsertItems(items, character);
                        var isOk = true;
                        foreach (var item in resultItems)
                            if (item.DbId <= 0)
                                isOk = false;

                        if (isOk)
                        {
                            transaction.Commit();
                            return true;
                        }
                    }

                    transaction.Rollback();
                    return false;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    account.Connection.Close();
                    Log.Error(e);
                    return false;
                }
            }
        }
    }
}