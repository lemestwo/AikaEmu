using AikaEmu.GameServer.Network.AuthServer;
using AikaEmu.Shared.Network;
using AikaEmu.Shared.Packets;

namespace AikaEmu.GameServer.Network.Packets.GA
{
    public class ResetAuthHash : AuthGamePacket
    {
        private readonly uint _accId;

        public ResetAuthHash(uint accId)
        {
            _accId = accId;

            Opcode = (ushort) InternalOpcode.ResetAuthHash;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(_accId);
            return stream;
        }
    }
}