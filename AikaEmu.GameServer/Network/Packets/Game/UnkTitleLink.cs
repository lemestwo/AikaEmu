using AikaEmu.GameServer.Models;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
	public class UnkTitleLink : GamePacket
	{
		public UnkTitleLink(Character character)
		{
			Opcode = (ushort) GameOpcode.UnkTitleLink;
			SenderId = character.ConnectionId;
		}

		public override PacketStream Write(PacketStream stream)
		{
			for (var i = 0; i < 12; i++)
			{
				stream.Write((byte) 0);
			}

			return stream;
		}
	}
}