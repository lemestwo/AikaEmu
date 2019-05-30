using AikaEmu.GameServer.Managers;
using AikaEmu.GameServer.Models.Account;
using AikaEmu.GameServer.Models.Units.Character;
using AikaEmu.Shared.Model.Network;
using AikaEmu.Shared.Network;
using NLog;

namespace AikaEmu.GameServer.Network.GameServer
{
    public class GameConnection : BaseConnection
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();
        public Account Account { get; set; }
        public Character ActiveCharacter => Account.ActiveCharacter;
        public ushort Id { get; set; }

        public GameConnection(Session session) : base(session)
        {
        }

        public void OnDisconnect()
        {
            AccountManager.Instance.RemoveAccount(Id);

            if (ActiveCharacter == null) return;

            if (!ActiveCharacter.IsInternalDisconnect)
                ActiveCharacter.Save();

            ActiveCharacter.Friends.GetOffline();
            WorldManager.Instance.Despawn(ActiveCharacter);
        }

        public void SendPacket(GamePacket packet)
        {
            packet.Connection = this;
            Session.SendPacket(packet.Encode());
            if (packet.Opcode != 0x30bf && packet.Opcode != 0x1001 && packet.Opcode != 0x3049)
                _log.Debug("S->Client: (0x{0:x2}) {1}.", packet.Opcode, (GameOpcode) packet.Opcode);
        }
    }
}