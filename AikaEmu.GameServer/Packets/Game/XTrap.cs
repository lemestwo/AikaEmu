using AikaEmu.GameServer.Network;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Packets.Game
{
	public class XTrap : GamePacket
	{
		private readonly int _active;

		public XTrap(int active)
		{
			_active = active;

			Opcode = (ushort) GameOpcode.XTrap;
			SenderId = 0;
		}

		public override PacketStream Write(PacketStream stream)
		{
			stream.Write(_active);
			return stream;
		}
	}
}