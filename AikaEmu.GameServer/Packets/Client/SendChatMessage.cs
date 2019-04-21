using System;
using AikaEmu.GameServer.Managers;
using AikaEmu.GameServer.Managers.Configuration;
using AikaEmu.GameServer.Managers.Id;
using AikaEmu.GameServer.Models;
using AikaEmu.GameServer.Models.Char.Inventory;
using AikaEmu.GameServer.Models.Unit;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.GameServer.Packets.Game;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Packets.Client
{
	public class SendChatMessage : GamePacket
	{
		protected override void Read(PacketStream stream)
		{
			var unk1 = stream.ReadInt32();
			var unk2 = stream.ReadInt32();
			var chatType = stream.ReadInt32();
			var charName = stream.ReadString(16);
			var msg = stream.ReadString(128);

			Log.Debug("SendChat, unk1: {0}, unk2: {1}, type: {2}", unk1, unk2, chatType);

			var command = msg.Split(" ");
			if (command.Length > 1)
			{
				ushort arg2 = 0;
				ushort arg3 = 0;
				if (!ushort.TryParse(command[1], out var arg1)) return;
				if (command.Length > 2)
				{
					if (!ushort.TryParse(command[2], out arg2)) return;
				}

				if (command.Length > 3)
				{
					if (!ushort.TryParse(command[3], out arg3)) return;
				}

				switch (command[0])
				{
					case "additem":
						var item = new Item
						{
							Id = 1,
							ItemId = arg1,
							SlotType = SlotType.Inventory,
							Slot = arg2,
							Effect1 = 50,
							Effect2 = 55,
							Effect3 = 60,
							Effect1Value = 100,
							Effect2Value = 150,
							Effect3Value = 200,
							Durability = 200,
							DurMax = 200,
							Quantity = Convert.ToByte(arg3),
							DisableDurplus = 1,
							ItemTime = 0,
						};
						Connection.SendPacket(new UpdateItem(item, true));
						break;
					case "move":
						var newPos = new Position
						{
							WorldId = 1, // TODO
							CoordX = Convert.ToSingle(arg1),
							CoordY = Convert.ToSingle(arg2)
						};
						Connection.ActiveCharacter.SetPosition(newPos);
						break;
					case "mobspawn":
						var temp = new Mob
						{
							Id = IdMobSpawnManager.Instance.GetNextId(),
							MobId = arg1,
							Hp = 2000,
							Mp = 2000,
							MaxHp = 2000,
							MaxMp = 2000,
							Name = DataManager.Instance.MnData.GetUnitName(arg1),
							Position = new Position
							{
								WorldId = 1,
								CoordX = Connection.ActiveCharacter.Position.CoordX + 2.0f,
								CoordY = Connection.ActiveCharacter.Position.CoordY + 2.0f
							},
							BodyTemplate = new BodyTemplate
							{
								Width = 7,
								Chest = 119,
								Leg = 119,
								Body = 0
							}
						};
						WorldManager.Instance.Spawn(temp);
						break;
					case "test":
						
						break;
				}
			}
		}
	}
}