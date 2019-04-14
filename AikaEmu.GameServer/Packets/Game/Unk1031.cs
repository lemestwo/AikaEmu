using System;
using AikaEmu.GameServer.Network;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Packets.Game
{
    public class Unk1031 : GamePacket
    {
        public Unk1031()
        {
            Opcode = (ushort) GameOpcode.Unk1031;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(uint.MaxValue);
            stream.Write(0);
            return stream;
        }
    }
}