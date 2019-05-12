using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class UnkTitleLink2 : GamePacket
    {
        public UnkTitleLink2()
        {
            Opcode = (ushort) GameOpcode.UnkTitleLink2;
            SenderId = 0;
        }

        public override PacketStream Write(PacketStream stream)
        {
            for (var i = 0; i < 32; i++)
            {
                if (i == 2) stream.Write(272);
                else if (i == 3) stream.Write(16);
                else stream.Write(0);
            }

            return stream;
        }
    }
}