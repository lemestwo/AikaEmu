using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class SendFriendOff : GamePacket
    {
        private readonly uint _id;

        public SendFriendOff(ushort conId, uint id)
        {
            _id = id;

            Opcode = (ushort) GameOpcode.SendFriendOff;
            SenderId = conId;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(_id); // dbId
            return stream;
        }
    }
}