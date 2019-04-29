using System.Collections.Generic;
using AikaEmu.GameServer.Models.Data.Npcs;
using AikaEmu.GameServer.Models.NpcM.Dialog;
using AikaEmu.GameServer.Models.Sound;
using AikaEmu.GameServer.Models.Unit;

namespace AikaEmu.GameServer.Models.NpcM
{
    public class Npc : BaseUnit
    {
        public ushort NpcId { get; set; }
        public ushort Hair { get; set; }
        public ushort Face { get; set; }
        public ushort Helmet { get; set; }
        public ushort Armor { get; set; }
        public ushort Gloves { get; set; }
        public ushort Pants { get; set; }
        public ushort Weapon { get; set; }
        public ushort Shield { get; set; }
        public byte[] Refinements { get; set; }
        public ushort Unk { get; set; }
        public byte SpawnType { get; set; }
        public ushort UnkId { get; set; }
        public short Unk2 { get; set; }
        public string Title { get; set; }
        public ushort Unk3 { get; set; }

        public uint SoundId { get; set; }
        public SoundType SoundType { get; set; }

        public List<NpcDialogData> DialogList { get; set; }
        
        public StoreType StoreType { get; set; }
        public ushort[] StoreItems { get; set; }

        public Npc()
        {
            DialogList = new List<NpcDialogData>();
        }

        public NpcDialogData GetDialog(DialogType type)
        {
            foreach (var dialog in DialogList)
            {
                if (dialog.OptionId == type) return dialog;
            }

            return null;
        }
    }
}