namespace AikaEmu.GameServer.Network
{
    public enum GameOpcode : ushort
    {
        RequestCharacterList = 0x24c1,
        
        SendCharacterList = 0x1801
    }
}