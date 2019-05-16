using System.Collections.Generic;
using System.IO;
using System.Linq;
using AikaEmu.GameServer.Helpers;
using AikaEmu.GameServer.Managers;
using AikaEmu.GameServer.Models;
using AikaEmu.GameServer.Models.Item;
using AikaEmu.GameServer.Models.Units.Character;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.GameServer.Network.Packets.Game;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Client
{
    public class UpdateTradeData : GamePacket
    {
        protected override void Read(PacketStream stream)
        {
            stream.ReadBytes(200); // itemsData
            var itemsSlot = new Dictionary<byte, ushort>();
            for (byte i = 0; i < 10; i++)
            {
                var slot = stream.ReadByte();
                if (slot != 0xFF) itemsSlot.Add(i, slot);
            }

            stream.ReadUInt16(); // unk
            var money = stream.ReadUInt64();
            var ok = stream.ReadBoolean();
            var confirm = stream.ReadBoolean();
            var conId = stream.ReadUInt16();
            
            TradeHelper.TradeUpdateData(Connection, itemsSlot, money, ok, confirm, conId);
        }
    }
}