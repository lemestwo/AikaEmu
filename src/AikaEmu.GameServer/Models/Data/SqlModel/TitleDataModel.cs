using System.Collections.Generic;
using System.Drawing;

namespace AikaEmu.GameServer.Models.Data.SqlModel
{
    public class TitleDataModel
    {
        public ushort Id { get; set; }
        public ushort Idx { get; set; }
        public ushort UnkId { get; set; }
        public uint Requires { get; set; }
        public List<(ushort EffId, ushort EffValue)> Effects { get; set; }
        public string Desc { get; set; }
        public ushort Unk { get; set; }
        public Color Color { get; set; }
    }
}