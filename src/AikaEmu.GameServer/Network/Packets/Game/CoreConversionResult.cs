using AikaEmu.GameServer.Network;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class CoreConversionResult : GamePacket
    {
        private readonly bool _result;

        public CoreConversionResult(ushort conId, bool result)
        {
            _result = result;

            Opcode = (ushort) GameOpcode.CoreConversionResult;
            SenderId = conId;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(_result, true);
            return stream;
        }
    }
}