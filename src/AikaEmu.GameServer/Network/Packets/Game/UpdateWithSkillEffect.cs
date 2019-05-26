using AikaEmu.GameServer.Models.Units;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class UpdateWithSkillEffect : GamePacket
    {
        private readonly ushort _conId;
        private readonly ushort _targetId;
        private readonly ushort _unkType;
        private readonly ushort _skillId;
        private readonly Position _pos;

        public UpdateWithSkillEffect(ushort conId, ushort targetId, ushort unkType, ushort skillId, Position pos)
        {
            _conId = conId;
            _targetId = targetId;
            _unkType = unkType;
            _skillId = skillId;
            _pos = pos;

            Opcode = (ushort) GameOpcode.UpdateWithSkillEffect;
            SenderId = conId;
        }

        public override PacketStream Write(PacketStream stream)
        {
            byte a = 26;
            byte c = 1;
            int b = 36;
            var e = 0;
            var f = 0;
            stream.Write((uint) _skillId);
            stream.Write(_pos.CoordX);
            stream.Write(_pos.CoordY);
            stream.Write(0); // 500 sometimes, aoe range?
            stream.Write(_conId);
            stream.Write((byte) 0);
            stream.Write(_unkType); // 5
            stream.Write((byte) 0);
            stream.Write((short) 0);
            stream.Write(0);
            stream.Write(0);
            stream.Write(200); // hp
            stream.Write(0);
            stream.Write(0);
            stream.Write(_targetId);
            stream.Write((byte) c); // 1 - crit / 2 - double / 8 - dodge?
            stream.Write((byte) a); // 23, 26, 30, 29 aoe?
            stream.Write((int) b); // dmg
            //stream.Write((short) d);
            stream.Write(e); // blue dmg
            stream.Write(f); // blue dmg // both sum
            stream.Write(200); // hp
            stream.Write(0); // coords aoe?
            stream.Write(0); // coords aoe?
            /*
             * weird behavior, sometimes this packet has 280 size
             * mostly of the time when last values are valid coordinates
             * stream.Write("", 196);
             */
            return stream;
        }
    }
}