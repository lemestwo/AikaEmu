using System.Collections.Generic;
using System.Text.RegularExpressions;
using AikaEmu.GameServer.Managers;
using AikaEmu.GameServer.Managers.Id;
using AikaEmu.GameServer.Models.Account.Const;
using AikaEmu.GameServer.Models.Chat;
using AikaEmu.GameServer.Models.Chat.Const;
using AikaEmu.GameServer.Models.Item.Const;
using AikaEmu.GameServer.Models.Units;
using AikaEmu.GameServer.Models.Units.Character;
using AikaEmu.GameServer.Models.Units.Const;
using AikaEmu.GameServer.Models.World.Nation;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.GameServer.Network.Packets.Game;

namespace AikaEmu.GameServer.Models.Account
{
    public class Account
    {
        public uint DbId { get; }
        public AccountLevel Level { get; set; } = AccountLevel.Default;
        public NationId NationId { get; set; }
        public GameConnection Connection { get; }
        public Dictionary<byte, Character> AccCharLobby { get; private set; }
        public Character ActiveCharacter { get; set; }

        public Account(uint accDbId, GameConnection connection)
        {
            DbId = accDbId;
            Connection = connection;
            Connection.Id = (ushort) IdConnectionManager.Instance.GetNextId();
            NationId = GetNationId();

            AccCharLobby = new Dictionary<byte, Character>();
        }

        private NationId GetNationId()
        {
            using (var connection = DatabaseManager.Instance.GetConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM account_nation WHERE acc_id=@acc_id";
                command.Parameters.AddWithValue("@acc_id", DbId);
                command.Prepare();
                using (var reader = command.ExecuteReader())
                {
                    return !reader.Read() ? NationId.None : (NationId) reader.GetByte("nation_id");
                }
            }
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
                Connection.SendPacket(new SendMessage(new Message("Not a valid preset.", MessageType.Error), 0));
                return;
            }

            foreach (var character in AccCharLobby)
            {
                if (character.Value.Slot != slot) continue;

                Connection.SendPacket(new SendMessage(new Message("Slot not available.", MessageType.Error), 0));
                return;
            }

            // TODO - include bad words verification
            var nameRegex = new Regex(DataManager.Instance.CharacterData.Data.NameRegex, RegexOptions.Compiled);
            if (!nameRegex.IsMatch(name))
            {
                Connection.SendPacket(new SendMessage(new Message("This name is already taken.", MessageType.Error), 0));
                return;
            }

            var configs = DataManager.Instance.CharacterData;
            var charInitials = configs.GetInitial((ushort) charClass);
            var charTemplate = new Character
            {
                Account = this,
                Profession = charClass,
                Name = name,
                Position = new Position
                {
                    NationId = 1,
                    CoordX = isRanch ? configs.Data.StartPosition[1].X : configs.Data.StartPosition[0].X,
                    CoordY = isRanch ? configs.Data.StartPosition[1].Y : configs.Data.StartPosition[0].Y,
                },
                Hp = charInitials.HpMp[0],
                MaxHp = charInitials.HpMp[0],
                Mp = charInitials.HpMp[1],
                MaxMp = charInitials.HpMp[1],
                Slot = slot,
                Level = 0,
                Money = 0,
                Token = "1111", // TODO - empty.string
                Attributes = new Attributes(charInitials.Attributes),
                Experience = 0,
                PvpPoints = 0,
                HonorPoints = 0,
                InfamyPoints = 0,
                BodyTemplate = new BodyTemplate(charInitials.Body)
            };
            var listItems = new List<Item.Item>
            {
                new Item.Item(SlotType.Equipments, 0, face),
                new Item.Item(SlotType.Equipments, 1, hair)
            };
            foreach (var item in configs.Data.StartItems)
            {
                var itemTemplate = new Item.Item(item.SlotType, item.Slot, item.ItemId, false)
                {
                    Quantity = item.Quantity,
                    Durability = item.Durability,
                    DurMax = item.Durability,
                };
                listItems.Add(itemTemplate);
            }

            foreach (var item in charInitials.Items)
            {
                var itemTemplate = new Item.Item(item.SlotType, item.Slot, item.ItemId, false)
                {
                    Quantity = item.Quantity,
                    Durability = item.Durability,
                    DurMax = item.Durability,
                };
                listItems.Add(itemTemplate);
            }

            // TODO - SKILLS
            if (DatabaseManager.Instance.InsertCharacter(charTemplate, listItems, this))
                SendCharacterList();
            else
                Connection.SendPacket(new SendMessage(new Message("Something went wrong, please contact administration.", MessageType.Error), 0));
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