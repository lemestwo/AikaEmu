using System.Collections.Generic;
using AikaEmu.GameServer.Models.Group.Const;
using AikaEmu.GameServer.Models.Units.Character;

namespace AikaEmu.GameServer.Models.Group
{
    public class Party
    {
        public uint Id { get; set; }
        public Dictionary<byte, Character> Members { get; set; }
        public XpType XpType { get; set; }
        public LootType LootType { get; set; }
        public ushort LeaderConId { get; set; }

        public Party()
        {
            Members = new Dictionary<byte, Character>();
        }

        public byte GetNextSlot()
        {
            for (byte i = 0; i < 6; i++)
            {
                if (!Members.ContainsKey(i)) return i;
            }

            return 255;
        }
    }
}