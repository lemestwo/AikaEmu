namespace AikaEmu.GameServer.Models.Quest.Const
{
    public enum GetType
    {
        KillMob = 1, // Requirement
        Item = 2, // Requirement - Requisite - Reward - Remove - Choice - Misc
        LevelRange = 4, // Requisite
        Class = 5, // Requisite
        Unk6 = 6, // Requisite
        ExperienceGuild = 7, // Reward
        QuestRequire = 9, // Requisite
        Unk10 = 10, // Requisite - Reward
        ItemChoice = 13, // Reward 
        Experience = 14, // Requirement - Reward
        Gold = 15, // Requirement - Reward - Remove
        ClassChange = 16, // Reward
        Unk18 = 18, // Reward
        TalkTo = 19, // Requirement
        Pran = 21, // Reward
        Unk23 = 23, // Reward
        Unk26 = 26, // Reward
        Unk27 = 27, // Reward
        Unk29 = 29, // Requisite
        Use = 31, // Requirement
        Unk32 = 32, // Requisite
        Equip = 34, // Requirement
        MissionChoice = 37, // Choice
        ComeFrom = 38, // Choice
        Unk42 = 42, // Requisite
        Unk44 = 44, // Requirement
        Unk47 = 47, // Reward
        AfterMission = 48, // Requisite
        BattleFieldVictory = 51, // Requirement
        Unk55 = 55, // Requisite
        Unk56 = 56, // Requirement
        Unk60 = 60, // Reward
        SkillAcquire = 62, // Reward
        Unk63 = 63, // Requirement - Requisite
        Unk64 = 64, // Requisite
        Unk66 = 66, // Requirement
        Unk67 = 67, // Requisite
        Unk68 = 68, // Requirement
        Unk79 = 79, // Requisite - Reward
        Unk85 = 85, // Requisite
        Unk95 = 95, // Requisite
        Unk97 = 97, // Requirement
        Unk98 = 98, // Requisite
    }
}