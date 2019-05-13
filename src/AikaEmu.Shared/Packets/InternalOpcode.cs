namespace AikaEmu.Shared.Packets
{
    public enum GameAuthOpcode : ushort
    {
        RegisterGs = 0x01,
        AuthEnterGame = 0x02,
    }

    public enum AuthGameOpcode : ushort
    {
        RegisterGs = 0x01,
        AuthEnterGame = 0x02,
        RequestEnterResult = 0x03,
    }
}