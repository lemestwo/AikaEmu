using NLog;

namespace AikaEmu.Shared.Network.Packets
{
    public abstract class BasePacket
    {
        protected readonly Logger Log = LogManager.GetCurrentClassLogger();

        public ushort Opcode { protected get; set; }

        public virtual void Read(PacketStream stream)
        {
        }

        public virtual PacketStream Write(PacketStream stream)
        {
            return stream;
        }

        public abstract PacketStream Encode();

        public abstract BasePacket Decode(PacketStream stream);
    }
}