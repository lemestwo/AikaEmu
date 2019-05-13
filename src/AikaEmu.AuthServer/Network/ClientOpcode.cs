// ReSharper disable InconsistentNaming

namespace AikaEmu.AuthServer.Network
{
    public enum ClientOpcode : ushort
    {
        AuthAccount = 0x0081,
    }

    public enum AuthOpcode : ushort
    {
        AuthSuccess = 0x0082,
    }
}