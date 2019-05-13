using AikaEmu.GameServer.Network;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class SendTradeRequest : GamePacket
    {
        private readonly ushort _conId;

        public SendTradeRequest(ushort conId)
        {
            _conId = conId;

            Opcode = (ushort) GameOpcode.SendTradeRequest;
            SenderId = conId;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(_conId);
            stream.Write((short) 0);
            stream.Write(1);
            return stream;
        }
    }
}