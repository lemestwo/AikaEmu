using AikaEmu.GameServer.Models;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
	public class PlaySound : GamePacket
	{
		private readonly uint _soundId;
		private readonly SoundType _type;

		public PlaySound(uint soundId, SoundType type)
		{
			_soundId = soundId;
			_type = type;
			Opcode = (ushort) GameOpcode.PlaySound;
		}

		public override PacketStream Write(PacketStream stream)
		{
			stream.Write(_soundId);
			stream.Write((int) _type);
			return stream;
		}
	}
}