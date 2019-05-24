using System.Drawing;
using AikaEmu.GameServer.Models.Item.Const;

namespace AikaEmu.GameServer.Utils
{
    public static class GlobalUtils
    {
        public static Color GetColorFromString(string color)
        {
            return Color.FromArgb(int.Parse(color.Replace("#", ""), System.Globalization.NumberStyles.AllowHexSpecifier));
        }

        public static bool IsEquipment(ItemType itemType)
        {
            return itemType > ItemType.MonsterWeapon && itemType <= ItemType.Staff ||
                   itemType >= ItemType.Hair && itemType <= ItemType.Shield ||
                   itemType >= ItemType.Ring && itemType <= ItemType.Necklace;
        }
    }
}