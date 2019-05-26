using AikaEmu.GameServer.Models.Units.Character.CharSkill.Const;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class UpdateSkillBar : GamePacket
    {
        private readonly ushort _slot;
        private readonly BarSlotType _slotType;
        private readonly ushort _id;

        public UpdateSkillBar(ushort conId, ushort slot, BarSlotType slotType, ushort id)
        {
            _slot = slot;
            _slotType = slotType;
            _id = id;

            Opcode = (ushort) GameOpcode.UpdateSkillBar;
            SenderId = conId;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write((uint) _slot);
            stream.Write((uint) _slotType);
            stream.Write((uint) _id);
            return stream;
        }
    }
}