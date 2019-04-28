using System.ComponentModel;

namespace AikaEmu.GameServer.Models.NpcM.Dialog
{
    public enum DialogSubType3 : uint
    {
        [Description("Ursula")] Ursula = 1,
        [Description("Regenshein")] Regenshein = 2,
        [Description("Basilan Paid")] BasilanPaid = 9,
        [Description("Regenshein")] Regenshein2 = 23,
        [Description("Asteria [High]")] AsteriaHigh = 53,
        [Description("Asteria [Low]")] AsteriaLow = 54,
    }
}