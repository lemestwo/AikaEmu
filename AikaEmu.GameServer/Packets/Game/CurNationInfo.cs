using AikaEmu.GameServer.Network;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Packets.Game
{
    public class CurNationInfo : GamePacket
    {
        public CurNationInfo()
        {
            Opcode = (ushort) GameOpcode.CurNationInfo;
            SenderId = 0;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write((byte) 0); // rank placement
            stream.Write((byte) 0); // citizen tax
            stream.Write((byte) 0); // visitor tax
            stream.Write((byte) 0); // 
            stream.Write(0);
            stream.Write(0);
            stream.Write((uint) 0); // (byte)
            stream.Write(0); //empty fill
            return stream;
        }
    }
}