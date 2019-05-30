using AikaEmu.GameServer.Managers;
using AikaEmu.GameServer.Models;
using AikaEmu.GameServer.Models.Chat;
using AikaEmu.GameServer.Models.Chat.Const;
using AikaEmu.GameServer.Models.Quest.Const;
using AikaEmu.GameServer.Models.Units.Character;
using AikaEmu.GameServer.Models.Units.Character.Const;
using AikaEmu.GameServer.Models.Units.Const;
using AikaEmu.GameServer.Models.Units.Npc;
using AikaEmu.GameServer.Models.Units.Npc.Const;
using AikaEmu.GameServer.Network.Packets.Game;
using AikaEmu.GameServer.Utils;
using NLog;

namespace AikaEmu.GameServer.Helpers
{
    public static class NpcDialogHelper
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public static void BuySkillFromNpc(Character character, uint npcConId, ushort skillId)
        {
            if (character.OpenedShopNpcConId != npcConId || character.OpenedShopType != ShopType.SkillShop)
            {
                character.Connection.Close();
                return;
            }

            var skillData = DataManager.Instance.SkillData.GetData(skillId);
            if (skillData == null) return;

            var isError = false;
            var msg = string.Empty;
            if (skillData.Profession != character.Profession && skillData.Profession != character.Profession - 1 &&
                skillData.Profession != character.Profession - 2)
            {
                msg = "This skill doesn't fit your profession.";
                isError = true;
            }
            else if (character.Money < skillData.LearnPrice || character.SkillPoints < skillData.LearnSkillPoint)
            {
                msg = "Not enough resources to buy this skill.";
                isError = true;
            }
            // LEVEL +1 because of how Aika handles levels
            else if (character.Level + 1 < skillData.RequiredLevel)
            {
                msg = "Not enough level to buy this skill.";
                isError = true;
            }
            else if (skillData.Level > skillData.MaxLevel)
            {
                msg = "Unknow error.";
                isError = true;
            }

            if (isError)
            {
                character.Connection.SendPacket(new SendMessage(new Message(msg, MessageType.Error)));
                return;
            }

            var playerSkill = character.Skills.GetSkill((ushort) (skillData.Id - skillData.Level + 1));
            if (playerSkill != null && playerSkill.Level + 1 != skillData.Level) return;

            if (!character.Skills.AddSkill(skillId, playerSkill)) return;

            character.Money -= skillData.LearnPrice;
            character.SkillPoints -= skillData.LearnSkillPoint;
            character.SendPacket(new UpdateCharGold(character));
            character.SendPacket(new UpdateAttributes(character));
            character.SendPacket(new UpdateSkills(character));
            character.Save(SaveType.Skills);
        }

        public static void BuyFromShop(Character character, ushort npcConId, int index, uint quantity)
        {
            if (character.OpenedShopNpcConId != npcConId || character.OpenedShopType != ShopType.Store)
            {
                character.Connection.Close();
                return;
            }

            if (quantity <= 0) return;

            var npc = WorldManager.Instance.GetNpc(npcConId);
            if (npc == null) return;

            if (npc.StoreItems[index] <= 0) return;

            var item = DataManager.Instance.ItemsData.GetData(npc.StoreItems[index]);
            if (item == null) return;

            if (GlobalUtils.IsEquipment(item.ItemType) && quantity > 1 || character.Money < item.BuyPrice * quantity) return;

            if (!character.Inventory.AddItem(SlotType.Inventory, quantity, npc.StoreItems[index])) return;

            character.SendPacket(new Unk303D(character, 0));
            character.Money -= item.BuyPrice * quantity;
            character.SendPacket(new UpdateCharGold(character));
            character.SendPacket(new Unk303D(character, 1));

            character.Save(SaveType.Inventory);
        }

