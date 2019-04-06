using System.Net;
using AikaEmu.Shared.Network.Packets;

namespace AikaEmu.Shared.Network
{
    public abstract class BaseConnection
    {
        protected readonly Session Session;

        public uint SessionId => Session.Id;
        public IPAddress SessionIp => Session.Ip;

        protected BaseConnection(Session session)
        {
            Session = session;
        }

        public void Close()
        {
            Session?.Close();
        }

        public void AddAttribute(string name, object value)
        {
            Session.AddAttribute(name, value);
        }

        public virtual void OnConnect()
        {
        }
    }
}