using AikaEmu.GameServer.Models;
using AikaEmu.GameServer.Models.Chat;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.GameServer.Network.Packets.Game;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Client
{
    public class OpenMyPersonalStore : GamePacket
    {
        protected override void Read(PacketStream stream)
        {
            var conId = stream.ReadUInt16();
            stream.ReadUInt16();
            if (conId != Connection.Id) return;

            var personalStore = new PersonalStore(stream, Connection.ActiveCharacter);

            if (personalStore.Items.Count <= 0)
            {
                Connection.SendPacket(new SendMessage(new Message("Invalid personal store.")));
                return;
            }

            Connection.SendPacket(new SendUnitSpawn(Connection.ActiveCharacter));
            // TODO UNIT SPAWN 3 HEADEFFECT 1 ISSTORE
            Connection.SendPacket(new SendPersonalStore(personalStore));
        }
    }
}