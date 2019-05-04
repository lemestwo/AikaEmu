using AikaEmu.GameServer.Models.Item;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
	public class UpdateItem : GamePacket
	{
		private readonly Item _item;
		private readonly bool _isNotice;

		public UpdateItem(Item item, bool isNotice)
		{
			_item = item;
			_isNotice = isNotice;

			Opcode = (ushort) GameOpcode.UpdateItem;
		}

		public override PacketStream Write(PacketStream stream)
		{
			stream.Write(_isNotice);
			stream.Write((byte) _item.SlotType);
			stream.Write(_item.Slot);
			stream.Write(_item);

			return stream;
		}
	}
}