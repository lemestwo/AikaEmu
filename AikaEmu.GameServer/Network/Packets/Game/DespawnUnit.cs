using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
	public class DespawnUnit : GamePacket
	{
		private readonly uint _unitId;
		private readonly bool _isNormal;

		public DespawnUnit(uint unitId, bool isNormal = true)
		{
			_unitId = unitId;
			_isNormal = isNormal;

			Opcode = (ushort) GameOpcode.DespawnUnit;
		}

		public override PacketStream Write(PacketStream stream)
		{
			stream.Write(_unitId);
			stream.Write(_isNormal, true); // if its not tp / forced despawn
			return stream;
		}
	}
}