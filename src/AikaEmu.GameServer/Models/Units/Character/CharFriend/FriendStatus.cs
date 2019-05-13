namespace AikaEmu.GameServer.Models.Units.Character.CharFriend
{
    public enum FriendStatus : byte
    {
        OnlineBlocked = 1,
        Offline = 2,
        Online = 3,
        OfflineBlocked = 4, // TODO - maybe is 0????
    }
}