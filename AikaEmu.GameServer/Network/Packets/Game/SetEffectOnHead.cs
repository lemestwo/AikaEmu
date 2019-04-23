using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
	public class SetEffectOnHead : GamePacket
	{
		private readonly uint _npcId;
		private readonly uint _effectId;

		public SetEffectOnHead(uint npcId, uint effectId)
		{
			_npcId = npcId;
			_effectId = effectId;

			Opcode = (ushort) GameOpcode.SetEffectOnHead;
		}

		public override PacketStream Write(PacketStream stream)
		{
			stream.Write(_npcId);
			stream.Write(_effectId);
			return stream;
		}
	}
}