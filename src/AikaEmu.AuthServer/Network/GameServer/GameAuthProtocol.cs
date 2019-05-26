using System;
using AikaEmu.AuthServer.Managers;
using AikaEmu.AuthServer.Managers.Connections;
using AikaEmu.Shared.Model.Network;
using AikaEmu.Shared.Network;
using AikaEmu.Shared.Packets;
using NLog;

namespace AikaEmu.AuthServer.Network.GameServer
{
    public class GameAuthProtocol : BaseProtocol
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();

        public override void OnConnect(Session session)
        {
            var template = new GameAuthConnection(session);
            InternalConnManager.Instance.Add(template);
            _log.Info("Gameserver ({0}) connected with SessionId: {1}.", session.Ip.ToString(), session.Id.ToString());
        }

        public override void OnDisconnect(Session session)
        {
            InternalConnManager.Instance.Remove(session.Id);
            var gsId = session.GetAttribute("gsId");
            if (gsId != null)
                AuthGameManager.Instance.Remove((byte) gsId);
            InternalConnManager.Instance.Remove(session.Id);
            _log.Info("Gameserver ({0}) disconnected.", session.Ip.ToString());
        }

        public override void OnReceive(Session session, byte[] buff, int bytes)
        {
            try
            {
                var connection = InternalConnManager.Instance.GetConnection(session.Id);
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

                    if (Enum.IsDefined(typeof(InternalOpcode), opcode))
                    {
                        var pName = Enum.GetName(typeof(InternalOpcode), opcode);
                        var pType = Type.GetType($"AikaEmu.AuthServer.Packets.GA.{pName}");
                        var packet = (GameAuthPacket) Activator.CreateInstance(pType);
                        packet.Opcode = opcode;
                        packet.Connection = connection;
                        packet.Decode(stream);
                        _log.Debug("Game->Auth: 0x{0:x2} {1}.", opcode, pName);
                    }
                    else
                    {
                        _log.Error("Opcode not found: {0} (0x{1:x2}).", connection.Ip, opcode);
                        _log.Error("Data: {0}", BitConverter.ToString(stream.ReadBytes(stream.Count - stream.Pos)));
                    }
                }
                else
                {
                    _log.Error("Packet with wrong size. ({0} >= {1}).", stream.Count, size);
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