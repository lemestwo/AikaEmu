using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using AikaEmu.GameServer.Managers;
using AikaEmu.GameServer.Managers.Configuration;
using AikaEmu.GameServer.Managers.Id;
using AikaEmu.GameServer.Models.Chat;
using AikaEmu.GameServer.Models.Item;
using AikaEmu.GameServer.Models.Item.Const;
using AikaEmu.GameServer.Models.Units;
using AikaEmu.GameServer.Models.Units.Character;
using AikaEmu.GameServer.Models.Units.Const;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.GameServer.Network.Packets.Game;
using NLog;

namespace AikaEmu.GameServer.Models
{
    public enum AccLevel : byte
    {
        Default = 0,

        PgRed1 = 1, // Both are lv 1 in client
        PgRed2 = 3,

        PgBlue1 = 2, // Both are lv 2 in client
        PgBlue2 = 4,
    }

    public class Account
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();
        public uint Id { get; }
        public AccLevel Level { get; set; } = AccLevel.Default;
        public GameConnection Connection { get; }
        public ushort ConnectionId => Connection.Id;
        public Dictionary<uint, Character> AccCharLobby { get; private set; }
        public Character ActiveCharacter { get; set; }

        public Account(uint accId, GameConnection conn)
        {
            Id = accId;
            Connection = conn;
            Connection.Id = (ushort) IdConnectionManager.Instance.GetNextId();

            AccCharLobby = new Dictionary<uint, Character>();
        }

        public Character GetSlotCharacter(uint slot)
        {
            return AccCharLobby.ContainsKey(slot) ? AccCharLobby[slot] : null;
        }

