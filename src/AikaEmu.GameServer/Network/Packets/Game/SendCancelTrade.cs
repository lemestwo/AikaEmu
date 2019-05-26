using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class SendCancelTrade : GamePacket
    {
        public SendCancelTrade(ushort conId = 0)
        {
            Opcode = (ushort) GameOpcode.SendCancelTrade;
            if (conId > 0)
                SenderId = conId;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(0);
            return stream;
        }
    }
}