using AikaEmu.GameServer.Managers;
using AikaEmu.GameServer.Models.Chat;
using AikaEmu.GameServer.Models.Data.SqlModel.Const;
using AikaEmu.GameServer.Models.Units.Character;
using AikaEmu.GameServer.Network.Packets.Game;
using NLog;

namespace AikaEmu.GameServer.Models.Item.UseItem
{
    public class ScrollPortal : IUseItem
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();

        public void Execute(Character character, Item item, int data)
        {
            var region = DataManager.Instance.MapsData.GetTeleportPosition((ushort) data, TpLevel.BasicScroll);
            if (region == null) return;


            // TODO - ITEM COUNT
            character.TeleportTo(region.TpX + .44f, region.TpY + .44f);
            character.SendPacket(new SendMessage(new Message("Teleported to [" + region.Name + "].")));
        }
    }
}