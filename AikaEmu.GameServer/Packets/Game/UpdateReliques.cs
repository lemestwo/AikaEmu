using AikaEmu.GameServer.Network;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Packets.Game
{
	public class UpdateReliques : GamePacket
	{
		public UpdateReliques()
		{
			Opcode = (ushort) GameOpcode.UpdateReliques;
			SenderId = 0;
		}

		public override PacketStream Write(PacketStream stream)
		{
			for (var i = 0; i < 24; i++)
			{
				stream.Write((ushort) (12010 + i)); // itemId
			}

			stream.Write((ushort) 0); // unk
			
			stream.Write((ushort) 0); // triggers event when =1 or =2
			// if 1 - lock all effects and open the special effect tab
			// 2 - unlock all effects and special effects
			// 0 - hide special effects
			return stream;
		}
	}
}