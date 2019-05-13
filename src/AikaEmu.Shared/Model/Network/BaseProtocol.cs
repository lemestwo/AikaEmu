using AikaEmu.Shared.Network;

namespace AikaEmu.Shared.Model.Network
{
    public abstract class BaseProtocol
    {
        public virtual void OnConnect(Session session)
        {
        }

        public virtual void OnReceive(Session session, byte[] buff, int bytes)
        {
        }

        public virtual void OnSend(Session session, byte[] buff, int offset, int bytes)
        {
        }

        public virtual void OnDisconnect(Session session)
        {
        }
    }
}