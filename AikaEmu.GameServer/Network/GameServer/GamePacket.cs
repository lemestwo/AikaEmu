using System;
using AikaEmu.Shared.Model.Network;
using AikaEmu.Shared.Network;
using AikaEmu.Shared.Network.Encryption;

namespace AikaEmu.GameServer.Network.GameServer
{
    public class GamePacket : BasePacket
    {
        public GameConnection Connection { protected get; set; }

        public PacketStream Encode()
        {
            var stream = new PacketStream();
            try
            {
                var packet = new PacketStream().Write(0).Write(Connection.ConnectionId).Write(Opcode).Write(Time);
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