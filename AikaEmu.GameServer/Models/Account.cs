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
    public class Account
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();
        public uint Id { get; }
        public AccountLevel Level { get; set; } = AccountLevel.Default;
        public GameConnection Connection { get; }
        public Dictionary<byte, Character> AccCharLobby { get; private set; }
        public Character ActiveCharacter { get; set; }

        public Account(uint accId, GameConnection conn)
        {
            Id = accId;
            Connection = conn;
            Connection.Id = (ushort) IdConnectionManager.Instance.GetNextId();

            AccCharLobby = new Dictionary<byte, Character>();
        }

        public Character GetSlotCharacter(byte slot)
        {
            return AccCharLobby.ContainsKey(slot) ? AccCharLobby[slot] : null;
        }

        public void SendCharacterList()
        {
            AccCharLobby.Clear();
            AccCharLobby = DatabaseManager.Instance.GetCharactersFromAccount(this);

            Connection.SendPacket(new SendCharacterList(this));
        }

        public void CreateCharacter(byte slot, string name, ushort face, ushort hair, bool isRanch)
        {
            var charClass = GetClassByFace(face);
            if (AccCharLobby.Count > 3 || slot >= 3 || charClass == Profession.Undefined || DataManager.Instance.ItemsData.GetItemSlot(hair) != ItemType.Hair)
            {
                var msg = new Message(MessageSender.System, MessageType.Error, "Not a valid preset.");
                Connection.SendPacket(new SendMessage(msg, 0));
                return;
            }

            foreach (var character in AccCharLobby)
            {
                if (character.Value.Slot != slot) continue;

                var msg = new Message(MessageSender.System, MessageType.Error, "Slot not available.");
                Connection.SendPacket(new SendMessage(msg, 0));
                return;
            }

            // TODO - include bad words verification
            var nameRegex = new Regex(DataManager.Instance.CharInitial.Data.NameRegex, RegexOptions.Compiled);
            if (!nameRegex.IsMatch(name))
            {
                var msg = new Message(MessageSender.System, MessageType.Error, "This name is already taken.");
                Connection.SendPacket(new SendMessage(msg, 0));
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
                Experience = 0,
                PvpPoints = 0,
                HonorPoints = 0,
                InfamyPoints = 0,
                BodyTemplate = new BodyTemplate(charInitials.Body)
            };

            if (DatabaseManager.Instance.AddNewCharacter(template, this))
                SendCharacterList();
            else
            {
                var msg = new Message(MessageSender.System, MessageType.Error, "Something went wrong, please contact administration.");
                Connection.SendPacket(new SendMessage(msg, 0));
            }
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
    }
}