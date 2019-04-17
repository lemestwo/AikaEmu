using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using AikaEmu.GameServer.Managers.Configuration;
using AikaEmu.GameServer.Managers.Id;
using AikaEmu.GameServer.Models.Char;
using AikaEmu.GameServer.Models.Unit;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.GameServer.Packets.Game;
using MySql.Data.MySqlClient;
using NLog;

namespace AikaEmu.GameServer.Models
{
    public class Account
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();
        public uint Id { get; }
        public uint Level { get; set; } // TODO
        public GameConnection Connection { get; }
        public ushort ConnectionId => Connection.ConnectionId;

        private Dictionary<uint, Character> _accCharLobby;

        public Account(uint accId, GameConnection conn)
        {
            Id = accId;
            Connection = conn;
            Connection.ConnectionId = (ushort) IdConnectionManager.Instance.GetNextId();
        }

        public Character GetSlotCharacter(uint slot)
        {
            return _accCharLobby.ContainsKey(slot) ? _accCharLobby[slot] : null;
        }

        public void SendCharacterList()
        {
            _accCharLobby = new Dictionary<uint, Character>();
            using (var sql = GameServer.Instance.DatabaseManager.GetConnection())
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
                            CharClass = (CharClass) reader.GetUInt16("class"),
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
                                WorldId = 1,
                                CoordX = reader.GetFloat("x"),
                                CoordY = reader.GetFloat("y"),
                            },
                            CharAttributes = new CharAttributes
                            {
                                Strenght = reader.GetUInt16("str"),
                                Agility = reader.GetUInt16("agi"),
                                Intelligence = reader.GetUInt16("int"),
                                Constitution = reader.GetUInt16("const"),
                                Spirit = reader.GetUInt16("spi"),
                            },
                            Experience = reader.GetUInt64("exp"),
                            Money = reader.GetUInt64("money"),
                            HonorPoints = reader.GetInt32("honor_point"),
                            PvpPoints = reader.GetInt32("pvp_point"),
                            InfamyPoints = reader.GetInt32("infamy_point"),
                            Hp = reader.GetInt32("hp"),
                            Mp = reader.GetInt32("mp"),
                            Token = reader.GetString("token"),
                            AccountId = Id
                        };
                        template.Init();
                        _accCharLobby.Add(template.Slot, template);
                    }
                }
            }

            Connection.SendPacket(new SendCharacterList(ConnectionId, Id, _accCharLobby));
        }

        public void CreateCharacter(uint slot, string name, ushort face, ushort hair, bool isRanch)
        {
            var charClass = GetClassByFace(face);
            if (_accCharLobby.Count > 3 || slot >= 3 || charClass == CharClass.Undefined || DataManager.Instance.ItemsData.GetItemSlot(hair) != 1)
            {
                SendCharacterList();
                return;
            }

            foreach (var character in _accCharLobby)
            {
                if (character.Value.Slot != slot) continue;

                SendCharacterList();
                return;
            }

            // TODO - include bad words verification
            var nameRegex = new Regex(DataManager.Instance.CharInitial.Data.NameRegex, RegexOptions.Compiled);
            if (!nameRegex.IsMatch(name))
            {
                Connection.SendPacket(new SendMessage(new Message(16, 1, "This name is already taken.")));
                return;
            }

            var configs = DataManager.Instance.CharInitial;
            var charInitials = configs.GetInitial((ushort) charClass);
            var template = new Character
            {
                Id = IdCharacterManager.Instance.GetNextId(),
                AccountId = Id,
                CharClass = charClass,
                Name = name,
                Position = new Position
                {
                    WorldId = 1,
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
                CharAttributes = new CharAttributes(charInitials.Attributes),
                Experience = 1,
                PvpPoints = 0,
                HonorPoints = 0,
                InfamyPoints = 0,
                BodyTemplate = new BodyTemplate(charInitials.Body)
            };

            if (template.Save())
                _log.Info("Character ({0}) {1} created with success.", template.Id, name);

            SendCharacterList();
        }

        private static CharClass GetClassByFace(ushort face)
        {
            if (face >= 10 && face < 15) return CharClass.Warrior;
            if (face >= 20 && face < 25) return CharClass.Paladin;
            if (face >= 30 && face < 35) return CharClass.Rifleman;
            if (face >= 40 && face < 45) return CharClass.DualGunner;
            if (face >= 50 && face < 55) return CharClass.Warlock;
            if (face >= 60 && face < 65) return CharClass.Cleric;
            return CharClass.Undefined;
        }
    }
}