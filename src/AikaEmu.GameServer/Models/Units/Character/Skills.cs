using System.Collections.Generic;
using AikaEmu.GameServer.Managers;
using AikaEmu.GameServer.Models.Units.Character.CharSkill;
using AikaEmu.Shared.Model;
using AikaEmu.Shared.Network;
using MySql.Data.MySqlClient;

namespace AikaEmu.GameServer.Models.Units.Character
{
    public class Skills : ISaveData
    {
        private readonly Character _character;
        private readonly Dictionary<byte, Skill[]> _skillList;

        public Skills(Character character)
        {
            _character = character;
            _skillList = new Dictionary<byte, Skill[]>
            {
                {0, new Skill[6]},
                {1, new Skill[6]},
                {2, new Skill[6]},
                {3, new Skill[6]},
                {4, new Skill[6]},
                {5, new Skill[6]},
                {6, new Skill[6]},
            };
        }

        public Skill GetSkill(ushort skillId)
        {
            foreach (var skill in _skillList.Values)
            {
                foreach (var sk in skill)
                {
                    if (sk?.SkillId == skillId) return sk;
                }
            }

            return null;
        }

        public Skill GetSkillx(ushort skillIdx)
        {
            foreach (var skill in _skillList.Values)
            {
                foreach (var sk in skill)
                {
                    if (sk?.SkillIdx == skillIdx) return sk;
                }
            }

            return null;
        }

        public bool AddSkill(ushort skillId, Skill playerSkill = null)
        {
            if (playerSkill != null)
            {
                _skillList[playerSkill.SkillData.Tier][playerSkill.SkillData.TierPos].Level++;
            }
            else
            {
                var template = new Skill
                {
                    SkillId = skillId,
                    Level = 1,
                    SkillData = DataManager.Instance.SkillData.GetData(skillId)
                };
                if (template.SkillData != null)
                {
                    _skillList[template.SkillData.Tier][template.SkillData.TierPos] = template;
                }
            }

            return true;
        }

        public PacketStream WriteSkills()
        {
            var stream = new PacketStream();

            for (byte i = 0; i < 10; i++)
            {
                if (!_character.Skills._skillList.ContainsKey(i))
                {
                    stream.Write("", 12);
                    continue;
                }

                for (byte j = 0; j < 6; j++)
                {
                    if (_character.Skills._skillList[i][j] != null)
                    {
                        var skillByte = (ushort) _character.Skills._skillList[i][j].Levelx;
                        if (i > 0 && j == 0 && _character.Skills._skillList[(byte) (i - 1)][5]?.Level == 16 && skillByte < ushort.MaxValue) skillByte++;
                        else if (j > 0 && _character.Skills._skillList[i][j - 1]?.Level == 16 && skillByte < ushort.MaxValue) skillByte++;
                        stream.Write(skillByte);
                    }
                    else
                        stream.Write((ushort) 0);
                }
            }

            return stream;
        }


        public void Init(MySqlConnection connection)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM character_skills WHERE char_id=@char_id";
                command.Parameters.AddWithValue("@char_id", _character.DbId);
                command.Prepare();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var template = new Skill
                        {
                            SkillId = reader.GetUInt16("skill_id"),
                            Level = reader.GetByte("level"),
                        };
                        template.SkillData = DataManager.Instance.SkillData.GetData(template.SkillIdx);
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
                        {"char_id", _character.DbId},
                        {"skill_id", sk.SkillId},
                        {"level", sk.Level}
                    };
                    DatabaseManager.Instance.MySqlCommand(SqlCommandType.Replace, "character_skills", parameters, connection, transaction);
                }
            }
        }
    }
}