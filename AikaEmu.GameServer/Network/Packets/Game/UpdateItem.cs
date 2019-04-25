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
			stream.Write(_item.ItemId);
			stream.Write(_item.ItemId);
			stream.Write(_item.Id);
			stream.Write(_item.Effect1);
			stream.Write(_item.Effect2);
			stream.Write(_item.Effect3);
			stream.Write((byte) (_item.Effect1Value >> 1));
			stream.Write((byte) (_item.Effect2Value >> 1));
			stream.Write((byte) (_item.Effect3Value >> 1));
			stream.Write(_item.Durability);
			stream.Write(_item.DurMax);
			stream.Write((byte) (_item.Quantity << 4)); // TODO - << 4 for refinements, without for quantity
			stream.Write(_item.DisableDurplus);
			stream.Write(_item.ItemTime);

			return stream;
		}
	}
}