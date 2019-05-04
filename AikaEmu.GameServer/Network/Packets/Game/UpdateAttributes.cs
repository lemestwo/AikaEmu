using AikaEmu.GameServer.Models.Units.Character;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
	public class UpdateAttributes : GamePacket
	{
		private readonly Attributes _attr;

		public UpdateAttributes(Attributes attr)
		{
			_attr = attr;

			Opcode = (ushort) GameOpcode.UpdateAttributes;
		}

		public override PacketStream Write(PacketStream stream)
		{
			stream.Write(_attr);
			stream.Write((ushort) 0);
			stream.Write((ushort) 10); // attr point
			stream.Write((ushort) 20); // skill point
			return stream;
		}
	}
}