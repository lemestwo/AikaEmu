using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class EventItemDone : GamePacket
    {
        public EventItemDone(ushort conId)
        {
            Opcode = (ushort) GameOpcode.EventItemDone;
            SenderId = conId;
        }

        public override PacketStream Write(PacketStream stream)
        {
            return stream;
        }
    }
}