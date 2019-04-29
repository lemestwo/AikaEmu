using AikaEmu.GameServer.Models.NpcM;
using AikaEmu.GameServer.Network;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class OpenNpcShop : GamePacket
    {
        private readonly ShopType _shopType;

        public OpenNpcShop(ShopType shopType)
        {
            _shopType = shopType;
            Opcode = (ushort) GameOpcode.OpenNpcShop;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write((uint) _shopType);
            return stream;
        }
    }
}