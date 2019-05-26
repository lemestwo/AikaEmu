using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class CloseNpcChat : GamePacket
    {
        public CloseNpcChat()
        {
            Opcode = (ushort) GameOpcode.CloseNpcChat;
        }

        public override PacketStream Write(PacketStream stream)
        {
            return stream;
        }
    }
}