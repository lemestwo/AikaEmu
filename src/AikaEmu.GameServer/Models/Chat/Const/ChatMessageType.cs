namespace AikaEmu.GameServer.Models.Chat.Const
{
    public enum ChatMessageType : short
    {
        Normal = 0, // -1
        Whisper = 1, // -26113
        Party = 2, // -4469505
        Guild = 3, // -6684775
        Shout = 4, // -23977
        Alliance = 5, // -21863
        Government = 6, // -1614513
        Gm = 7, // -1709
        Normal2 = 8, // -1 - Should not be used I guess
        Unk9 = 9, // -966849
        Unk10 = 10 //-966849
    }
}