using AikaEmu.GameServer.Models.Units.Character;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
	public class UpdateEquipDur : GamePacket
	{
		private readonly Character _character;

		public UpdateEquipDur(Character character)
		{
			_character = character;

			Opcode = (ushort) GameOpcode.UpdateEquipDur;
			SenderId = character.Connection.Id;
		}

		public override PacketStream Write(PacketStream stream)
		{
			var equips = _character.Inventory.GetItemsBySlotType(SlotType.Equipments);
			for (ushort i = 0; i < 16; i++)
			{
				stream.Write(equips.ContainsKey(i) ? equips[i].Durability : (byte) 0);
			}

			return stream;
		}
	}
}