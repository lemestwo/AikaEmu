using System;
using System.Collections.Generic;
using AikaEmu.GameServer.Managers;
using AikaEmu.GameServer.Models.ItemM;
using AikaEmu.GameServer.Models.NpcM;
using AikaEmu.GameServer.Models.PranM;
using AikaEmu.GameServer.Models.Unit;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.GameServer.Network.Packets.Game;
using MySql.Data.MySqlClient;
using NLog;

namespace AikaEmu.GameServer.Models.CharacterM
{
    public class Character : BaseUnit
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();

        public Account Account { get; set; }
        public GameConnection Connection => Account.Connection;
        public ushort ConnectionId => Connection.Account.ConnectionId;

        public uint Slot { get; set; }
        public ulong Money { get; set; }
        public ulong BankMoney { get; set; }
        public ulong Experience { get; set; }
        public int HonorPoints { get; set; }
        public int PvpPoints { get; set; }
        public int InfamyPoints { get; set; }
        public string Token { get; set; }

        public Attributes Attributes { get; set; }
        public Professions Professions { get; set; }
        public Inventory Inventory { get; private set; }
        public Pran ActivePran { get; private set; }

        public ShopType OpenedShopType { get; set; }
        public uint OpenedShopNpcConId { get; set; }

        public void ActivatePran()
        {
            var item = Inventory.GetItem(SlotType.Equipments, (ushort) ItemType.PranStone);
            if (item == null) return;
            var pran = new Pran(Account, item.DbId);
            if (pran.Load())
            {
                ActivePran = pran;
            }
        }

        public void Init()
        {
            using (var connection = DatabaseManager.Instance.GetConnection())
            {
                Inventory = new Inventory(this);
                Inventory.Init(connection, SlotType.Inventory);
                Inventory.Init(connection, SlotType.Equipments);
                Inventory.Init(connection, SlotType.Bank);
                InitBankMoney(connection);
                if (ActivePran != null)
                {
                    Inventory.Init(connection, SlotType.PranInventory);
                    Inventory.Init(connection, SlotType.PranEquipments);
                }
            }
        }

        public void PartialInit()
        {
            using (var sql = DatabaseManager.Instance.GetConnection())
            {
                Inventory = new Inventory(this);
                Inventory.Init(sql, SlotType.Equipments);
            }
        }

        public void SendPacket(GamePacket packet)
        {
            Connection.SendPacket(packet);
        }

        public void SendPacketAll(GamePacket packet, bool inRange = true)
        {
            // TODO - inRange
            foreach (var character in WorldManager.Instance.GetOnlineCharacters())
            {
                character.SendPacket(packet);
            }
        }

        public override void SetPosition(Position pos)
        {
            Position = pos;
            ActivePran?.SetPosition(pos);

//            if (AbosoluteDistance(pos.CoordX, Position.CoordX) > 150 || AbosoluteDistance(pos.CoordY, Position.CoordY) > 150)
//                Connection.SendPacket(new UpdatePosition(this, 1));

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

        private void InitBankMoney(MySqlConnection connection)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM bank_gold WHERE acc_id=@acc_id";
                command.Parameters.AddWithValue("@acc_id", Id);
                command.Prepare();
                using (var reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        BankMoney = 0;
                        return;
                    }

                    BankMoney = reader.GetUInt64("gold");
                }
            }
        }

        private void SaveBankMoney(MySqlConnection connection, MySqlTransaction transaction)
        {
            using (var command = connection.CreateCommand())
            {
                command.Connection = connection;
                command.Transaction = transaction;

                command.CommandText =
                    "REPLACE INTO `bank_gold` (`acc_id`,`gold`,`updated_at`) VALUES (@acc_id, @gold, @updated_at)";
                command.Parameters.AddWithValue("@acc_id", Account.Id);
                command.Parameters.AddWithValue("@gold", BankMoney);
                command.Parameters.AddWithValue("@updated_at", DateTime.UtcNow);
                command.ExecuteNonQuery();
            }
        }

        public bool Save(PartialSave partialSave = PartialSave.All)
        {
            using (var connection = DatabaseManager.Instance.GetConnection())
            using (var transaction = connection.BeginTransaction())
            {
                using (var command = connection.CreateCommand())
                {
                    command.Connection = connection;
                    command.Transaction = transaction;

                    command.CommandText =
                        "REPLACE INTO `characters`" +
                        "(`id`,`acc_id`, `slot`, `name`, `level`, `class`, `width`, `chest`, `leg`, `body`, `exp`, `money`, `hp`, `mp`, `x`, `y`, `rotation`, `honor_point`, `pvp_point`, `infamy_point`, `str`, `agi`, `int`, `const`, `spi`, `token`, `updated_at`)" +
                        "VALUES (@id, @acc_id, @slot, @name, @level, @class, @width, @chest, @leg, @body, @exp, @money, @hp, @mp, @x, @y, @rotation, @honor, @pvp, @infamy, @str, @agi, @int, @const, @spi, @token, @updated_at)";

                    command.Parameters.AddWithValue("@id", Id);
                    command.Parameters.AddWithValue("@acc_id", Account.Id);
                    command.Parameters.AddWithValue("@slot", Slot);
                    command.Parameters.AddWithValue("@name", Name);
                    command.Parameters.AddWithValue("@level", Level);
                    command.Parameters.AddWithValue("@class", (ushort) Professions);
                    command.Parameters.AddWithValue("@width", BodyTemplate.Width);
                    command.Parameters.AddWithValue("@chest", BodyTemplate.Chest);
                    command.Parameters.AddWithValue("@leg", BodyTemplate.Leg);
                    command.Parameters.AddWithValue("@body", BodyTemplate.Body);
                    command.Parameters.AddWithValue("@exp", Experience);
                    command.Parameters.AddWithValue("@money", Money);
                    command.Parameters.AddWithValue("@hp", Hp);
                    command.Parameters.AddWithValue("@mp", Mp);
                    command.Parameters.AddWithValue("@x", Position.CoordX);
                    command.Parameters.AddWithValue("@y", Position.CoordY);
                    command.Parameters.AddWithValue("@rotation", Position.Rotation);
                    command.Parameters.AddWithValue("@honor", HonorPoints);
                    command.Parameters.AddWithValue("@pvp", PvpPoints);
                    command.Parameters.AddWithValue("@infamy", InfamyPoints);
                    command.Parameters.AddWithValue("@str", Attributes.Strenght);
                    command.Parameters.AddWithValue("@agi", Attributes.Agility);
                    command.Parameters.AddWithValue("@int", Attributes.Intelligence);
                    command.Parameters.AddWithValue("@const", Attributes.Constitution);
                    command.Parameters.AddWithValue("@spi", Attributes.Spirit);
                    command.Parameters.AddWithValue("@token", Token);
                    command.Parameters.AddWithValue("@updated_at", DateTime.UtcNow);
                    command.ExecuteNonQuery();
                }

                switch (partialSave)
                {
                    case PartialSave.All:
                        SaveBankMoney(connection, transaction);
                        Inventory?.Save(connection, transaction);
                        break;
                    case PartialSave.Inventory:
                        SaveBankMoney(connection, transaction);
                        Inventory?.Save(connection, transaction);
                        break;
                    case PartialSave.OnlyMoney:
                        SaveBankMoney(connection, transaction);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(partialSave), partialSave, null);
                }

                try
                {
                    transaction.Commit();
                    return true;
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
                    }

                    return false;
                }
            }
        }
    }
}