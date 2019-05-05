using AikaEmu.GameServer.Models.Data.JsonModel;

namespace AikaEmu.GameServer.Models.Units.Character.Skill
{
    public class Skill
    {
        public ushort SkillId { get; set; }
        public byte Level { get; set; }

        public ushort SkillIdx => (ushort) (SkillId + Level - 1);

        public SkillDataJson SkillData { get; set; }
    }
}