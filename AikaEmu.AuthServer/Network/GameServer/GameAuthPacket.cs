using System;
using AikaEmu.Shared.Network;
using AikaEmu.Shared.Network.Packets;

namespace AikaEmu.AuthServer.Network.GameServer
{
    public class GameAuthPacket : BasePacket
    {
        public GameAuthConnection Connection { protected get; set; }
        public override PacketStream Encode()
        {
            var stream = new PacketStream();
            try
            {
                var packet = new PacketStream().Write(Opcode).Write(this);
                stream.Write(packet, true);
            }
            catch (Exception e)
            {
                Log.Fatal(e);
                throw;
            }

            return stream;
        }

        public override BasePacket Decode(PacketStream stream)
        {
            try
            {
                Read(stream);
            }
            catch (Exception e)
            {
                Log.Fatal(e);
                throw;
            }

            return this;
        }
    }
}