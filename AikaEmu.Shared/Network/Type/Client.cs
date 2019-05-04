using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using AikaEmu.Shared.Model.Network;
using NLog;

namespace AikaEmu.Shared.Network.Type
{
    public class Client : INetwork
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private const int BufferSize = 8096;
        private readonly IPEndPoint _remoteEndPoint;
        private readonly BufferControl _bufferControl;
        private readonly Socket _socket;
        private readonly BaseProtocol _protocol;

        public Client(IPEndPoint remoteEndPoint, BaseProtocol handler)
        {
            _remoteEndPoint = remoteEndPoint;
            _socket = new Socket(remoteEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            _bufferControl = new BufferControl(BufferSize, BufferSize);
            _bufferControl.Init();

            _protocol = handler;
        }

        public void Start()
        {
            StartConnect();
        }

        public void Stop()
        {
            if (_socket.Connected)
                _socket.Disconnect(true);
            _socket.Close();
            Log.Info("Client network stopped.");
        }

        private void StartConnect()
        {
            Log.Info("Connecting to {0}", _remoteEndPoint.ToString());
            var connectEventArg = new SocketAsyncEventArgs {RemoteEndPoint = _remoteEndPoint};
            connectEventArg.Completed += AcceptEventArg_Completed;
            var willRaiseEvent = _socket.ConnectAsync(connectEventArg);
            if (!willRaiseEvent)
                ProcessConnect(connectEventArg);
        }

        private void AcceptEventArg_Completed(object sender, SocketAsyncEventArgs e)
        {
            ProcessConnect(e);
        }

        private void ProcessConnect(SocketAsyncEventArgs e)
        {
            var errorCode = e.SocketError;
            switch (errorCode)
            {
                case SocketError.TimedOut:
                case SocketError.ConnectionRefused:
                    Log.Info("Connection to ({0}) failed, trying new attempt...", _remoteEndPoint);
                    Thread.Sleep(5000);
                    StartConnect();
                    return;
                case SocketError.Interrupted:
                case SocketError.OperationAborted:
                    return;
            }

            if (errorCode != SocketError.Success)
                throw new SocketException((int) errorCode);

            var readEventArg = new SocketAsyncEventArgs();
            readEventArg.Completed += ReadComplete;
            _bufferControl.Set(readEventArg);

            if (_socket?.RemoteEndPoint == null)
            {
                Log.Info("Connection to ({0}) failed, trying new attempt...", _remoteEndPoint);
                Thread.Sleep(10000);
                StartConnect();
                return;
            }

            var session = new Session(this, readEventArg, _socket);
            readEventArg.UserToken = session;

            _protocol.OnConnect(session);

            var willRaiseEvent = _socket.ReceiveAsync(readEventArg);
            if (!willRaiseEvent)
                ProcessReceive(readEventArg);
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
                var buf = new byte[e.BytesTransferred];
                Buffer.BlockCopy(e.Buffer, e.Offset, buf, 0, e.BytesTransferred);
                _protocol.OnReceive(session, buf, e.BytesTransferred);

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
                    Log.Error("Error on ProcessReceive: {0}", e.SocketError.ToString());
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

        public void OnReceive(Session session, byte[] buf, int bytes)
        {
            _protocol.OnReceive(session, buf, bytes);
        }

        public void OnSend(Session session, byte[] buf, int offset, int bytes)
        {
            _protocol.OnSend(session, buf, offset, bytes);
        }

        public void RemoveSession(Session session)
        {
            _bufferControl.Empty(session.ReadEventArg);
        }
    }
}