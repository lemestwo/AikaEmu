using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class UpdateNationGuild : GamePacket
    {
        public UpdateNationGuild()
        {
            Opcode = (ushort) GameOpcode.UpdateNationGuild;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(1); // conId
            stream.Write((short) 0); // nation id
            stream.Write((short) 0); // guild logo index
            stream.Write(0); // +20 unk (short) guild id?
            stream.Write("GuildName", 20); // guild name
            return stream;
        }
    }
}