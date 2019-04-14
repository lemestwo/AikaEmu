using AikaEmu.AuthServer.Network;
using AikaEmu.AuthServer.Network.AuthServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.AuthServer.Packets.Game
{
    public class AuthSuccess : AuthPacket
    {
        private readonly uint _accId;
        private readonly int _key;

        public AuthSuccess(uint accId, int key)
        {
            _accId = accId;
            _key = key;
            Opcode = (ushort) AuthOpcode.AuthSuccess;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(_accId);
            stream.Write(_key);
            stream.Write(0L);
            return stream;
        }
    }
}