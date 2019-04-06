using System;
using AikaEmu.Shared.Network;
using AikaEmu.Shared.Network.Packets;
using NLog;
using static AikaEmu.GameServer.GameServer;

namespace AikaEmu.GameServer.Network.AuthServer
{
    public class AuthGameProtocol : BaseProtocol
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();

        public override void OnConnect(Session session)
        {
            var connection = new AuthGameConnection(session);
            connection.OnConnect();
            Instance.AuthGameConnection = connection;
            _log.Info("Connection to {0} established with SessionId: {1}.", session.Ip.ToString(), session.Id.ToString());
        }

        public override void OnDisconnect(Session session)
        {
            Instance.AuthGameConnection = null;
            session.Close();
            _log.Info("Connection with AuthServer is lost.");
        }

        public override void OnReceive(Session session, byte[] buff, int bytes)
        {
            try
            {
                var connection = Instance.AuthGameConnection;
                if (connection == null || buff.Length < 2) return;

                var stream = new PacketStream();
                stream.Insert(stream.Count, buff);

                var size = stream.ReadUInt16();
                if (stream.Count >= size)
                {
                    stream.Replace(stream.Buffer, 0, size);
                    stream.Pos = 0;

                    stream.ReadUInt16();
                    var opcode = stream.ReadUInt16();

                    if (Enum.IsDefined(typeof(GameAuthOpcode), opcode))
                    {
                        var pName = Enum.GetName(typeof(GameAuthOpcode), opcode);
                        var pType = Type.GetType($"AikaEmu.GameServer.Packets.Auth.{pName}");
                        var packet = (AuthGamePacket) Activator.CreateInstance(pType);
                        packet.Opcode = opcode;
                        packet.Connection = connection;
                        packet.Decode(stream);
                        _log.Debug("Auth->Game: {0:x2} {1}", opcode, pName);
                    }
                    else
                    {
                        _log.Error("Opcode not found: {0} (0x{1:x2})", connection.SessionIp, opcode);
                        _log.Error("Data: {0}", BitConverter.ToString(stream.ReadBytes(stream.Count - stream.Pos)));
                    }
                }
                else
                {
                    _log.Error("Packet with wrong size. ({0} >= {1})", stream.Count, size);
                }
            }
            catch (Exception e)
            {
                session?.Close();
                _log.Error(e);
            }
        }
    }
}