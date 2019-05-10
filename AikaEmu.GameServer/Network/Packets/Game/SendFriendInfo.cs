using AikaEmu.GameServer.Models.Units.Const;
using AikaEmu.GameServer.Network;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class SendFriendInfo : GamePacket
    {
        public SendFriendInfo(ushort conId)
        {
            Opcode = (ushort) GameOpcode.SendFriendInfo;
            SenderId = conId;
        }

        public override PacketStream Write(PacketStream stream)
        {
            byte c = 0;
            byte d= 0;
            ushort e = 0;
            stream.Write("FriendName", 16);
            stream.Write((uint) 1); // id
            // 4 - offline block
            // 3 - online
            // 2 - offline
            // 1 - online block
            stream.Write((byte) c);
            stream.Write((byte) d); // server channel
            stream.Write((ushort) e); // unk - conId?
            stream.Write((byte) Profession.Saint); // profession
            stream.Write((byte) 59); // map
            stream.Write((byte) 49); // lvl
            stream.Write((byte) 1); // nation?
            return stream;
        }
    }
}