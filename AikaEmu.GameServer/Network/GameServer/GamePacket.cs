using System;
using AikaEmu.Shared.Model.Network;
using AikaEmu.Shared.Network;
using AikaEmu.Shared.Network.Encryption;

namespace AikaEmu.GameServer.Network.GameServer
{
    public class GamePacket : BasePacket
    {
        private ushort _changedConnectionId = 0;
        private bool _isConnectionIdChanged = false;
        public GameConnection Connection { protected get; set; }

        protected ushort ChangeConnectionId
        {
            set
            {
                _changedConnectionId = value;
                _isConnectionIdChanged = true;
            }
        }

        public PacketStream Encode()
        {
            var stream = new PacketStream();
            try
            {
                var conId = _isConnectionIdChanged ? _changedConnectionId : Connection.ConnectionId;
                var packet = new PacketStream().Write(0).Write(conId).Write(Opcode).Write(Time);
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