using System;
using AikaEmu.GameServer.Managers;
using AikaEmu.Shared.Model.Network;
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
                ConnectionManager.Instance.Add(new GameConnection(session));
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
                var connection = ConnectionManager.Instance.GetConnection(session.Id);
                if (connection == null) return;

                connection.OnDisconnect();
                ConnectionManager.Instance.Remove(session.Id);
            }
            catch (Exception e)
            {
                session.Close();
                _log.Error(e);
            }

            _log.Info("Client {0} disconnected.", session.Ip.ToString());
        }

        // TODO - Need complete overhaul
        public override void OnReceive(Session session, byte[] buff, int bytes)
        {
            var connection = ConnectionManager.Instance.GetConnection(session.Id);
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
                            stream.ReadInt32();
                            var sender = stream.ReadUInt16();
                            var opcode = stream.ReadUInt16();
                            stream.ReadInt32();

                            if (Enum.IsDefined(typeof(ClientOpcode), opcode))
                            {
                                var pName = Enum.GetName(typeof(ClientOpcode), opcode);
                                var pType = Type.GetType($"AikaEmu.GameServer.Network.Packets.Client.{pName}");
                                var packet = (GamePacket) Activator.CreateInstance(pType);
                                packet.Opcode = opcode;
                                packet.Connection = connection;
                                packet.SenderId = sender;

                                if (opcode != 0x30bf && opcode != 0x3005 && opcode != 0x3006)
                                    _log.Debug("C->Game: (0x{0:x2}) {1}.", opcode, pName);

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
                            // _log.Error("Data: {0}", BitConverter.ToString(stream.ReadBytes(stream.Count - stream.Pos)));
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