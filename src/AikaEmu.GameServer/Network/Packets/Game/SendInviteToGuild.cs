using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class SendInviteToGuild : GamePacket
    {
        private readonly string _name;

        public SendInviteToGuild(ushort conId, string name)
        {
            _name = name;
            Opcode = (ushort) GameOpcode.SendInviteToGuild;
            SenderId = conId;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(_name, 16);
            return stream;
        }
    }
}
