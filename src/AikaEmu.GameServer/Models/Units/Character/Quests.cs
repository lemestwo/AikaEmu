using System;
using System.Collections.Generic;
using AikaEmu.GameServer.Managers;
using AikaEmu.GameServer.Models.Data.JsonModel;
using AikaEmu.GameServer.Network.Packets.Game;
using AikaEmu.Shared.Model;
using MySql.Data.MySqlClient;

namespace AikaEmu.GameServer.Models.Units.Character
{
    public class Quests : ISaveData
    {
        private readonly Dictionary<ushort, Quest.Quest> _quests;
        private readonly Character _character;

        public Quests(Character character)
        {
            _character = character;
            _quests = new Dictionary<ushort, Quest.Quest>();
        }

        public Quest.Quest GetQuest(ushort id)
        {
            return _quests.ContainsKey(id) ? _quests[id] : null;
        }

        public Quest.Quest AddQuest(QuestJson quest)
        {
            if (_quests.ContainsKey(quest.Id)) return null;
            var temp = new Quest.Quest
            {
                Id = quest.Id,
                QuestData = quest,
            };
            return _quests.TryAdd(quest.Id, temp) ? temp : null;
        }

        public void AddReward(ushort questId)
        {
            var quest = GetQuest(questId);
            if (quest == null || !quest.IsCompleted || quest.IsDone) return;

            var maxMoney = DataManager.Instance.CharacterData.Data.MaxGold;
            var maxLevel = DataManager.Instance.CharacterData.Data.MaxLevel;
            var maxStack = DataManager.Instance.CharacterData.Data.ItemStack;

            var canProcced = quest.QuestData.Rewards.Count;
            var totalStacks = 0;
            foreach (var reward in quest.QuestData.Rewards)
            {
                switch (reward.TypeId)
                {
                    case Quest.Const.GetType.Item:
                    {
                        var stacks = Math.DivRem(maxStack, maxStack, out var remainder);
                        if (remainder != 0) stacks++;
                        totalStacks += stacks;
                        if (_character.Inventory.GetFreeSlots(SlotType.Inventory) >= totalStacks)
                            canProcced--;
                    }
                        break;
                    case Quest.Const.GetType.ExperienceGuild:
                        break;
                    case Quest.Const.GetType.Unk10:
                        break;
                    case Quest.Const.GetType.ItemChoice:
                        break;
                    case Quest.Const.GetType.Experience:
                    {
                        // TODO - EXPERIENCE LVUP
                        canProcced--;
                    }
                        break;
                    case Quest.Const.GetType.Gold:
                    {
                        if (_character.Money + (ulong) reward.Quantity2 <= maxMoney)
                            canProcced--;
                    }
                        break;
                    case Quest.Const.GetType.ClassChange:
                        break;
                    case Quest.Const.GetType.Unk18:
                        break;
                    case Quest.Const.GetType.Pran:
                        break;
                    case Quest.Const.GetType.Unk23:
                        break;
                    case Quest.Const.GetType.Unk26:
                        break;
                    case Quest.Const.GetType.Unk27:
                        break;
                    case Quest.Const.GetType.Unk47:
                        break;
                    case Quest.Const.GetType.Unk60:
                        break;
                    case Quest.Const.GetType.SkillAcquire:
                        break;
                    case Quest.Const.GetType.Unk79:
                        break;
                }
            }

            if (canProcced > 0) return;

            var modXp = 0u;
            var modMoney = 0u;
            foreach (var reward in quest.QuestData.Rewards)
            {
                switch (reward.TypeId)
                {
                    case Quest.Const.GetType.Item:
                        _character.Inventory.AddItem(SlotType.Inventory, (uint) reward.Unk6, (ushort) reward.Unk3);
                        break;
                    case Quest.Const.GetType.ExperienceGuild:
                        break;
                    case Quest.Const.GetType.Unk10:
                        break;
                    case Quest.Const.GetType.ItemChoice:
                        break;
                    case Quest.Const.GetType.Experience:
                        _character.Experience += (ulong) reward.Quantity2;
                        modXp += (uint) reward.Quantity2;
                        _character.SendPacket(new UpdateExperience(_character));
                        break;
                    case Quest.Const.GetType.Gold:
                        _character.Money += (ulong) reward.Quantity2;
                        modMoney += (uint) reward.Quantity2;
                        _character.SendPacket(new UpdateCharGold(_character));
                        break;
                    case Quest.Const.GetType.ClassChange:
                        break;
                    case Quest.Const.GetType.Unk18:
                        break;
                    case Quest.Const.GetType.Pran:
                        break;
                    case Quest.Const.GetType.Unk23:
                        break;
                    case Quest.Const.GetType.Unk26:
                        break;
                    case Quest.Const.GetType.Unk27:
                        break;
                    case Quest.Const.GetType.Unk47:
                        break;
                    case Quest.Const.GetType.Unk60:
                        break;
                    case Quest.Const.GetType.SkillAcquire:
                        break;
                    case Quest.Const.GetType.Unk79:
                        break;
                }
            }

            if (modXp > 0 || modMoney > 0)
                _character.SendPacketAll(new SendXpGoldAnimation(_character.Connection.Id, modXp, modMoney));
            _quests[questId].IsDone = true;
            _character.SendPacket(new RemoveQuestInfo(questId));
        }

        public void Init(MySqlConnection connection)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM character_quests WHERE char_id=@char_id";
                command.Parameters.AddWithValue("@char_id", _character.DbId);
                command.Prepare();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var quest = new Quest.Quest
                        {
                            Id = reader.GetUInt16("quest_id"),
                            Completed =
                            {
                                [0] = reader.GetByte("req_1"),
                                [1] = reader.GetByte("req_2"),
                                [2] = reader.GetByte("req_3"),
                                [3] = reader.GetByte("req_4"),
                                [4] = reader.GetByte("req_5"),
                            },
                            IsDone = reader.GetBoolean("is_done"),
                        };
                        quest.QuestData = DataManager.Instance.QuestData.GetData(quest.Id);

                        if (quest.QuestData != null)
                            _quests.Add(quest.Id, quest);
                    }
                }
            }
        }

        public void Save(MySqlConnection connection, MySqlTransaction transaction)
        {
            foreach (var quest in _quests.Values)
            {
                var parameters = new Dictionary<string, object>
                {
                    {"char_id", _character.DbId},
                    {"quest_id", quest.Id},
                    {"req_1", quest.Completed[0]},
                    {"req_2", quest.Completed[1]},
                    {"req_3", quest.Completed[2]},
                    {"req_4", quest.Completed[3]},
                    {"req_5", quest.Completed[4]},
                    {"is_done", quest.IsDone}
                };
                DatabaseManager.Instance.MySqlCommand(SqlCommandType.Replace, "character_quests", parameters, connection, transaction);
            }
        }
    }
}