using AikaEmu.GameServer.Network;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Packets.Game
{
    public class SetEffectOnHead : GamePacket
    {
        public SetEffectOnHead()
        {
            Opcode = (ushort) GameOpcode.SetEffectOnHead;
        }

        public override PacketStream Write(PacketStream stream)
        {
            // 0E 09 00 00 05 00 00 00
            return stream;
        }
    }
}