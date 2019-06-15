using System;
using AikaEmu.AuthServer.Managers.Connections;
using AikaEmu.Shared.Model.Network;
using AikaEmu.Shared.Network;
using AikaEmu.Shared.Network.Encryption;
using NLog;

namespace AikaEmu.AuthServer.Network.AuthServer
{
    public class AuthProtocol : BaseProtocol
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();

        public override void OnConnect(Session session)
        {
            _log.Info("Client ({0}) connected with SessionId: {1}.", session.Ip.ToString(), session.Id.ToString());
            try
            {
                AuthConnManager.Instance.Add(new AuthConnection(session));
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
                var con = AuthConnManager.Instance.GetConnection(session.Id);
                if (con != null)
                    AuthConnManager.Instance.Remove(session.Id);
            }
            catch (Exception e)
            {
                session.Close();
                _log.Error(e);
            }

            _log.Info("Client ({0}) disconnected.", session.Ip.ToString());
        }

        public override void OnReceive(Session session, byte[] buff, int bytes)
        {
            var connection = AuthConnManager.Instance.GetConnection(session.Id);
            if (connection == null) return;

            try
            {
                // TODO - Better fix for 11 F3 11 1F at start of the packet 
                if (buff.Length > 2 && buff[0] == 0x11 && buff[1] == 0xF3)
                {
                    var newBuff = new byte[buff.Length - 4];
                    Array.Copy(buff, 4, newBuff, 0, buff.Length - 4);
                    buff = newBuff;
                }

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

                            if (Enum.IsDefined(typeof(ClientOpcode), opcode))
                            {
                                var pName = Enum.GetName(typeof(ClientOpcode), opcode);
                                var pType = Type.GetType($"AikaEmu.AuthServer.Packets.Client.{pName}");
                                var packet = (AuthPacket) Activator.CreateInstance(pType);
                                packet.Opcode = opcode;
                                packet.Connection = connection;
                                _log.Debug("C->Auth: {0:x2} {1}", opcode, pName);
                                packet.Decode(stream);
                            }
                            else
                            {
                                _log.Error("Opcode not found: {0} (0x{1:x2})", connection.Ip, opcode);
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