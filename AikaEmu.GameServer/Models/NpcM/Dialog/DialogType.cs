using System.ComponentModel;

namespace AikaEmu.GameServer.Models.NpcM.Dialog
{
    public enum DialogType : uint
    {
        [Description("Talk")] Talk = 1,
        [Description("Quest")] Quest = 2,
        [Description("Teleport")] Teleport = 3,
        [Description("Store")] Store = 5,
        [Description("Bank")] Bank = 7,
        [Description("Close")] ChatClose = 8,
        [Description("Create Guild")] GuildCreate = 10,
        [Description("Guild Storage")] GuildBank = 11,
        [Description("End Dialog")] DialogEnd = 12,
        [Description("Pran Station")] PranStation = 13,
        [Description("Fortification")] Fortification = 16,
        [Description("Change Nation")] ChangeNation = 20,
        [Description("Menu")] QuestMenu = 21,
        [Description("Accept")] QuestAccept = 22,
        [Description("Reward")] QuestReward = 24,
        [Description("Save Location")] SaveLocation = 25,
        [Description("Enter Dungeon Ranking")] DungeonRanking = 26,
        [Description("Repair")] Repair = 31,
        [Description("Repair All")] RepairAll = 32,
        [Description("Bless Free")] BlessFree = 35,
        [Description("Upgrade")] Upgrade = 39,
        [Description("Reward")] QuestReward2 = 40,
        [Description("Nation Status")] NationStatus = 41,
        [Description("Guild Skills")] GuildSkills = 46,
        [Description("Upgrade Costume")] UpgradeCostume = 62,
        [Description("Bless Paid")] BlessPaid = 65,
        [Description("Evolution")] Evolution = 71,
    }
}