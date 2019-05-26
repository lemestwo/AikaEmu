using AikaEmu.AuthServer.Network;
using AikaEmu.AuthServer.Network.AuthServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.AuthServer.Packets.Game
{
    public class AuthResult : AuthPacket
    {
        private readonly uint _accId;
        private readonly uint _key;

        public AuthResult(uint accId, uint key)
        {
            _accId = accId;
            _key = key;
            Opcode = (ushort) AuthOpcode.AuthResult;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(_accId);
            stream.Write(_key);
            stream.Write(2L); // TODO - nationId?
            return stream;
        }
    }
}