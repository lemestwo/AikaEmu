using System;
using AikaEmu.Shared.Network;
using AikaEmu.Shared.Network.Encryption;
using AikaEmu.Shared.Network.Packets;

namespace AikaEmu.GameServer.Network.GameServer
{
    public class GamePacket : BasePacket
    {
        public GameConnection Connection { protected get; set; }

        public override PacketStream Encode()
        {
            var stream = new PacketStream();
            try
            {
                var packet = new PacketStream().Write(0u).Write((short) 0).Write(Opcode).Write(62512);
                packet.Write(this);
                stream.Write(packet);
            }
            catch (Exception e)
            {
                Log.Fatal(e);
                throw;
            }

            stream.Replace(stream.Buffer, 0, stream.Count);
            var buffer = stream.Buffer;
            Encryption.Encrypt(ref buffer, buffer.Length);
            stream.Replace(buffer);
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