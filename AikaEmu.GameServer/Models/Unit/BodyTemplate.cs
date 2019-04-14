using System.Collections.Generic;
using AikaEmu.Shared.Model.Network;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Models.Unit
{
    public class BodyTemplate : BasePacket
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

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(Width);
            stream.Write(Chest);
            stream.Write(Leg);
            stream.Write(Body);
            return stream;
        }
    }
}