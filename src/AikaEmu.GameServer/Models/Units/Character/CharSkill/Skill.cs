using System;
using AikaEmu.GameServer.Models.Data.JsonModel;
using AikaEmu.GameServer.Models.Units.Character.CharSkill.Const;

namespace AikaEmu.GameServer.Models.Units.Character.CharSkill
{
    public class Skill
    {
        public ushort SkillId { get; set; }
        public byte Level { get; set; }

        public ushort SkillIdx => (ushort) (SkillId + Level - 1);

        public SkillLevel Levelx
        {
            get
            {
                SkillLevel sum = 0;
                for (var i = 1; i <= Level; i++)
                {
                    sum |= (SkillLevel) Enum.Parse(typeof(SkillLevel), "Level" + i);
                }

                return sum;
            }
        }

        public SkillDataJson SkillData { get; set; }
    }
}