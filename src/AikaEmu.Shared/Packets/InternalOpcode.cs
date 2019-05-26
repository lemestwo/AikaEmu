namespace AikaEmu.Shared.Packets
{
    public enum InternalOpcode : ushort
    {
        RegisterGs = 0x01,
        AuthEnterGame = 0x02,
        RequestEnterResult = 0x03,
        ResetAuthHash = 0x04,
    }
}