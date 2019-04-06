using AikaEmu.GameServer.Network.AuthServer;
using AikaEmu.Shared.Network;
using AikaEmu.Shared.Network.Packets;

namespace AikaEmu.GameServer.Packets.Game
{
    public class RegisterGS : AuthGamePacket
    {
        private readonly byte _gsId;

        public RegisterGS(byte gsId)
        {
            _gsId = gsId;
            Opcode = (ushort) GameAuthOpcode.RegisterGS;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(_gsId);
            return stream;
        }
    }
}