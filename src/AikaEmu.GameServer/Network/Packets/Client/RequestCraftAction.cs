using AikaEmu.GameServer.Helpers;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Client
{
    public class RequestCraftAction : GamePacket
    {
        protected override void Read(PacketStream stream)
        {
            var parameter = stream.ReadUInt16();
            stream.ReadUInt16();
            var quantity = stream.ReadUInt32();
            var unk = stream.ReadUInt32();

            if (parameter > 79)
            {
                CraftHelper.AnvilCraft(Connection, parameter, quantity);
            }
            else
            {
                var slot = parameter;
                // dismantle
                // TODO - NEED FIND WHERE ARE THE VALUES OF DISMANTLE
            }

            // unk = 1 = dismantle
            // 0 = anvil craft
            // return 0x302e
        }
    }
}