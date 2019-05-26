using System;
using System.Collections.Generic;
using AikaEmu.GameServer.Models.Data.JsonModel;
using AikaEmu.GameServer.Models.Data.Npcs;
using AikaEmu.GameServer.Models.Quest.Const;
using AikaEmu.GameServer.Models.Units.Const;
using AikaEmu.GameServer.Models.Units.Npc.Const;

namespace AikaEmu.GameServer.Models.Units.Npc
{
    public class Npc : BaseUnit
    {
        public ushort NpcId { get; set; }
        public ushort NpcIdX { get; set; }
        public ushort Hair { get; set; }
        public ushort Face { get; set; }
        public ushort Helmet { get; set; }
        public ushort Armor { get; set; }
        public ushort Gloves { get; set; }
        public ushort Pants { get; set; }
        public ushort Weapon { get; set; }
        public ushort Shield { get; set; }
        public byte[] Refinements { get; set; }
        public ushort Unk { get; set; }
        public byte SpawnType { get; set; }
        public ushort UnkId { get; set; }
        public string Title { get; set; }
        public ushort Unk3 { get; set; }

        public uint SoundId { get; set; }
        public SoundType SoundType { get; set; }

        public List<NpcDialogData> DialogList { get; set; }

        public StoreType StoreType { get; set; }
        public ushort[] StoreItems { get; set; }
        public List<QuestJson> Quests { get; set; }

        public Npc()
        {
            DialogList = new List<NpcDialogData>();
        }

        public QuestJson GetQuest(ushort id)
        {
            foreach (var quest in Quests)
                if (quest.Id == id)
                    return quest;

            return null;
        }

        public List<QuestJson> AvailableQuests(Character.Character character)
        {
            var list = new List<QuestJson>();
            foreach (var quest in Quests)
            {
                var questAvailable = IsQuestAvailable(quest, character);
                if (questAvailable != QuestState.NotAvailable && questAvailable != QuestState.GotReward)
                    list.Add(quest);
            }

            return list;
        }

        public QuestState IsQuestAvailable(QuestJson quest, Character.Character character)
        {
            if (quest == null || character == null || quest.Level > character.Level)
                return QuestState.NotAvailable;

            var charQuest = character.Quests.GetQuest(quest.Id);
            if (charQuest != null)
                return charQuest.IsCompleted
                    ? charQuest.IsDone ? QuestState.GotReward : QuestState.Completed
                    : QuestState.OnProgress;

            var isAvailable = quest.PreConditions.Count;
            foreach (var preCondition in quest.PreConditions)
            {
                switch (preCondition.TypeId)
                {
                    case Quest.Const.GetType.Item:
                        break;
                    case Quest.Const.GetType.LevelRange:
                        if (character.Level >= preCondition.Quantity2 && character.Level <= preCondition.ItemId1)
                            isAvailable--;
                        break;
                    case Quest.Const.GetType.Class:
                        var cP = character.Profession;
                        switch ((ProfessionBaseType) preCondition.Quantity2)
                        {
                            case ProfessionBaseType.Warrior:
                                if (cP == Profession.Warrior || cP == Profession.Conquerer || cP == Profession.Warrior3)
                                    isAvailable--;
                                break;
                            case ProfessionBaseType.Paladin:
                                if (cP == Profession.Paladin || cP == Profession.Templar || cP == Profession.Paladin3)
                                    isAvailable--;
                                break;
                            case ProfessionBaseType.DualGunner:
                                if (cP == Profession.DualGunner || cP == Profession.Pistoleer || cP == Profession.DualGunner3)
                                    isAvailable--;
                                break;
                            case ProfessionBaseType.Rifleman:
                                if (cP == Profession.Rifleman || cP == Profession.Sniper || cP == Profession.Rifleman3)
                                    isAvailable--;
                                break;
                            case ProfessionBaseType.Warlock:
                                if (cP == Profession.Warlock || cP == Profession.Arcanist || cP == Profession.Warlock3)
                                    isAvailable--;
                                break;
                            case ProfessionBaseType.Cleric:
                                if (cP == Profession.Cleric || cP == Profession.Saint || cP == Profession.Cleric3)
                                    isAvailable--;
                                break;
                        }

                        break;
                    case Quest.Const.GetType.Unk6:
                        break;
                    case Quest.Const.GetType.QuestRequire:
                    case Quest.Const.GetType.AfterMission:
                        var cQuest = character.Quests.GetQuest(quest.Id);
                        if (cQuest != null && cQuest.IsCompleted)
                            isAvailable--;
                        break;
                    case Quest.Const.GetType.Unk10:
                        break;
                    case Quest.Const.GetType.Unk29:
                        break;
                    case Quest.Const.GetType.Unk32:
                        break;
                    case Quest.Const.GetType.Unk42:
                        break;
                    case Quest.Const.GetType.Unk55:
                        break;
                    case Quest.Const.GetType.Unk63:
                        break;
                    case Quest.Const.GetType.Unk64:
                        break;
                    case Quest.Const.GetType.Unk67:
                        break;
                    case Quest.Const.GetType.Unk79:
                        break;
                    case Quest.Const.GetType.Unk85:
                        break;
                    case Quest.Const.GetType.Unk95:
                        break;
                    case Quest.Const.GetType.Unk98:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return isAvailable > 0 ? QuestState.NotAvailable : QuestState.Available;
        }
    }
}