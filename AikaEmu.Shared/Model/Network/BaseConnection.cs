using System.Net;
using AikaEmu.Shared.Network;

namespace AikaEmu.Shared.Model.Network
{
    public abstract class BaseConnection
    {
        protected readonly Session Session;

        public uint SessionId => Session.Id;
        public IPAddress Ip => Session.Ip;

        protected BaseConnection(Session session)
        {
            Session = session;
        }

        public virtual void Close()
        {
            Session?.Close();
        }

        public void AddAttribute(string name, object value)
        {
            Session.AddAttribute(name, value);
        }
    }
}