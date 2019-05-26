using AikaEmu.AuthServer.Network.GameServer;
using AikaEmu.Shared.Network;
using AikaEmu.Shared.Packets;

namespace AikaEmu.AuthServer.Packets.AG
{
    public class RegisterGs : GameAuthPacket
    {
        private readonly bool _result;

        public RegisterGs(bool result)
        {
            _result = result;
            Opcode = (ushort) InternalOpcode.RegisterGs;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(_result);
            return stream;
        }
    }
}