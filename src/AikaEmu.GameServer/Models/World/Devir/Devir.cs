using System.Collections.Generic;
using AikaEmu.GameServer.Models.World.Nation;

namespace AikaEmu.GameServer.Models.World.Devir
{
    public class Devir
    {
        public NationId NationId { get; }
        public DevirId Id { get; }
        public Dictionary<byte, DevirSlot> Slots { get; set; }

        public Devir(NationId nationId, DevirId devirId)
        {
            NationId = nationId;
            Id = devirId;
            Slots = new Dictionary<byte, DevirSlot>();
        }
    }
}