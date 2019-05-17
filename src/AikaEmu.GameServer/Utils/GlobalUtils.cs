using System.Drawing;

namespace AikaEmu.GameServer.Utils
{
    public static class GlobalUtils
    {
        public static Color GetColorFromString(string color)
        {
            return Color.FromArgb(int.Parse(color.Replace("#", ""), System.Globalization.NumberStyles.AllowHexSpecifier));
        }
    }
}