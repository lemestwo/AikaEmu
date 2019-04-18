using System;
using AikaEmu.GameServer.Models;
using AikaEmu.GameServer.Models.Base;
using AikaEmu.GameServer.Models.Char.Inventory;
using AikaEmu.GameServer.Network;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Packets.Game
{
	public class SendUnitSpawn : GamePacket
	{
		private readonly BaseUnit _unit;

		public SendUnitSpawn(BaseUnit unit)
		{
			_unit = unit;

			Opcode = (ushort) GameOpcode.SendUnitSpawn;
			SetSenderIdWithUnit(_unit);
		}

		public override PacketStream Write(PacketStream stream)
		{
			if (_unit is Character character)
			{
				stream.Write(character.Name, 16);

				var equips = character.Inventory.GetItemsBySlotType(SlotType.Equipments);
				stream.Write(equips.ContainsKey(0) ? equips[0].ItemId : (ushort) 0);
				stream.Write(equips.ContainsKey(1) ? equips[1].ItemId : (ushort) 0);
				stream.Write(equips.ContainsKey(2) ? equips[2].ItemId : (ushort) 0);
				stream.Write(equips.ContainsKey(3) ? equips[3].ItemId : (ushort) 0);
				stream.Write(equips.ContainsKey(4) ? equips[4].ItemId : (ushort) 0);
				stream.Write(equips.ContainsKey(5) ? equips[5].ItemId : (ushort) 0);
				stream.Write(equips.ContainsKey(6) ? equips[6].ItemId : (ushort) 0);
				stream.Write(equips.ContainsKey(7) ? equips[7].ItemId : (ushort) 0);
				stream.Write((byte) 0); // accessories
				stream.Write((byte) 0); // accessories
				stream.Write((byte) (equips.ContainsKey(2) ? equips[2].Quantity << 4 : 0));
				stream.Write((byte) (equips.ContainsKey(3) ? equips[3].Quantity << 4 : (ushort) 0));
				stream.Write((byte) 0); // accessories
				stream.Write((byte) (equips.ContainsKey(4) ? equips[4].Quantity << 4 : (ushort) 0));
				stream.Write((byte) (equips.ContainsKey(5) ? equips[5].Quantity << 4 : (ushort) 0));
				stream.Write((byte) (equips.ContainsKey(6) ? equips[6].Quantity << 4 : (ushort) 0));
				stream.Write((byte) (equips.ContainsKey(7) ? equips[7].Quantity << 4 : (ushort) 0));
				stream.WriteCc(3);

				stream.Write(character.Position.CoordX);
				stream.Write(character.Position.CoordY);
				stream.Write(0);
				stream.Write(character.Hp);
				stream.Write(character.Hp); // TODO - MAX
				stream.Write(character.Mp);
				stream.Write(character.Mp); // TODO - MAX
				stream.Write((byte) 10); // unk
				stream.Write((byte) 45); // unk
				stream.Write((byte) 1); //spawnType
				stream.Write(character.BodyTemplate.Width);
				stream.Write(character.BodyTemplate.Chest);
				stream.Write(character.BodyTemplate.Leg);
				stream.Write((ushort) 0);
				stream.Write((ushort) 0);
				stream.Write((ushort) 0);
				stream.Write("", 80); 
				stream.Write((ushort) 9031);
				stream.Write("", 38);

				stream.Write(0);

				stream.Write("", 236);
				stream.Write("", 32); // title
				stream.Write(0);
				stream.Write(0); // test maybe title class
				stream.Write(0);
				stream.Write(0);
				stream.Write(0); // test maybe title class
			}
			else if (_unit is Npc npc)
			{
				stream.Write(Convert.ToString(npc.NpcId), 16);
				stream.Write((ushort) npc.NpcId); // TODO - Model ID
				stream.Write("", 26);

				stream.Write(npc.Position.CoordX + 0.4f);
				stream.Write(npc.Position.CoordY + 0.4f);

				stream.Write(180); // unk

				stream.Write(npc.Hp);
				stream.Write(npc.MaxHp);
				stream.Write(npc.Mp);
				stream.Write(npc.MaxMp);
				stream.Write((byte) 0); // unk
				stream.Write((byte) 40); // unk
				stream.Write((byte) 0); //spawnType
				stream.Write(npc.BodyTemplate.Width);
				stream.Write(npc.BodyTemplate.Chest);
				stream.Write(npc.BodyTemplate.Leg);
				stream.Write((byte) 160); // body?
				stream.Write((byte) 1); // unk
				stream.Write((byte) 1); // unk
				stream.Write("", 363);
				stream.Write("TODO", 32);
				stream.Write(0); // unk
				stream.Write(32);
				stream.Write("", 12);
			}


			return stream;
		}
	}
}