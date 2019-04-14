using AikaEmu.GameServer.Network;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Packets.Game
{
    public class UpdateDungeonTimer : GamePacket
    {
        public UpdateDungeonTimer()
        {
            Opcode = (ushort) GameOpcode.UpdateDungeonTimer;
        }

        public override PacketStream Write(PacketStream stream)
        {
            for (var i = 0; i < 38; i++)
            {
                stream.Write(0);
            }
            return stream;
        }
    }
}
