using System.Collections.Generic;

namespace AikaEmu.GameServer.Models.Data.SqlModel
{
    public class SetDataModel
    {
        public ushort Id { get; set; }
        public string Name { get; set; }
        public string Name2 { get; set; }
        public ushort Unk { get; set; }
        public HashSet<ushort> Items { get; set; }
        public List<SetEffectData> Effects { get; set; }
    }

    public class SetEffectData
    {
        public byte Count { get; set; }
        public bool IsSkill { get; set; }
        public byte Chance { get; set; }
        public ushort EffectId { get; set; }
        public ushort EffectValue { get; set; }
    }
}