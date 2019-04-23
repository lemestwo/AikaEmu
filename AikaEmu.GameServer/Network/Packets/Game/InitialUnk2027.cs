using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
	public class InitialUnk2027 : GamePacket
	{
		public InitialUnk2027()
		{
			Opcode = (ushort) GameOpcode.InitialUnk202;
			SenderId = 0;
		}

		public override PacketStream Write(PacketStream stream)
		{
			stream.Write((ushort) 0);
			stream.Write((ushort) 2000);
			stream.Write((ushort) 0);
			stream.Write((byte) 0);
			stream.Write("", 5); // empty fill
			stream.Write(100000);
			return stream;
		}
	}
}