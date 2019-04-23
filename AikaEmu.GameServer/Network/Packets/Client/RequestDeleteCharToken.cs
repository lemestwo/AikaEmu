using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.GameServer.Network.Packets.Game;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Client
{
    public class RequestDeleteCharToken : GamePacket
    {
        protected override void Read(PacketStream stream)
        {
            var step = stream.ReadInt32();
            var charSlot = stream.ReadUInt32();
            var accId = stream.ReadUInt32();

            Log.Info("RequestDeleteCharToken");
            Connection.SendPacket(new ResponseDeleteCharToken(8));
        }
    }
}