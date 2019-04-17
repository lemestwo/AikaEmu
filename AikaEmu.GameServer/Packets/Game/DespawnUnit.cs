using AikaEmu.GameServer.Network;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Packets.Game
{
    public class DespawnUnit : GamePacket
    {
        private readonly uint _unitId;

        public DespawnUnit(uint unitId)
        {
            _unitId = unitId;
            Opcode = (ushort) GameOpcode.DespawnUnit;
            ChangeConnectionId = 3005;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(_unitId);
            stream.Write(1); // TODO - could be despawn animation
            return stream;
        }
    }
}