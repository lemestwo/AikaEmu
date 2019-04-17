using AikaEmu.GameServer.Managers.Configuration;
using AikaEmu.GameServer.Models.Char.Inventory;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.GameServer.Packets.Game;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Packets.Client
{
    public class SendChatMessage : GamePacket
    {
        protected override void Read(PacketStream stream)
        {
            var unk1 = stream.ReadInt32();
            var unk2 = stream.ReadInt32();
            var chatType = stream.ReadInt32();
            var charName = stream.ReadString(16);
            var msg = stream.ReadString(128);

            Log.Debug("SendChat, unk1: {0}, unk2: {1}, type: {2}", unk1, unk2, chatType);

            var command = msg.Split(" ");
            if (command.Length >= 3)
            {
                if (!ushort.TryParse(command[1], out var id)) return;
                if (!ushort.TryParse(command[2], out var slot)) return;
                if (!byte.TryParse(command[3], out var refi)) return;

                switch (command[0])
                {
                    case "additem":
                        var item = new Item
                        {
                            Id = 1,
                            ItemId = id,
                            SlotType = SlotType.Inventory,
                            Slot = slot,
                            Effect1 = 50,
                            Effect2 = 55,
                            Effect3 = 60,
                            Effect1Value = 100,
                            Effect2Value = 150,
                            Effect3Value = 200,
                            Durability = 200,
                            DurMax = 200,
                            Refinement = refi,
                            DisableDurplus = 1,
                            Time = 0,
                        };
                        Connection.SendPacket(new UpdateItem(item, true));
                        break;
                }
            }
        }
    }
}