using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class StoneCombinationResult : GamePacket
    {
        private readonly bool _result;

        public StoneCombinationResult(ushort conId, bool result)
        {
            _result = result;

            Opcode = (ushort) GameOpcode.StoneCombinationResult;
            SenderId = conId;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(_result, true);
            return stream;
        }
    }
}