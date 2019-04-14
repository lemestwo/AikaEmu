using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Packets.Client
{
    public class ReturnCharacterSelect : GamePacket
    {
        protected override void Read(PacketStream stream)
        {
            Connection.Account.SendCharacterList();
        }
    }
}