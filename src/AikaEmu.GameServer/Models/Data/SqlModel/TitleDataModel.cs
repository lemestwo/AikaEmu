using System.Collections.Generic;
using System.Drawing;

namespace AikaEmu.GameServer.Models.Data.SqlModel
{
    public class TitleDataModel
    {
        public ushort Id { get; set; }
        public ushort Idx { get; set; }
        public byte Level { get; set; }
        public ushort UnkId1 { get; set; }
        public ushort UnkId2 { get; set; }
        public uint Requires { get; set; }
        public Effect Effect1 { get; set; }
        public Effect Effect2 { get; set; }
        public Effect Effect3 { get; set; }
        public string Desc { get; set; }
        public ushort Unk { get; set; }
        public Color Color { get; set; }
    }
}