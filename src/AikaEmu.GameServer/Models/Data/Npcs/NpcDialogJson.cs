using System.Collections.Generic;
using AikaEmu.GameServer.Models.Units.Npc.Const;

namespace AikaEmu.GameServer.Models.Data.Npcs
{
    public class NpcDialogJson
    {
        public uint NpcId { get; set; }
        public uint SoundId { get; set; }
        public SoundType SoundType { get; set; }
        public List<NpcDialogData> DialogData { get; set; }

        public NpcDialogJson()
        {
            DialogData = new List<NpcDialogData>();
        }
    }

    public class NpcDialogData
    {
        public DialogType OptionId { get; set; }
        public uint SubOptionId { get; set; }
        public string Text { get; set; }
        public int Unk { get; set; }
    }
}