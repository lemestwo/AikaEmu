using AikaEmu.AuthServer.Network.GameServer;
using AikaEmu.Shared.Network;
using AikaEmu.Shared.Packets;

namespace AikaEmu.AuthServer.Packets.AG
{
    public class RequestEnterResult : GameAuthPacket
    {
        private readonly uint _accId;
        private readonly uint _conId;
        private readonly byte _result;

        public RequestEnterResult(uint accId, uint conId, byte result)
        {
            _accId = accId;
            _conId = conId;
            _result = result;
            Opcode = (ushort) InternalOpcode.RequestEnterResult;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(_accId);
            stream.Write(_conId);
            stream.Write(_result);
            return stream;
        }
    }
}