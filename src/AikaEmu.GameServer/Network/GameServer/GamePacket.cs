using System;
using AikaEmu.GameServer.Models.Units;
using AikaEmu.GameServer.Models.Units.Character;
using AikaEmu.GameServer.Models.Units.Mob;
using AikaEmu.GameServer.Models.Units.Npc;
using AikaEmu.GameServer.Models.Units.Pran;
using AikaEmu.Shared.Model.Network;
using AikaEmu.Shared.Network;
using AikaEmu.Shared.Network.Encryption;

namespace AikaEmu.GameServer.Network.GameServer
{
	public class GamePacket : BasePacket
	{
		public ushort SenderId { private get; set; } = ushort.MaxValue;
		public GameConnection Connection { protected get; set; }

		public PacketStream Encode()
		{
			var stream = new PacketStream();
			try
			{
				if (SenderId == ushort.MaxValue)
					SenderId = AikaEmu.GameServer.GameServer.SystemSender;

				var packet = new PacketStream().Write(0).Write(SenderId).Write(Opcode).Write(Time);
				packet.Write(this);
				stream.Write(packet);
			}
			catch (Exception e)
			{
				Log.Fatal(e);
				throw;
			}

			stream.Replace(stream.Buffer, 0, stream.Count);
			var buffer = stream.Buffer;
			Encryption.Encrypt(ref buffer, buffer.Length);
			stream.Replace(buffer);
			return stream;
		}

		public void Decode(PacketStream stream)
		{
			try
			{
				Read(stream);
			}
			catch (Exception e)
			{
				Log.Fatal(e);
				throw;
			}
		}

		protected void SetSenderIdWithUnit(BaseUnit unit)
		{
			switch (unit)
			{
				case Character character:
					SenderId = character.Connection.Id;
					break;
				case Npc npc:
					SenderId = (ushort) npc.Id;
					break;
				case Mob mob:
					SenderId = (ushort) mob.Id;
					break;
				case Pran pran:
					SenderId = (ushort) pran.Id;
					break;
			}
		}
	}
}