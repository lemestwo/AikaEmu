using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class Unk1031 : GamePacket
    {
        public Unk1031()
        {
            Opcode = (ushort) GameOpcode.Unk1031;
            SenderId = 0;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(-1); // if <= -4 do something
            stream.Write(0);
            return stream;
        }
    }
}