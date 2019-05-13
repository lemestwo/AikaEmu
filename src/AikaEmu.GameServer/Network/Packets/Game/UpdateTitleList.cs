using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class UpdateTitleList : GamePacket
    {
        public UpdateTitleList(ushort conId)
        {
            Opcode = (ushort) GameOpcode.UpdateTitleList;
            SenderId = conId;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(0); // id - when acquire
            stream.Write(0); // id - when update level
            // TODO - ids are different, need to find the relation
            return stream;
        }
    }
}
