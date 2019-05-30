using System;
using System.Collections.Generic;
using AikaEmu.GameServer.Managers;
using AikaEmu.GameServer.Models.Item.Const;
using AikaEmu.GameServer.Models.Units.Character.Const;
using AikaEmu.GameServer.Models.Units.Const;
using AikaEmu.GameServer.Models.Units.Npc.Const;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.GameServer.Network.Packets.Game;
using AikaEmu.Shared.Model;
using MySql.Data.MySqlClient;
using NLog;

namespace AikaEmu.GameServer.Models.Units.Character
{
    public class Character : BaseUnit
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();

        public Account.Account Account { get; set; }
        public GameConnection Connection => Account.Connection;

        public new ushort Id => Connection.Id;
        public uint DbId { get; set; }
        public byte Slot { get; set; }
        public ulong Money { get; set; }
        public ulong BankMoney { get; set; }
        public ulong Experience { get; set; }
        public ushort SkillPoints { get; set; }
        public ushort AttrPoints { get; set; }
        public int HonorPoints { get; set; }
        public int PvpPoints { get; set; }
        public int InfamyPoints { get; set; }
        public string Token { get; set; }

        public Attributes Attributes { get; set; }
        public Profession Profession { get; set; }
        public Inventory Inventory { get; private set; }
        public Pran.Pran ActivePran { get; private set; }
        public Quests Quests { get; private set; }
        public Skills Skills { get; private set; }
        public SkillsBar SkillsBar { get; private set; }
        public Friends Friends { get; private set; }
        public Titles Titles { get; private set; }

        public ShopType OpenedShopType { get; set; }
        public uint OpenedShopNpcConId { get; set; }

        public bool IsInternalDisconnect { get; set; }

        public void ActivatePran()
        {
            var item = Inventory.GetItem(SlotType.Equipments, (ushort) ItemType.PranStone);
            if (item == null) return;
            var pran = new Pran.Pran(Account, item.DbId);
            if (pran.Load())
            {
                ActivePran = pran;
            }
        }

        public void Init()
        {
            using (var connection = DatabaseManager.Instance.GetConnection())
            {
                BankMoney = InitBankMoney(connection);
                Inventory = new Inventory(this);
                Inventory.Init(connection, SlotType.Inventory);
                Inventory.Init(connection, SlotType.Equipments);
                Inventory.Init(connection, SlotType.Bank);
                if (ActivePran != null)
                {
                    Inventory.Init(connection, SlotType.PranInventory);
                    Inventory.Init(connection, SlotType.PranEquipments);
                }

                Quests = new Quests(this);
                Quests.Init(connection);
                Skills = new Skills(this);
                Skills.Init(connection);
                // Skillbars must be initialized after skills
                SkillsBar = new SkillsBar(this);
                SkillsBar.Init(connection);
                Friends = new Friends(this);
                Friends.Init(connection);
                Titles = new Titles(this);
                Titles.Init(connection);
            }

            IsInternalDisconnect = false;
        }

        public void PartialInit()
        {
            using (var connection = DatabaseManager.Instance.GetConnection())
            {
                Inventory = new Inventory(this);
                Inventory.Init(connection, SlotType.Equipments);
            }
        }

        public void SendPacket(GamePacket packet)
        {
            Connection.SendPacket(packet);
        }

        public void SendPacketAll(GamePacket packet, bool inRange = true, bool includeMyself = false)
        {
            if (inRange)
            {
                foreach (var visibleUnits in VisibleUnits.Values)
                {
                    if (visibleUnits is Character character)
                        character.SendPacket(packet);
                }

                if (includeMyself)
                    SendPacket(packet);
                return;
            }

            foreach (var character in WorldManager.Instance.GetCharacters())
                character.SendPacket(packet);
        }

        public override void SetPosition(Position pos)
        {
            ActivePran?.SetPosition(Position);
            Position = pos;

            SendPacketAll(new UpdatePosition(this, 0));
            WorldManager.Instance.ShowVisibleUnits(this);
        }

