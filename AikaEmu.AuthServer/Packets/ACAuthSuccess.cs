using AikaEmu.AuthServer.Network;
using AikaEmu.AuthServer.Network.AuthServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.AuthServer.Packets
{
    public class ACAuthSuccess : AuthPacket
    {
        public ACAuthSuccess()
        {
            Opcode = (ushort) AuthOpcode.ACAuthSuccess;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(2161);
            stream.Write(4863);
            stream.Write(0L);
            return stream;
        }
    }
}