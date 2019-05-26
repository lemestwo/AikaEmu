using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class ResetChatOptions : GamePacket
    {
        public ResetChatOptions()
        {
            Opcode = (ushort) GameOpcode.ResetChatOptions;
        }

        public override PacketStream Write(PacketStream stream)
        {
            return stream;
        }
    }
}