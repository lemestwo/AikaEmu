using System;

namespace AikaEmu.GameServer.Models.Group
{
    public class PartyInvite
    {
        public ushort ConInviter { get; set; }
        public ushort ConInvited { get; set; }
        public DateTime Time { get; set; }
    }
}