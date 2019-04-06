using AikaEmu.AuthServer.Network.GameServer;
using AikaEmu.Shared.Network;
using AikaEmu.Shared.Network.Packets;

namespace AikaEmu.AuthServer.Packets.Auth
{
    public class RegisterGSResult : GameAuthPacket
    {
        private readonly bool _result;

        public RegisterGSResult(bool result)
        {
            _result = result;
            Opcode = (ushort) GameAuthOpcode.RegisterGSResult;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(_result);
            return stream;
        }
    }
}