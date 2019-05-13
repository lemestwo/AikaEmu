using AikaEmu.GameServer.Models.Units.Character;

namespace AikaEmu.GameServer.Models
{
    public class Trade
    {
        public uint Id { get; set; }
        public Character Owner { get; set; }
        public Character Target { get; set; }
        public bool OwnerOk { get; set; }
        public bool TargetOk { get; set; }
        public bool OwnerConfirm { get; set; }
        public bool TargetConfirm { get; set; }

        public Trade()
        {
            OwnerOk = false;
            TargetOk = false;
            OwnerConfirm = false;
            TargetConfirm = false;
        }
    }
}