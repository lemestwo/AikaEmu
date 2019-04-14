using System;
using AikaEmu.Shared.Model.Network;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.AuthServer
{
    public class AuthGamePacket : BasePacket
    {
        public AuthGameConnection Connection { protected get; set; }

        public virtual PacketStream Encode()
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

        public virtual BasePacket Decode(PacketStream stream)
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