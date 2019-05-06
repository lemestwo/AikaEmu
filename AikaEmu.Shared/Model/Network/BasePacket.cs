using System;
using AikaEmu.Shared.Network;
using NLog;

namespace AikaEmu.Shared.Model.Network
{
    public abstract class BasePacket
    {
        protected readonly Logger Log = LogManager.GetCurrentClassLogger();

        public ushort Opcode { get; set; }

        protected static uint Time
        {
            get
            {
                var time = new TimeSpan(DateTime.Now.Ticks - (new DateTime(2001, 1, 1)).Ticks);
                return (uint) time.TotalSeconds;
            }
        }

        protected virtual void Read(PacketStream stream)
        {
        }

        public virtual PacketStream Write(PacketStream stream)
        {
            return stream;
        }
    }
}