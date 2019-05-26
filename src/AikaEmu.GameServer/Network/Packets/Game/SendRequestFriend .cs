using AikaEmu.GameServer.Models.Units.Character;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class SendRequestFriend : GamePacket
    {
        private readonly Character _character;

        public SendRequestFriend(Character character)
        {
            _character = character;

            Opcode = (ushort) GameOpcode.SendRequestFriend;
            SenderId = 0;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write((uint) _character.Connection.Id); // conId?
            stream.Write(_character.Name, 16);
            return stream;
        }
    }
}