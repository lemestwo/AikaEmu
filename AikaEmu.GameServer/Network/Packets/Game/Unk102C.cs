using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
	public class Unk102C : GamePacket
	{
		public Unk102C()
		{
			Opcode = (ushort) GameOpcode.Unk102C;
			SenderId = 0;
		}

		public override PacketStream Write(PacketStream stream)
		{
			stream.Write(0); // if > 0 only use half packet

			// m128i x4
			stream.Write("", 16);
			stream.Write("", 16);
			stream.Write("", 16);
			stream.Write("", 16);
			return stream;
		}
	}
}