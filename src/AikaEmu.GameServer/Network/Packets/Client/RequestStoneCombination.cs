using System.Collections.Generic;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Client
{
    public class RequestStoneCombination : GamePacket
    {
        protected override void Read(PacketStream stream)
        {
            var mainSlot = stream.ReadUInt16();
            stream.ReadUInt16();
            var reagents = new List<ushort>();
            for (var i = 0; i < 4; i++)
            {
                var stone = stream.ReadUInt16();
                stream.ReadUInt16();
                if(stone < ushort.MaxValue) reagents.Add(stone);
            }

            var warrantySlot = stream.ReadUInt16();
            
            Log.Debug("RequestStoneCombination: Not implemented yet.");
        }
    }
}