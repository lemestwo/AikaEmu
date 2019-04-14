using AikaEmu.GameServer.Network;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Packets.Game
{
    public class UpdateAccountLevel : GamePacket
    {
        private readonly int _level;

        public UpdateAccountLevel(int level)
        {
            _level = level;
            Opcode = (ushort) GameOpcode.UpdateAccountLevel;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(_level);
            return stream;
        }
    }
}