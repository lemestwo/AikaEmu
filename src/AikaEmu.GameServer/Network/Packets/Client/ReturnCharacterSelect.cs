using AikaEmu.GameServer.Managers;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Client
{
    public class ReturnCharacterSelect : GamePacket
    {
        protected override void Read(PacketStream stream)
        {
            if (Connection.ActiveCharacter != null)
            {
                Connection.ActiveCharacter.Save();
                Connection.ActiveCharacter.Friends?.GetOffline();
                WorldManager.Instance.Despawn(Connection.ActiveCharacter);
            }

            Connection.Account.SendCharacterList();
        }
    }
}