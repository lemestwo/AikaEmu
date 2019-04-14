using AikaEmu.GameServer.Network;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Packets.Game
{
    public class PranEffect : GamePacket
    {
        private readonly uint _targetId;
        private readonly uint _effectId;

        public PranEffect(uint targetId, uint effectId)
        {
            _targetId = targetId;
            _effectId = effectId;
            Opcode = (ushort) GameOpcode.PranEffect;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(_targetId);
            stream.Write(_effectId);
            return stream;
        }
    }
}