using System.Drawing;
using AikaEmu.GameServer.Models.Data.SqlModel;

namespace AikaEmu.GameServer.Models.Units.Character.CharTitle
{
    public class Title
    {
        public ushort Id { get; set; }
        public byte Level { get; set; }
        public bool IsActive { get; set; }
        public TitleDataModel TitleData { get; set; }
    }
}