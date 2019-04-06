using System;
using System.Text;
using AikaEmu.GameServer.Controllers;
using AikaEmu.Shared.Network;
using AikaEmu.Shared.Network.Encryption;
using NLog;

namespace AikaEmu.GameServer.Network.GameServer
{
    public class GameProtocol : BaseProtocol
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();

        public override void OnConnect(Session session)
        {
            try
            {
                GameConnections.Instance.Add(new GameConnection(session));
                _log.Info("Client {0} connected with SessionId: {1}.", session.Ip.ToString(), session.Id.ToString());
            }
            catch (Exception e)
            {
                session.Close();
                _log.Error(e);
            }
        }

        public override void OnDisconnect(Session session)
        {
            try
            {
                var connection = GameConnections.Instance.GetConnection(session.Id);
                if (connection == null) return;
                // ondisconnect
                GameConnections.Instance.Remove(session.Id);
            }
            catch (Exception e)
            {
                session.Close();
                _log.Error(e);
            }

            _log.Info("Client {0} disconnected.", session.Ip.ToString());
        }

        public override void OnReceive(Session session, byte[] buff, int bytes)
        {
            var connection = GameConnections.Instance.GetConnection(session.Id);
            if (connection == null) return;

            try
            {
                var stream = new PacketStream();
                stream.Insert(stream.Count, buff);
                if (stream.Count < 12) return;

                var size = stream.ReadUInt16();
                if (stream.Count >= size)
                {
                    stream.Replace(stream.Buffer, 0, size);
                    stream.Pos = 0;

                    var isDecrypted = false;
                    try
                    {
                        var newBuffer = stream.Buffer;
                        isDecrypted = Encryption.Decrypt(ref newBuffer, newBuffer.Length);
                        stream.Replace(newBuffer);
                        stream.Pos = 0;
                    }
                    catch (Exception e)
                    {
                        _log.Error(e);
                    }
                    finally
                    {
                        if (isDecrypted)
                        {
                            stream.ReadBytes(6);
                            var opcode = stream.ReadUInt16();
                            stream.ReadInt32();

                            if (Enum.IsDefined(typeof(GameOpcode), opcode))
                            {
                                var pName = Enum.GetName(typeof(GameOpcode), opcode);
                                var pType = Type.GetType($"AikaEmu.GameServer.Packets.Client.{pName}");
                                var packet = (GamePacket) Activator.CreateInstance(pType);
                                packet.Opcode = opcode;
                                packet.Connection = connection;
                                packet.Decode(stream);
                                _log.Debug("C->Game: {0:x2} {1}", opcode, pName);
                            }
                            else
                            {
                                _log.Error("Opcode not found: {0} (0x{1:x2})", connection.SessionIp, opcode);
                                _log.Error("Data: {0}", BitConverter.ToString(stream.ReadBytes(stream.Count - stream.Pos)));
                            }
                        }
                        else
                        {
                            _log.Error("Failed to decrypt packet.");
                        }
                    }
                }
                else
                {
                    _log.Error("Packet with wrong size. ({0} >= {1})", stream.Count, size);
                }
            }
            catch (Exception e)
            {
                connection?.Close();
                _log.Error(e);
            }
        }
    }
}