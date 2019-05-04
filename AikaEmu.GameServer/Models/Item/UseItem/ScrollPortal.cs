using AikaEmu.GameServer.Managers;
using AikaEmu.GameServer.Models.Data;
using AikaEmu.GameServer.Models.Units.Character;
using NLog;

namespace AikaEmu.GameServer.Models.Item.UseItem
{
    public class ScrollPortal : IUseItem
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();

        public void Execute(Character character, Item item, int data)
        {
            var (x, y) = DataManager.Instance.SPositionData.GetPosition((ushort) data, TpLevel.BasicScroll);
            if (x == 0 || y == 0) return;

            // TODO - ITEM COUNT
            character.TeleportTo(x, y);
        }
    }
}