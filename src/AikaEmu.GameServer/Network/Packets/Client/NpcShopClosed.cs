using AikaEmu.GameServer.Helpers;
using AikaEmu.GameServer.Models.Units.Npc.Const;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Client
{
    public class NpcShopClosed : GamePacket
    {
        protected override void Read(PacketStream stream)
        {
            var type = (ShopType) stream.ReadUInt32();
            NpcDialogHelper.CloseShop(Connection.ActiveCharacter, type);
        }
    }
}