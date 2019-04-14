using System;
using AikaEmu.Shared.Model.Network;
using AikaEmu.Shared.Network;
using AikaEmu.Shared.Network.Encryption;

namespace AikaEmu.AuthServer.Network.AuthServer
{
    public class AuthPacket : BasePacket
    {
        public AuthConnection Connection { protected get; set; }

        public PacketStream Encode()
        {
            var stream = new PacketStream();
            try
            {
                var packet = new PacketStream().Write(0).Write((ushort) 0).Write(Opcode).Write(Time);
                packet.Write(this);
                stream.Write(packet);
            }
            catch (Exception e)
            {
                Log.Fatal(e);
                throw;
            }

            var buffer = stream.Buffer;
            Encryption.Encrypt(ref buffer, buffer.Length);
            stream.Replace(buffer);
            return stream;
        }

        public BasePacket Decode(PacketStream stream)
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