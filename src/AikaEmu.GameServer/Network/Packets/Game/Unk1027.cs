using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class Unk1027 : GamePacket
    {
        public Unk1027(ushort conId)
        {
            Opcode = (ushort) GameOpcode.Unk1027;
            SenderId = conId;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write((ushort) 0); // id?
            stream.Write((ushort) 0);
            stream.Write(0);
            return stream;
        }
    }
}
