using System;

namespace AikaEmu.GameServer.Models.Units.Character.CharFriend
{
    public class Friend : ICloneable
    {
        public uint Id { get; set; }
        public uint CharacterId { get; set; }
        public uint FriendId { get; set; }
        public string Name { get; set; }
        public bool IsBlocked { get; set; }

        public DateTime AddTime { get; set; }

        public void SwapIds(string name)
        {
            var c1 = CharacterId;
            var c2 = FriendId;
            CharacterId = c2;
            FriendId = c1;
            Name = name;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}