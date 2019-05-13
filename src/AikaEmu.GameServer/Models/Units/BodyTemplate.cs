using System.Collections.Generic;

namespace AikaEmu.GameServer.Models.Units
{
    public class BodyTemplate
    {
        public byte Width { get; set; }
        public byte Chest { get; set; }
        public byte Leg { get; set; }
        public byte Body { get; set; }


        public BodyTemplate()
        {
        }

        public BodyTemplate(IReadOnlyList<byte> temp)
        {
            Width = temp[0];
            Chest = temp[1];
            Leg = temp[2];
            Body = temp[3];
        }
    }
}