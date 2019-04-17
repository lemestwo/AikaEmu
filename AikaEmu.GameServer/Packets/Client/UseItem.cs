using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Packets.Client
{
    public class UseItem : GamePacket
    {
        protected override void Read(PacketStream stream)
        {
            var bag = stream.ReadUInt16();
            var unk = stream.ReadUInt16();
            var slot = stream.ReadUInt16();
            var unk2 = stream.ReadUInt16();
            var unk3 = stream.ReadInt32();

            Log.Debug("UseItem, bag: {0}, unk: {1}, slot: {2}, unk2: {3}, unk3: {4}", bag, unk, slot, unk2, unk3);
        }
    }
}