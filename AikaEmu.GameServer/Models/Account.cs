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
		}

		public Character GetSlotCharacter(uint slot)
		{
			return AccCharLobby.ContainsKey(slot) ? AccCharLobby[slot] : null;
		}

		public void SendCharacterList()
		{
			AccCharLobby = new Dictionary<uint, Character>();
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
							CharAttributes = new CharAttributes(reader.GetUInt16("str"), reader.GetUInt16("agi"), reader.GetUInt16("int"),
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
						template.Init();
						AccCharLobby.Add(template.Slot, template);
					}
				}
			}

			Connection.SendPacket(new SendCharacterList(this));
		}

		public void CreateCharacter(uint slot, string name, ushort face, ushort hair, bool isRanch)
		{
			var charClass = GetClassByFace(face);
			if (AccCharLobby.Count > 3 || slot >= 3 || charClass == CharClass.Undefined || DataManager.Instance.ItemsData.GetItemSlot(hair) != 1)
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
				Id = IdCharacterManager.Instance.GetNextId(),
				Account = this,
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