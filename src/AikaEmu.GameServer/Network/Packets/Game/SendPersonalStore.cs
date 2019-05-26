using AikaEmu.GameServer.Models;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class SendPersonalStore : GamePacket
    {
        private readonly PersonalStore _store;

        public SendPersonalStore(PersonalStore store)
        {
            _store = store;

            Opcode = (ushort) GameOpcode.SendPersonalStore;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(_store);
            return stream;
        }
    }
}