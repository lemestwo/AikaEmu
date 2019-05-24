using System.Collections.Generic;
using AikaEmu.GameServer.Helpers;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Client
{
    public class RequestCoreConvert : GamePacket
    {
        protected override void Read(PacketStream stream)
        {
            // Reduce 3 enchant (ex: +12 to +9)
            var coreSlot = stream.ReadByte();
            var itemSlot = stream.ReadByte();
            var extractSlots = new List<ushort>();
            for (var i = 0; i < 4; i++)
            {
                var slot = stream.ReadByte();
                if (slot < byte.MaxValue) extractSlots.Add(slot);
            }

            GearCoreHelper.CoreConvert(Connection, coreSlot, itemSlot, extractSlots);
        }
    }
}