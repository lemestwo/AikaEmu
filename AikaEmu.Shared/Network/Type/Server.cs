using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using AikaEmu.Shared.Model.Network;
using NLog;

namespace AikaEmu.Shared.Network.Type
{
    public class Server : INetwork
    {
        private static Logger _log = LogManager.GetCurrentClassLogger();

        private const int ReceiveBufferSize = 8096;
        private readonly BufferControl _bufferControl;
        private readonly Socket _listenSocket;
        private readonly Semaphore _maxNumberAcceptedClients;
        private readonly ConcurrentDictionary<uint, Session> _sessions;
        private readonly BaseProtocol _protocol;

        public bool IsStarted { get; private set; }

        public Server(EndPoint localEndPoint, int numConnections, BaseProtocol handler)
        {
            _listenSocket = new Socket(localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _listenSocket.Bind(localEndPoint);
            _listenSocket.Listen(100);

            _bufferControl = new BufferControl(ReceiveBufferSize * numConnections, ReceiveBufferSize);
            _maxNumberAcceptedClients = new Semaphore(numConnections, numConnections);
            _sessions = new ConcurrentDictionary<uint, Session>();
            _bufferControl.Init();

            _protocol = handler;
        }

        public void Start()
        {
            IsStarted = true;
            StartAccept(null);
        }

        public void Stop()
        {
            foreach (var session in _sessions.Values)
                session.Close();
            IsStarted = false;
            _listenSocket.Close();
        }

        private void StartAccept(SocketAsyncEventArgs acceptEventArg)
        {
            if (acceptEventArg == null)
            {
                acceptEventArg = new SocketAsyncEventArgs();
                acceptEventArg.Completed += AcceptEventArg_Completed;
            }
            else
            {
                acceptEventArg.AcceptSocket = null;
            }

            _maxNumberAcceptedClients.WaitOne();
            var willRaiseEvent = _listenSocket.AcceptAsync(acceptEventArg);
            if (!willRaiseEvent)
                ProcessAccept(acceptEventArg);
        }

        private void AcceptEventArg_Completed(object sender, SocketAsyncEventArgs e)
        {
            ProcessAccept(e);
        }

        private void ProcessAccept(SocketAsyncEventArgs e)
        {
            var readEventArg = new SocketAsyncEventArgs();
            readEventArg.Completed += ReadComplete;
            _bufferControl.Set(readEventArg);

            if (e.AcceptSocket?.RemoteEndPoint == null)
            {
                if (IsStarted)
                    StartAccept(e);
                return;
            }

            var session = new Session(this, readEventArg, e.AcceptSocket);
            readEventArg.UserToken = session;

            _sessions.TryAdd(session.Id, session);

            _protocol.OnConnect(session);

            var willRaiseEvent = e.AcceptSocket.ReceiveAsync(readEventArg);
            if (!willRaiseEvent)
                ProcessReceive(readEventArg);

            StartAccept(e);
        }

        private void ReadComplete(object sender, SocketAsyncEventArgs e)
        {
            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Receive:
                    ProcessReceive(e);
                    break;
                default:
                    throw new ArgumentException("The last operation completed on the socket was not a receive");
            }
        }

        private void ProcessReceive(SocketAsyncEventArgs e)
        {
            var session = (Session) e.UserToken;
            if (e.BytesTransferred > 0 && e.SocketError == SocketError.Success)
            {
                var buffer = new byte[e.BytesTransferred];
                Buffer.BlockCopy(e.Buffer, e.Offset, buffer, 0, e.BytesTransferred);
                _protocol.OnReceive(session, buffer, e.BytesTransferred);

                try
                {
                    var willRaiseEvent = session.Socket.ReceiveAsync(e);
                    if (!willRaiseEvent)
                        ProcessReceive(e);
                }
                catch (ObjectDisposedException)
                {
                    session.Close();
                }
            }
            else
            {
                if (e.SocketError != SocketError.Success && e.SocketError != SocketError.OperationAborted &&
                    e.SocketError != SocketError.ConnectionReset)
                    _log.Error("Error on ProcessReceive: {0}", e.SocketError.ToString());
                session.Close();
            }
        }

        public void OnConnect(Session session)
        {
            _protocol.OnConnect(session);
        }

        public void OnDisconnect(Session session)
        {
            _protocol.OnDisconnect(session);
        }

        public void OnReceive(Session session, byte[] buff, int bytes)
        {
            _protocol.OnReceive(session, buff, bytes);
        }

        public void OnSend(Session session, byte[] buff, int offset, int bytes)
        {
            _protocol.OnSend(session, buff, offset, bytes);
        }

        public void RemoveSession(Session session)
        {
            _bufferControl.Empty(session.ReadEventArg);

            _sessions.TryRemove(session.Id, out var val);
            if (val != null)
                _maxNumberAcceptedClients.Release();
        }
    }
}