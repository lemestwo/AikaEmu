namespace AikaEmu.GameServer.Models.Units.Npc.Const
{
    public enum ShopType : uint
    {
        None = 0,
        Bank = 1,
        PranStation = 2,
        Craft = 4,
        Fortification = 5,
        Enchant = 6,
        Transfer = 7,
        LevelDown = 8,
        ChangeNation = 9,
        ChangePranHair = 12,
        ConfigCastle = 13,
        Repair = 14,
        RepairAll = 15,
        Dismantle = 17,

        // When closed sends a StoreType = 26
        // So it can conflict and disconnect the char
        // Seens like it should not be used anymore
        AuctionHouseOld = 18,

        JoinCastleWar = 19,
        BattleFieldLobby = 21,
        EnchantMount = 23,

        Unk24 = 24, // only opens inventory

        AuctionHouse = 26,
        Store = 28,
        SkillShop = 29,
        PranJokempo = 33,
        PranInteract = 34,
        PranCostumeEnchant = 35,
        Evolution = 38,
        RumbleWar = 39,
        StoneRefinement = 40,
        StoneEnchant = 41,
        StoneCombination = 42,
    }
}