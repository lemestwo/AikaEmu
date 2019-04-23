using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class SendXpGoldAnimation : GamePacket
    {
        private readonly uint _expExtra;
        private readonly uint _goldExtra;

        public SendXpGoldAnimation(ushort conId,uint expExtra, uint goldExtra)
        {
            _expExtra = expExtra;
            _goldExtra = goldExtra;
            
            Opcode = (ushort) GameOpcode.SendXpGoldAnimation;
            SenderId = conId;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(_expExtra);
            stream.Write(_goldExtra);
            return stream;
        }
    }
}
