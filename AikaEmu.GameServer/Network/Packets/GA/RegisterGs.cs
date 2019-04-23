using AikaEmu.GameServer.Network.AuthServer;
using AikaEmu.Shared.Network;
using AikaEmu.Shared.Packets;

namespace AikaEmu.GameServer.Network.Packets.GA
{
    public class RegisterGs : AuthGamePacket
    {
        private readonly byte _gsId;

        public RegisterGs(byte gsId)
        {
            Opcode = (ushort) GameAuthOpcode.RegisterGs;
            _gsId = gsId;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(_gsId);
            return stream;
        }
    }
}