        public void SendCharacterList()
        {
            AccCharLobby.Clear();
            using (var sql = DatabaseManager.Instance.GetConnection())
            using (var command = sql.CreateCommand())
            {
                command.CommandText = "SELECT * FROM characters WHERE acc_id=@acc_id";
                command.Parameters.AddWithValue("@acc_id", Id);
                command.Prepare();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var template = new Character
                        {
                            Id = reader.GetUInt32("id"),
                            Slot = reader.GetUInt32("slot"),
                            Name = reader.GetString("name"),
                            Profession = (Profession) reader.GetUInt16("class"),
                            Level = reader.GetUInt16("level"),
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
                            Account = this
                        };
                        template.PartialInit();
                        AccCharLobby.Add(template.Slot, template);
                    }
                }
            }

            Connection.SendPacket(new SendCharacterList(this));
        }

        public void CreateCharacter(uint slot, string name, ushort face, ushort hair, bool isRanch)
        {
            var charClass = GetClassByFace(face);
            if (AccCharLobby.Count > 3 || slot >= 3 || charClass == Profession.Undefined || DataManager.Instance.ItemsData.GetItemSlot(hair) != ItemType.Hair)
            {
                SendCharacterList();
                return;
            }

            foreach (var character in AccCharLobby)
            {
                if (character.Value.Slot != slot) continue;

                SendCharacterList();
                return;
            }

            // TODO - include bad words verification
            var nameRegex = new Regex(DataManager.Instance.CharInitial.Data.NameRegex, RegexOptions.Compiled);
            if (!nameRegex.IsMatch(name))
            {
                var msg = new Message(MessageSender.System, MessageType.Error, "This name is already taken.");
                Connection.SendPacket(new SendMessage(msg));
                return;
            }

            var configs = DataManager.Instance.CharInitial;
            var charInitials = configs.GetInitial((ushort) charClass);
            var template = new Character
            {
                Account = this,
                Profession = charClass,
                Name = name,
                Position = new Position
                {
                    NationId = 1,
                    CoordX = isRanch ? configs.Data.StartPosition[1].CoordX : configs.Data.StartPosition[0].CoordX,
                    CoordY = isRanch ? configs.Data.StartPosition[1].CoordY : configs.Data.StartPosition[0].CoordY,
                },
                Hp = charInitials.HpMp[0],
                MaxHp = charInitials.HpMp[0],
                Mp = charInitials.HpMp[1],
                MaxMp = charInitials.HpMp[1],
                Slot = slot,
                Level = 1,
                Money = 0,
                Token = string.Empty,
                Attributes = new Attributes(charInitials.Attributes),
                Experience = 1,
                PvpPoints = 0,
                HonorPoints = 0,
                InfamyPoints = 0,
                BodyTemplate = new BodyTemplate(charInitials.Body)
            };

            template.Id = InsertCharacter(template);

            SendCharacterList();
        }

        private static Profession GetClassByFace(ushort face)
        {
            if (face >= 10 && face < 15) return Profession.Warrior;
            if (face >= 20 && face < 25) return Profession.Paladin;
            if (face >= 30 && face < 35) return Profession.Rifleman;
            if (face >= 40 && face < 45) return Profession.DualGunner;
            if (face >= 50 && face < 55) return Profession.Warlock;
            if (face >= 60 && face < 65) return Profession.Cleric;
            return Profession.Undefined;
        }

        private uint InsertCharacter(Character character)
        {
            using (var connection = DatabaseManager.Instance.GetConnection())
            using (var transaction = connection.BeginTransaction())
            using (var command = connection.CreateCommand())
            {
                try
                {
                    command.CommandText =
                        "INSERT INTO `characters`" +
                        "(`acc_id`, `slot`, `name`, `level`, `class`, `width`, `chest`, `leg`, `body`, `exp`, `money`, `hp`, `mp`, `x`, `y`, `rotation`, `honor_point`, `pvp_point`, `infamy_point`, `str`, `agi`, `int`, `const`, `spi`, `token`)" +
                        "VALUES (@acc_id, @slot, @name, @level, @class, @width, @chest, @leg, @body, @exp, @money, @hp, @mp, @x, @y, @rotation, @honor, @pvp, @infamy, @str, @agi, @int, @const, @spi, @token)";

                    command.Parameters.AddWithValue("@acc_id", Id);
                    command.Parameters.AddWithValue("@slot", character.Slot);
                    command.Parameters.AddWithValue("@name", character.Name);
                    command.Parameters.AddWithValue("@level", character.Level);
                    command.Parameters.AddWithValue("@class", (ushort) character.Profession);
                    command.Parameters.AddWithValue("@width", character.BodyTemplate.Width);
                    command.Parameters.AddWithValue("@chest", character.BodyTemplate.Chest);
                    command.Parameters.AddWithValue("@leg", character.BodyTemplate.Leg);
                    command.Parameters.AddWithValue("@body", character.BodyTemplate.Body);
                    command.Parameters.AddWithValue("@exp", character.Experience);
                    command.Parameters.AddWithValue("@money", character.Money);
                    command.Parameters.AddWithValue("@hp", character.Hp);
                    command.Parameters.AddWithValue("@mp", character.Mp);
                    command.Parameters.AddWithValue("@x", character.Position.CoordX);
                    command.Parameters.AddWithValue("@y", character.Position.CoordY);
                    command.Parameters.AddWithValue("@rotation", character.Position.Rotation);
                    command.Parameters.AddWithValue("@honor", character.HonorPoints);
                    command.Parameters.AddWithValue("@pvp", character.PvpPoints);
                    command.Parameters.AddWithValue("@infamy", character.InfamyPoints);
                    command.Parameters.AddWithValue("@str", character.Attributes.Strenght);
                    command.Parameters.AddWithValue("@agi", character.Attributes.Agility);
                    command.Parameters.AddWithValue("@int", character.Attributes.Intelligence);
                    command.Parameters.AddWithValue("@const", character.Attributes.Constitution);
                    command.Parameters.AddWithValue("@spi", character.Attributes.Spirit);
                    command.Parameters.AddWithValue("@token", character.Token);
                    command.ExecuteNonQuery();

                    transaction.Commit();
                    return (uint) command.LastInsertedId;
                }
                catch (Exception e)
                {
                    _log.Error(e.Message);
                    Connection.Close();
                    transaction.Rollback();
                    return 0;
                }
            }
        }
    }
}