        public void TeleportTo(float x, float y)
        {
            var pos = (Position) Position.Clone();
            pos.CoordX = x;
            pos.CoordY = y;

            Position = pos;
            ActivePran?.SetPosition(pos);

            Connection.SendPacket(new UpdatePosition(this, 1));
            WorldManager.Instance.ShowVisibleUnits(this);
        }

        public bool UpdateMoney(ulong value, bool isAdd = false)
        {
            if (isAdd) Money += value;
            else Money -= value;
            SendPacket(new UpdateCharGold(this));
            return true;
        }

        private ulong InitBankMoney(MySqlConnection connection)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM bank_gold WHERE acc_id=@acc_id";
                command.Parameters.AddWithValue("@acc_id", Account.DbId);
                command.Prepare();
                using (var reader = command.ExecuteReader())
                {
                    return !reader.Read() ? 0 : reader.GetUInt64("gold");
                }
            }
        }

        private void SaveBankMoney(MySqlConnection connection, MySqlTransaction transaction)
        {
            var parameters = new Dictionary<string, object>
            {
                {"acc_id", Account.DbId},
                {"gold", BankMoney}
            };
            DatabaseManager.Instance.MySqlCommand(SqlCommandType.Replace, "bank_gold", parameters, connection, transaction);
        }

        public bool Save(SaveType saveType = SaveType.All)
        {
            using (var connection = DatabaseManager.Instance.GetConnection())
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    var parameters = new Dictionary<string, object>
                    {
                        {"id", DbId},
                        {"acc_id", Account.DbId},
                        {"slot", Slot},
                        {"name", Name},
                        {"level", Level},
                        {"class", (ushort) Profession},
                        {"width", BodyTemplate.Width},
                        {"chest", BodyTemplate.Chest},
                        {"leg", BodyTemplate.Leg},
                        {"body", BodyTemplate.Body},
                        {"exp", Experience},
                        {"money", Money},
                        {"skill_points", SkillPoints},
                        {"attr_points", AttrPoints},
                        {"hp", Hp},
                        {"mp", Mp},
                        {"x", Position.CoordX},
                        {"y", Position.CoordY},
                        {"rotation", Position.Rotation},
                        {"honor_point", HonorPoints},
                        {"pvp_point", PvpPoints},
                        {"infamy_point", InfamyPoints},
                        {"str", Attributes.Strenght},
                        {"agi", Attributes.Agility},
                        {"int", Attributes.Intelligence},
                        {"const", Attributes.Constitution},
                        {"spi", Attributes.Spirit},
                        {"token", Token}
                    };
                    DatabaseManager.Instance.MySqlCommand(SqlCommandType.Replace, "characters", parameters, connection, transaction);

                    SaveBankMoney(connection, transaction);
                    switch (saveType)
                    {
                        case SaveType.All:
                            Inventory?.Save(connection, transaction);
                            Quests?.Save(connection, transaction);
                            Skills?.Save(connection, transaction);
                            Friends?.Save(connection, transaction);
                            Titles?.Save(connection, transaction);
                            break;
                        case SaveType.Inventory:
                            Inventory?.Save(connection, transaction);
                            break;
                        case SaveType.Quests:
                            Quests?.Save(connection, transaction);
                            break;
                        case SaveType.OnlyCharacter:
                            break;
                        case SaveType.Skills:
                            Skills?.Save(connection, transaction);
                            break;
                        case SaveType.SkillBars:
                            SkillsBar?.Save(connection, transaction);
                            break;
                        case SaveType.Friends:
                            Friends?.Save(connection, transaction);
                            break;
                        case SaveType.Titles:
                            Titles?.Save(connection, transaction);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(saveType), saveType, null);
                    }

                    transaction.Commit();
                    return true;
                }
                catch (Exception e)
                {
                    _log.Error(e);
                    transaction.Rollback();
                    Connection?.Close();
                    return false;
                }
            }
        }
    }
}