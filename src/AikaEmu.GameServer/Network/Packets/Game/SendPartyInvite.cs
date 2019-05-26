using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class SendPartyInvite : GamePacket
    {
        private readonly uint _conIdInviter;
        private readonly string _nameInviter;

        public SendPartyInvite(ushort conId, uint conIdInviter, string nameInviter)
        {
            _conIdInviter = conIdInviter;
            _nameInviter = nameInviter;
            
            Opcode = (ushort) GameOpcode.SendPartyInvite;
            SenderId = conId;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(_conIdInviter);
            stream.Write(_nameInviter, 16);
            return stream;
        }
    }
}