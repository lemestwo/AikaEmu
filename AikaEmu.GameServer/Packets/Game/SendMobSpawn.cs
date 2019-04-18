using AikaEmu.GameServer.Network;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Packets.Game
{
    public class SendMobSpawn : GamePacket
    {
        public SendMobSpawn()
        {
            Opcode = (ushort) GameOpcode.SendMobSpawn;
        }

        public override PacketStream Write(PacketStream stream)
        {
            // TODO
            return stream;
        }
    }
}