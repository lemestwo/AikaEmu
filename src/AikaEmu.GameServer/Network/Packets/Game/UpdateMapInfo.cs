using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class UpdateMapInfo : GamePacket
    {
        public UpdateMapInfo(ushort conId)
        {
            Opcode = (ushort) GameOpcode.UpdateMapInfo;
            SenderId = conId;
        }

        public override PacketStream Write(PacketStream stream)
        {
            // Zeelant War?
            stream.Write((short) 0);
            for (var i = 0; i < 5; i++)
                stream.Write((short) 0);

            stream.Write(ushort.MaxValue);
            stream.Write((short) 0);
            stream.Write(0);
            // stream.Write((short) 6456);
            // for (var i = 0; i < 5; i++)
            // {
            //     stream.Write((short) 1800);
            // }
            //
            // stream.Write(1);
            // stream.Write(1555905089);
            return stream;
        }
    }
}