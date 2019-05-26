using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class SendTradeResult : GamePacket
    {
        private readonly ushort _conId;

        public SendTradeResult(ushort conId)
        {
            _conId = conId;

            Opcode = (ushort) GameOpcode.SendTradeResult;
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