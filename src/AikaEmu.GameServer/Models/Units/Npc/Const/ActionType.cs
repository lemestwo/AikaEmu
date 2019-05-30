namespace AikaEmu.GameServer.Models.Units.Npc.Const
{
    public enum ActionType : uint
    {
        /*
         * 4
         * 5
         * 17
         * 18
         * 19
         * 20
         * 21
         * 22
         * 23
         * 24
         * 25
         * 26
         *
         * 6-7-8-9-15-16-17+
         *
         * 10
         */
        Reinforcement = 0,
        Enchant = 1,
        LevelDown = 3,
        Craft = 4,
        Dismantle = 5,
        PranCostumeEnchant = 13,
        StoneRefinement = 23,
        StoneEnchant = 24,
    }
}