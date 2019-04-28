using AikaEmu.GameServer.Models.NpcM;
using AikaEmu.GameServer.Network;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class OpenNpcShop : GamePacket
    {
        private readonly ShopType _shoptype;

        public OpenNpcShop(ShopType shoptype)
        {
            _shoptype = shoptype;

            Opcode = (ushort) GameOpcode.OpenNpcShop;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write((uint) _shoptype);
            return stream;
        }
    }
}