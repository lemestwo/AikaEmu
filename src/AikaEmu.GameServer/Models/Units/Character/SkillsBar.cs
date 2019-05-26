using System.Collections.Generic;
using AikaEmu.GameServer.Managers;
using AikaEmu.GameServer.Models.Units.Character.CharSkill;
using AikaEmu.GameServer.Models.Units.Character.CharSkill.Const;
using AikaEmu.GameServer.Models.Units.Character.Const;
using AikaEmu.GameServer.Network.Packets.Game;
using AikaEmu.Shared.Model;
using AikaEmu.Shared.Network;
using MySql.Data.MySqlClient;

namespace AikaEmu.GameServer.Models.Units.Character
{
    public class SkillsBar : ISaveData
    {
        private readonly Character _character;
        private readonly Dictionary<byte, Skillbar> _skillBars;
        private readonly List<byte> _removedSlots;

        public SkillsBar(Character character)
        {
            _character = character;
            _skillBars = new Dictionary<byte, Skillbar>();
            _removedSlots = new List<byte>();
        }

        public void UpdateSkillBar(byte slot, BarSlotType slotType, ushort id)
        {
            var isOk = true;
            if (id == 0)
            {
                _skillBars[slot] = null;
                if (!_removedSlots.Contains(slot)) _removedSlots.Add(slot);
            }
            else
            {
                switch (slotType)
                {
                    case BarSlotType.Skill:
                    {
                        var skillData = _character.Skills.GetSkillx(id);
                        if (skillData == null) return;
                        var template = new Skillbar
                        {
                            Slot = slot,
                            SlotId = skillData.SkillId,
                            SlotType = BarSlotType.Skill,
                            SkillData = skillData
                        };
                        _skillBars[slot] = template;
                        if (_removedSlots.Contains(slot)) _removedSlots.Remove(slot);
                    }
                        break;
                    case BarSlotType.Item:
                        // TODO - Implement item 
                        isOk = false;
                        break;
                    default:
                        isOk = false;
                        break;
                }
            }

            if (!isOk) return;
            _character.SendPacket(new UpdateSkillBar(_character.Connection.Id, slot, slotType, id));
            _character.Save(SaveType.SkillBars);
        }

        public PacketStream WriteSkillsBar()
        {
            var stream = new PacketStream();
            for (byte i = 0; i < 25; i++)
            {
                if (_skillBars.ContainsKey(i))
                {
                    stream.Write((byte) (((_skillBars[i].SkillData.Level << 4) ^ 0x02) & 0xFF));
                    stream.Write((ushort) (_skillBars[i].SkillData.SkillData.Idx + (_skillBars[i].SkillData.Level == 16 ? 1 : 0)));
                    stream.Write((byte) 0);
                }
                else
                {
                    stream.Write(0);
                }
            }

            return stream;
        }

        public void Init(MySqlConnection connection)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM character_skillbars WHERE char_id=@char_id";
                command.Parameters.AddWithValue("@char_id", _character.DbId);
                command.Prepare();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var template = new Skillbar
                        {
                            Slot = reader.GetByte("slot"),
                            SlotId = reader.GetUInt16("slot_id"),
                            SlotType = (BarSlotType) reader.GetByte("slot_type"),
                        };
                        switch (template.SlotType)
                        {
                            case BarSlotType.Skill:
                                template.SkillData = _character.Skills.GetSkill(template.SlotId);
                                if (template.SkillData != null)
                                    _skillBars.Add(template.Slot, template);
                                break;
                            case BarSlotType.Item:
                                // TODO
                                break;
                        }
                    }
                }
            }
        }

        public void Save(MySqlConnection connection, MySqlTransaction transaction)
        {
            if (_removedSlots.Count > 0)
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM character_skillbars WHERE char_id=@char_id AND slot IN(" + string.Join(",", _removedSlots) + ")";
                    command.Prepare();
                    command.Parameters.AddWithValue("@char_id", _character.DbId);
                    command.ExecuteNonQuery();
                }

                _removedSlots.Clear();
            }

            foreach (var skill in _skillBars.Values)
            {
                if (skill == null) continue;
                var parameters = new Dictionary<string, object>
                {
                    {"char_id", _character.DbId},
                    {"slot", skill.Slot},
                    {"slot_id", skill.SlotId},
                    {"slot_type", (byte) skill.SlotType}
                };
                DatabaseManager.Instance.MySqlCommand(SqlCommandType.Replace, "character_skillbars", parameters, connection, transaction);
            }
        }
    }
}