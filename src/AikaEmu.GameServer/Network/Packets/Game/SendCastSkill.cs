using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class SendCastSkill : GamePacket
    {
        private readonly ushort _skillId;
        private readonly ushort _targetConId;
        private readonly float _x;
        private readonly float _y;

        public SendCastSkill(ushort conId, ushort skillId, ushort targetConId, float x, float y)
        {
            _skillId = skillId;
            _targetConId = targetConId;
            _x = x;
            _y = y;

            Opcode = (ushort) GameOpcode.SendCastSkill;
            SenderId = conId;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write((uint) _skillId);
            stream.Write((uint) _targetConId);
            stream.Write(_x);
            stream.Write(_y);
            return stream;
        }
    }
}