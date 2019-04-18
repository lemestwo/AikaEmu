using AikaEmu.GameServer.Models.Base;
using AikaEmu.GameServer.Network;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Packets.Game
{
	public class UpdateBuffs : GamePacket
	{
		private readonly BaseUnit _unit;

		public UpdateBuffs(BaseUnit unit)
		{
			_unit = unit;

			Opcode = (ushort) GameOpcode.UpdateBuffs;
			SetSenderIdWithUnit(unit);
		}

		public override PacketStream Write(PacketStream stream)
		{
			stream.Write(0); // unk
			stream.Write((ushort) 9031); // buffId
			stream.Write((ushort) 0); //unk
			stream.Write(0L); //unk
			stream.Write((byte) 206); // unk
			stream.Write((byte) 8); //unk
			stream.Write((ushort) 23736); // unk
			return stream;
		}
	}
}