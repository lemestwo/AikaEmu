using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class FinishedInGameState : GamePacket
    {
        public FinishedInGameState()
        {
            Opcode = (ushort) GameOpcode.FinishedInGameState;
            SenderId = 0;
        }

        public override PacketStream Write(PacketStream stream)
        {
            return stream;
        }
    }
}
