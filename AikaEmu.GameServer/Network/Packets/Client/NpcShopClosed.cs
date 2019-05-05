using AikaEmu.GameServer.Controller;
using AikaEmu.GameServer.Managers;
using AikaEmu.GameServer.Models.Units.Npc;
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
            NpcDialogController.CloseShop(Connection.ActiveCharacter, type);
        }
    }
}