        public static void StartDialog(Character character, ushort npcId, DialogType optionId, uint subOptionId)
        {
            if (character.OpenedShopType != ShopType.None) return;

            var npc = WorldManager.Instance.GetNpc(npcId);
            if (npc == null) return;

            if (!MathUtils.CheckInRange(character.Position, npc.Position, 5)) return;

            if (optionId <= 0)
            {
                if (npc.DialogList == null || npc.DialogList.Count <= 0) return;
                character.SendPacket(new OpenNpcChat(npc.Id));
                character.SendPacket(new PlaySound(npc.SoundId, npc.SoundType));
                character.SendPacket(new ResetChatOptions());

                var quests = npc.AvailableQuests(character);
                if (quests.Count > 0)
                {
                    var temp = new NpcDialog(DialogType.Quest, 0, $"Mission ({quests.Count})");
                    character.SendPacket(new SendNpcOption(temp));
                }

                foreach (var dialog in npc.DialogList)
                {
                    if (dialog.SubOptionId == 0)
                    {
                        var temp = new NpcDialog(dialog.OptionId, 0, dialog.Text);
                        character.SendPacket(new SendNpcOption(temp));
                    }
                }

                Log.Debug("OpenedChat, NpcId: {0}, Id: {1}", npc.NpcId, npc.Id);
            }
            else if (optionId == DialogType.Talk || optionId == DialogType.Quest || optionId == DialogType.Teleport)
            {
                character.SendPacket(new ResetChatOptions());
                switch (optionId)
                {
                    case DialogType.Quest when subOptionId <= 0:
                    {
                        var quests = npc.AvailableQuests(character);
                        foreach (var quest in quests)
                        {
                            var temp = new NpcDialog(DialogType.Quest, quest.Id, $"({quest.Id}) {quest.Name}");
                            character.SendPacket(new SendNpcOption(temp));
                        }

                        character.SendPacket(new SendNpcOption(new NpcDialog(DialogType.ChatClose, 0, "Close")));
                    }
                        break;
                    case DialogType.Quest when subOptionId > 0:
                    {
                        var quest = npc.GetQuest((ushort) subOptionId);
                        if (quest == null)
                        {
                            character.SendPacket(new SendNpcOption(new NpcDialog(DialogType.ChatClose, 0, "Close")));
                            return;
                        }

                        character.SendPacket(new PlaySound(7, SoundType.Bgm));
                        switch (npc.IsQuestAvailable(quest, character))
                        {
                            case QuestState.Available:
                                character.SendPacket(new NpcStartTalk(quest.StartDialog));
                                character.SendPacket(new SendNpcOption(new NpcDialog(DialogType.QuestAccept, subOptionId, "Accept")));
                                break;
                            case QuestState.OnProgress:
                                character.SendPacket(new NpcStartTalk(quest.UnfinishedDialog));
                                break;
                            case QuestState.Completed:
                                character.SendPacket(new NpcStartTalk(quest.EndDialog));
                                character.SendPacket(new SendNpcOption(new NpcDialog(DialogType.QuestReward, subOptionId, "Reward")));
                                break;
                        }

                        character.SendPacket(new SendNpcOption(new NpcDialog(DialogType.Quest, 0, "Menu")));
                        character.SendPacket(new SendNpcOption(new NpcDialog(DialogType.ChatClose, 0, "Close")));
                    }
                        break;
                    case DialogType.Talk:
                        break;
                    case DialogType.Teleport:
                        break;
                }
            }
            else
            {
                character.SendPacket(new ResetChatOptions());
                character.SendPacket(new CloseNpcChat());

                // TODO - CHECK IF CAN OPEN
                switch (optionId)
                {
                    case DialogType.Store:
                    {
                        if (npc.StoreItems != null && npc.StoreType == StoreType.Normal)
                        {
                            character.SendPacket(new NpcStoreOpen(npc.Id, npc.StoreType, npc.StoreItems));
                            character.OpenedShopType = ShopType.Store;
                            character.OpenedShopNpcConId = npc.Id;
                        }

                        Log.Debug("Store: NpcId: {0}, StoreType: {1}", npc.NpcId, npc.StoreType);
                    }
                        break;
                    case DialogType.StoreSkill:
                    {
                        // TODO - THIS STORE CHANGES DYNAMICALLY
                        // This is just placeholder
                        if (npc.StoreItems != null && npc.StoreType != StoreType.Normal)
                        {
                            character.SendPacket(new NpcStoreOpen(npc.Id, npc.StoreType, npc.StoreItems));
                            character.OpenedShopType = ShopType.SkillShop;
                            character.OpenedShopNpcConId = npc.Id;
                        }

                        Log.Debug("Store: NpcId: {0}, StoreType: {1}", npc.NpcId, npc.StoreType);
                    }
                        break;
                    case DialogType.Bank:
                        character.Inventory.SendBankData();
                        OpenShop(character, ShopType.Bank, npc.Id);
                        break;
                    case DialogType.PranStation:
                        character.Inventory.SendBankData();
                        OpenShop(character, ShopType.PranStation, npc.Id);
                        break;
                    case DialogType.GuildCreate:
                        character.SendPacket(new CreateGuildBox(character.Connection.Id, 0));
                        break;
                    case DialogType.GuildBank:
                        break;
                    case DialogType.DialogEnd:
                        break;
                    case DialogType.Fortification:
                        OpenShop(character, ShopType.Fortification, npc.Id);
                        break;
                    case DialogType.Enchant:
                        OpenShop(character, ShopType.Enchant, npc.Id);
                        break;
                    case DialogType.ChangeNation:
                        break;
                    case DialogType.QuestMenu:
                        break;
                    case DialogType.QuestAccept:
                    {
                        var questData = npc.GetQuest((ushort) subOptionId);
                        if (npc.IsQuestAvailable(questData, character) != QuestState.Available) return;

                        var quest = character.Quests.AddQuest(questData);
                        if (quest != null)
                        {
                            character.SendPacket(new SendMessage(new Message($"You have accepted the mission '{questData.Name}'")));
                            character.SendPacket(new SendQuestInfo(quest));
                            character.SendPacket(new SetEffectOnHead(npc.Id, EffectType.QuestOngoing));
                            character.SendPacket(new PlaySound(446, SoundType.NpcVoice));
                        }
                    }
                        break;
                    case DialogType.QuestReward:
                    {
                        var questData = npc.GetQuest((ushort) subOptionId);
                        if (npc.IsQuestAvailable(questData, character) != QuestState.Completed) return;

                        character.Quests.AddReward(questData.Id);
                    }
                        break;
                    case DialogType.SaveLocation:
                        break;
                    case DialogType.EnterInstance:
                        break;
                    case DialogType.Repair:
                        OpenShop(character, ShopType.Repair, npc.Id);
                        break;
                    case DialogType.RepairAll:
                        OpenShop(character, ShopType.RepairAll, npc.Id);
                        break;
                    case DialogType.BlessFree:
                        break;
                    case DialogType.Upgrade:
                        break;
                    case DialogType.QuestReward2:
                        break;
                    case DialogType.NationStatus:
                        break;
                    case DialogType.GuildSkills:
                        break;
                    case DialogType.PranCostumeEnchant:
                        OpenShop(character, ShopType.PranCostumeEnchant, npc.Id);
                        break;
                    case DialogType.BlessPaid:
                        break;
                    case DialogType.Evolution:
                        OpenShop(character, ShopType.Evolution, npc.Id);
                        break;
                    case DialogType.BattleField:
                        break;
                    case DialogType.TeleportDisckeroa:
                    {
                        character.TeleportTo(1893.4f, 3787.4f);
                        Log.Debug("TeleportTo: Disckeroa");
                    }
                        break;
                    case DialogType.MoveToWar:
                        break;
                    case DialogType.RegisterWar:
                        break;
                    case DialogType.StoneRefinement:
                        OpenShop(character, ShopType.StoneRefinement, npc.Id);
                        break;
                    case DialogType.StoneEnchant:
                        OpenShop(character, ShopType.StoneEnchant, npc.Id);
                        break;
                    case DialogType.StoneCombination:
                        OpenShop(character, ShopType.StoneCombination, npc.Id);
                        break;
                    case DialogType.Craft:
                        OpenShop(character, ShopType.Craft, npc.Id);
                        break;
                    case DialogType.Dismantle:
                        OpenShop(character, ShopType.Dismantle, npc.Id);
                        break;
                    case DialogType.Transfer:
                        OpenShop(character, ShopType.Transfer, npc.Id);
                        break;
                    case DialogType.LevelDown:
                        OpenShop(character, ShopType.LevelDown, npc.Id);
                        break;
                    case DialogType.ChatClose:
                        break;
                    default:
                        return;
                }
            }
        }

        public static void CloseShop(Character character, ShopType type)
        {
            if (type != ShopType.ChangePranHair && character.OpenedShopType != type)
            {
                character.Connection.Close();
                Log.Warn("Character: '{0}'. Opened store dont match with closed store!", character.Name);
            }
            else
            {
                character.OpenedShopType = ShopType.None;
                character.OpenedShopNpcConId = 0;
            }
        }

        private static void OpenShop(Character character, ShopType type, uint npcConId)
        {
            character.SendPacket(new OpenNpcShop(type));
            character.OpenedShopType = type;
            character.OpenedShopNpcConId = npcConId;
            Log.Debug("Character: {0}, ShopType: {1}", character.Name, type);
        }
    }
}