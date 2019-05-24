using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class CoreUpgradeResult : GamePacket
    {
        private readonly bool _result;

        public CoreUpgradeResult(ushort conId, bool result)
        {
            _result = result;
            Opcode = (ushort) GameOpcode.CoreUpgradeResult;
            SenderId = conId;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(_result, true);
            return stream;
        }
    }
}