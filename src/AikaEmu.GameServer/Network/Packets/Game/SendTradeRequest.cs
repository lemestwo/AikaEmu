using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class SendTradeRequest : GamePacket
    {
        private readonly ushort _ownerConId;

        public SendTradeRequest(ushort conId, ushort ownerConId)
        {
            _ownerConId = ownerConId;

            Opcode = (ushort) GameOpcode.SendTradeRequest;
            SenderId = conId;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write((uint) _ownerConId);
            return stream;
        }
    }
}