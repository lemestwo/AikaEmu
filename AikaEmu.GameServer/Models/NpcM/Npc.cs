using System;
using System.Collections.Generic;
using AikaEmu.GameServer.Models.CharacterM;
using AikaEmu.GameServer.Models.Data.JsonModel;
using AikaEmu.GameServer.Models.Data.Npcs;
using AikaEmu.GameServer.Models.NpcM.Dialog;
using AikaEmu.GameServer.Models.QuestM;
using AikaEmu.GameServer.Models.Sound;
using AikaEmu.GameServer.Models.Unit;

namespace AikaEmu.GameServer.Models.NpcM
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

        public List<QuestJson> AvailableQuests(Character character)
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

        public QuestState IsQuestAvailable(QuestJson quest, Character character)
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
                    case QuestM.GetType.Item:
                        break;
                    case QuestM.GetType.LevelRange:
                        if (character.Level >= preCondition.Quantity2 && character.Level <= preCondition.ItemId1)
                            isAvailable--;
                        break;
                    case QuestM.GetType.Class:
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
                    case QuestM.GetType.Unk6:
                        break;
                    case QuestM.GetType.QuestRequire:
                    case QuestM.GetType.AfterMission:
                        var cQuest = character.Quests.GetQuest(quest.Id);
                        if (cQuest != null && cQuest.IsCompleted)
                            isAvailable--;
                        break;
                    case QuestM.GetType.Unk10:
                        break;
                    case QuestM.GetType.Unk29:
                        break;
                    case QuestM.GetType.Unk32:
                        break;
                    case QuestM.GetType.Unk42:
                        break;
                    case QuestM.GetType.Unk55:
                        break;
                    case QuestM.GetType.Unk63:
                        break;
                    case QuestM.GetType.Unk64:
                        break;
                    case QuestM.GetType.Unk67:
                        break;
                    case QuestM.GetType.Unk79:
                        break;
                    case QuestM.GetType.Unk85:
                        break;
                    case QuestM.GetType.Unk95:
                        break;
                    case QuestM.GetType.Unk98:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return isAvailable > 0 ? QuestState.NotAvailable : QuestState.Available;
        }
    }
}