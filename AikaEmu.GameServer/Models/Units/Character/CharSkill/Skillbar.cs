using AikaEmu.GameServer.Models.Units.Character.CharSkill.Const;

namespace AikaEmu.GameServer.Models.Units.Character.CharSkill
{
    public class Skillbar
    {
        public byte Slot { get; set; }
        public ushort SlotId { get; set; }
        public BarSlotType SlotType { get; set; }

        public Skill SkillData { get; set; }
        public Item.Item ItemData { get; set; }
    }
}