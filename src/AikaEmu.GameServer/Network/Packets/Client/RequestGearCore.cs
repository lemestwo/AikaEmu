using System.Collections.Generic;
using AikaEmu.GameServer.Helpers;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Client
{
    public class RequestGearCore : GamePacket
    {
        protected override void Read(PacketStream stream)
        {
            // Read values from stream
            var coreSlot = stream.ReadUInt16();
            var itemSlot = stream.ReadUInt16();
            var extractSlots = new List<ushort>();
            for (var i = 0; i < 4; i++)
            {
                var slot = stream.ReadUInt16();
                if (slot != ushort.MaxValue) extractSlots.Add(slot);
            }

            GearCoreHelper.CoreUpgrade(Connection, coreSlot, itemSlot, extractSlots);
        }
    }
}