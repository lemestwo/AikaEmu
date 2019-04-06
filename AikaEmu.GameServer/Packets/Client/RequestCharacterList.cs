using System;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Packets.Client
{
    public class RequestCharacterList : GamePacket
    {
        public override void Read(PacketStream stream)
        {
            var accId = stream.ReadUInt32();
            var user = stream.ReadBytes(32);
            Log.Info("PacketRequestCharacterList, AccId: {0}, User: {1}", accId, System.Text.Encoding.UTF8.GetString(user));
            
            Connection.SendPacket(new SendCharacterList());
        }
    }
}