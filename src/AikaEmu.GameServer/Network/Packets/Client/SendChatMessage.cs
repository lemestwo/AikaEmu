using System;
using AikaEmu.GameServer.Models.Chat;
using AikaEmu.GameServer.Models.Chat.Const;
using AikaEmu.GameServer.Models.Units.Character;
using AikaEmu.GameServer.Models.Units.Const;
using AikaEmu.GameServer.Models.Units.Npc.Const;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.GameServer.Network.Packets.Game;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Client
{
    public class SendChatMessage : GamePacket
    {
        protected override void Read(PacketStream stream)
        {
            var chatType = (ChatMessageType) stream.ReadInt16();
            var chatTitle = (ChatMessageTitle) stream.ReadInt16();
            var unk2 = stream.ReadInt32();
            var chatUnk = stream.ReadInt32();
            var charName = stream.ReadString(16);
            var msg = stream.ReadString(128);

            var msgPacket = new ChatMessage(Connection.Id, chatUnk, charName, msg, chatType, chatTitle, unk2);
            Connection.ActiveCharacter.SendPacketAll(new Game.SendChatMessage(msgPacket), false, true);

            var command = msg.Split(" ");
            if (command.Length > 1)
            {
                ushort arg2 = 0;
                ushort arg3 = 0;
                if (!ushort.TryParse(command[1], out var arg1)) return;
                if (command.Length > 2)
                {
                    if (!ushort.TryParse(command[2], out arg2)) return;
                }

                if (command.Length > 3)
                {
                    if (!ushort.TryParse(command[3], out arg3)) return;
                }

                switch (command[0])
                {
                    case "additem":
                        Connection.ActiveCharacter.Inventory.AddItem(SlotType.Inventory, (byte) arg2, arg1);
                        break;
                    case "move":
                        var x = Convert.ToSingle(arg1);
                        var y = Convert.ToSingle(arg2);
                        Connection.ActiveCharacter.TeleportTo(x, y);
                        break;
                    case "openshop":
                        Connection.ActiveCharacter.OpenedShopType = (ShopType) arg1;
                        Connection.SendPacket(new OpenNpcShop((ShopType) arg1));
                        break;
                    case "effect":
                        Connection.SendPacket(new SetEffectOnHead(arg1, (EffectType) arg2));
                        break;
                    case "guild":
                        Connection.SendPacket(new SendInviteToGuild(Connection.Id, "Test"));
                        break;
                }
            }
        }
    }
}