namespace AikaEmu.GameServer.Network
{
    public enum GameOpcode : ushort
    {
        UpdateHpMp = 0x1003,
        UpdateStoreItem = 0x1006,
        UpdateLevel = 0x1008,
        UpdateAttributes = 0x1009,
        UpdateStatus = 0x100A,
        SetEffectOnHead = 0x100D,
        PranEffect = 0x1017,
        UpdatePremiumStash = 0x1038,
        XTrap = 0x1039,
        UpdateAccountLevel = 0x104F,
        SendCharacterList = 0x1801,
        SendToWorld = 0x1825,
        UpdateDungeonTimer = 0x184C,
        SendMessage = 0x1884,
        SendQuestInfo = 0x3031,
        SendUnitSpawn = 0x3049,
        SendSpawnMob = 0x305E,
        SendTokenResult = 0x309D,
        UpdateItem = 0x3C0E,
        ResponseDeleteCharToken = 0x3F33,
        
        Unk101F = 0x101F,
        Unk102C = 0x102C,
        Unk1028 = 0x1028,
        Unk1031 = 0x1031,
        Unk106F = 0x106F,
        Unk1086 = 0x1086,
        Unk1C41 = 0x1C41,
        Unk2027 = 0x2027,
        Unk303D = 0x303D,
        Unk3057 = 0x3057,
        Unk30A2 = 0x30A2,
        Unk30A5 = 0x30A5,
        Unk30A6 = 0x30A6,
        Unk3CBE = 0x3CBE,
        Unk3F1B = 0x3F1B,
        Unk3F34 = 0x3F34,
        Unk4756 = 0x4756,
    }

    public enum ClientOpcode : ushort
    {
        ReturnCharacterSelect = 0x2468,
        RequestEnterGame = 0x24C1,
        CreateCharacter = 0x2F04,
        RequestTokenResult = 0x309D,
        RequestToken = 0x3C02,
        UnkAtEnterWork = 0x3cbe,
        RequestDeleteCharToken = 0x3F33,
        RequestDeleteChar = 0x2403,
    }
}