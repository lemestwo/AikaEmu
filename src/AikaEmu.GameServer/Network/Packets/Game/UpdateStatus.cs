using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
	public class UpdateStatus : GamePacket
	{
		public UpdateStatus()
		{
			Opcode = (ushort) GameOpcode.UpdateStatus;
		}

		public override PacketStream Write(PacketStream stream)
		{
            stream.Write((ushort) 27); // pAtt
            stream.Write((ushort) 48); // pDef
            stream.Write((ushort) 11); // mAtt
            stream.Write((ushort) 53); // mDef
            stream.Write((ushort) 0);
            stream.Write((ushort) 0);
            stream.Write((ushort) 0);
            stream.Write((ushort) 70); // move speed
            stream.Write((ushort) 10); // unk
            stream.Write((ushort) 0);
            stream.Write((ushort) 0);
            stream.Write((ushort) 58); // nuk ?????
            stream.Write((ushort) 8); // crit
            stream.Write((byte) 5); // dodge rate
            stream.Write((byte) 17); // hit rate
            stream.Write((ushort) 6); // double
            stream.Write((ushort) 7); // resist
			return stream;
		}
	}
}