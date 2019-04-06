using System;
using AikaEmu.Shared.Network;
using AikaEmu.Shared.Network.Encryption;
using AikaEmu.Shared.Network.Packets;

namespace AikaEmu.AuthServer.Network.AuthServer
{
    public class AuthPacket : BasePacket
    {
        public AuthConnection Connection { protected get; set; }

        public override PacketStream Encode()
        {
            var stream = new PacketStream();
            try
            {
                var opcodeByte = BitConverter.GetBytes(Opcode);
                var header = new byte[] {0x0, 0x0, 0x0, 0x0, 0x0, 0x0, opcodeByte[0], opcodeByte[1], 0x3C, 0x5E, 0x5F, 0x70};
                stream.Write(header);
                stream.Write(new PacketStream().Write(this));
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