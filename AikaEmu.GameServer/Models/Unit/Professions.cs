namespace AikaEmu.GameServer.Models.Unit
{
    public enum Professions : ushort
    {
        Undefined = 0,
        
        // Characters
        Warrior = 1,
        Conquerer = 2,
        Paladin = 11,
        Templar = 12,
        Rifleman = 21,
        Sniper = 22,
        DualGunner = 31,
        Pistoleer = 32,
        Warlock = 41,
        Arcanist = 42,
        Cleric = 51,
        Saint = 52,
        
        // Prans
        FirePrFairy = 61,
        FirePrChild = 62,
        FirePrTeenager = 63,
        FirePrAdult = 64,
        WaterPrFairy = 71,
        WaterPrChild = 72,
        WaterPrTeenager = 73,
        WaterPrAdult = 74,
        AirPrFairy = 81,
        AirPrChild = 82,
        AirPrTeenager = 83,
        AirPrAdult = 84,
        
        // Unk
        Unk = 91
    }
}