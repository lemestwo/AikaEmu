using AikaEmu.GameServer.Models.Units.Character.CharSkill.Const;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Client
{
    public class RequestUpdateSkillBar : GamePacket
    {
        protected override void Read(PacketStream stream)
        {
            var slot = (byte) stream.ReadUInt32();
            // 2 = skill
            // 6 - inventory item
            var slotType = (BarSlotType) stream.ReadUInt32();
            var id = (ushort) stream.ReadUInt32();

            Connection.ActiveCharacter.SkillsBar.UpdateSkillBar(slot, slotType, id);
        }
    }
}