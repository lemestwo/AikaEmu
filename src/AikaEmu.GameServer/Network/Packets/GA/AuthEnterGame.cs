using AikaEmu.GameServer.Network.AuthServer;
using AikaEmu.Shared.Network;
using AikaEmu.Shared.Packets;

namespace AikaEmu.GameServer.Network.Packets.GA
{
    public class AuthEnterGame : AuthGamePacket
    {
        private readonly uint _accId;
        private readonly string _user;
        private readonly int _key;
        private readonly string _pass;
        private readonly byte _gsId;
        private readonly uint _conId;

        public AuthEnterGame(uint accId, string user, int key, string pass, byte gsId, uint conId)
        {
            _accId = accId;
            _user = user;
            _key = key;
            _pass = pass;
            _gsId = gsId;
            _conId = conId;
            Opcode = (ushort) InternalOpcode.AuthEnterGame;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(_accId);
            stream.Write(_user, 32);
            stream.Write(_key);
            stream.Write(_pass, 32);
            stream.Write(_gsId);
            stream.Write(_conId);
            return stream;
        }
    }
}