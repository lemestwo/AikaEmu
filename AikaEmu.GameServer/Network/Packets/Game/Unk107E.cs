using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
	public class Unk107E : GamePacket
	{
		private readonly byte _action;
		private readonly uint _id;
		private readonly uint _unk;

		public Unk107E(ushort conId, byte action, uint id, uint unk = 0)
		{
			_action = action;
			_id = id;
			_unk = unk;
			Opcode = (ushort) GameOpcode.Unk107E;
			SenderId = conId;
		}

		public override PacketStream Write(PacketStream stream)
		{
			/*
			 4 7 8 9 10 11 12 13 15 16 19 23 25 actions
			 */
			stream.Write((uint) _action);
			stream.Write(_id);
			stream.Write(_unk);
			return stream;
		}
	}
}