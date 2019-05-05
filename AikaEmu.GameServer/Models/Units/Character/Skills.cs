using System.Collections.Generic;
using AikaEmu.GameServer.Managers;
using AikaEmu.Shared.Model;
using MySql.Data.MySqlClient;

namespace AikaEmu.GameServer.Models.Units.Character
{
    public class Skills : ISaveData
    {
        private readonly Character _character;
        private readonly Dictionary<byte, Skill.Skill[]> _skillList;

        public Skills(Character character)
        {
            _character = character;
            _skillList = new Dictionary<byte, Skill.Skill[]>
            {
                {0, new Skill.Skill[6]},
                {1, new Skill.Skill[6]},
                {2, new Skill.Skill[6]},
                {3, new Skill.Skill[6]},
                {4, new Skill.Skill[6]},
                {5, new Skill.Skill[6]},
                {6, new Skill.Skill[6]},
            };
        }

        public Skill.Skill GetSkill(ushort skillId)
        {
            foreach (var skill in _skillList.Values)
            {
                foreach (var sk in skill)
                {
                    if (sk.SkillId == skillId) return sk;
                }
            }

            return null;
        }

        public bool AddSkill(ushort skillId)
        {
            // TODO
            return true;
        }

        public void Init(MySqlConnection connection)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM character_skills WHERE char_id=@char_id";
                command.Parameters.AddWithValue("@char_id", _character.Id);
                command.Prepare();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var template = new Skill.Skill
                        {
                            SkillId = reader.GetUInt16("skill_id"),
                            Level = reader.GetByte("level"),
                        };
                        template.SkillData = DataManager.Instance.SkillData.GetSkillData(template.SkillIdx);
                        if (template.SkillData != null)
                        {
                            _skillList[template.SkillData.Tier][template.SkillData.TierPos] = template;
                        }
                    }
                }
            }
        }

        public void Save(MySqlConnection connection, MySqlTransaction transaction)
        {
            foreach (var skill in _skillList.Values)
            {
                foreach (var sk in skill)
                {
                    if (sk == null) continue;

                    var parameters = new Dictionary<string, object>
                    {
                        {"char_id", _character.Id},
                        {"skill_id", sk.SkillId},
                        {"level", sk.Level}
                    };
                    DatabaseManager.Instance.MySqlCommand(SqlCommandType.Replace, "character_skills", parameters, connection, transaction);
                }
            }
        }
    }
}