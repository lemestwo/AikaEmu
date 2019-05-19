using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.GameServer.Network.Packets.Game;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Client
{
    public class RequestCoreUpgrade : GamePacket
    {
        protected override void Read(PacketStream stream)
        {
            // Reduce 3 enchant (+15 to +12)
            ushort coreSlot = stream.ReadByte();
            ushort itemSlot = stream.ReadByte();
            ushort extractSlot1 = stream.ReadByte();
            ushort extractSlot2 = stream.ReadByte();
            ushort extractSlot3 = stream.ReadByte();
            ushort extractSlot4 = stream.ReadByte();
            Log.Debug("RequestCoreConversion, coreSlot: {0}, itemSlot: {1}", coreSlot, itemSlot);

            Connection.SendPacket(new CoreUpgradeResult(Connection.Id, true));
        }
    }
}