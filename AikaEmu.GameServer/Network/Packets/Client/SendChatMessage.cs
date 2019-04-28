using System;
using AikaEmu.GameServer.Managers;
using AikaEmu.GameServer.Managers.Configuration;
using AikaEmu.GameServer.Managers.Id;
using AikaEmu.GameServer.Models;
using AikaEmu.GameServer.Models.CharacterM;
using AikaEmu.GameServer.Models.Chat;
using AikaEmu.GameServer.Models.ItemM;
using AikaEmu.GameServer.Models.NpcM;
using AikaEmu.GameServer.Models.Unit;
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
            Connection.ActiveCharacter.SendPacketAll(new Game.SendChatMessage(msgPacket));

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
                        var newPos = new Position
                        {
                            NationId = 1, // TODO
                            CoordX = Convert.ToSingle(arg1),
                            CoordY = Convert.ToSingle(arg2)
                        };
                        Connection.ActiveCharacter.SetPosition(newPos);
                        break;
                    case "mobspawn":
                        var temp = new Mob
                        {
                            Id = IdMobSpawnManager.Instance.GetNextId(),
                            MobId = arg1,
                            Hp = 2000,
                            Mp = 2000,
                            MaxHp = 2000,
                            MaxMp = 2000,
                            Name = DataManager.Instance.MnData.GetUnitName(arg1),
                            Position = new Position
                            {
                                NationId = 1,
                                CoordX = Connection.ActiveCharacter.Position.CoordX + 2.0f,
                                CoordY = Connection.ActiveCharacter.Position.CoordY + 2.0f
                            },
                            BodyTemplate = new BodyTemplate
                            {
                                Width = 7,
                                Chest = 119,
                                Leg = 119,
                                Body = 0
                            }
                        };
                        WorldManager.Instance.Spawn(temp);
                        break;
                    case "testnpc":
                        for (var i = 0u; i < arg1; i++)
                        {
                            var npc = new Npc
                            {
                                Id = IdUnitSpawnManager.Instance.GetNextId(),
                                NpcId = 41,
                                Hp = 2000,
                                Mp = 2000,
                                MaxHp = 2000,
                                MaxMp = 2000,
                                Name = "Effects "+i,
                                Position = new Position
                                {
                                    NationId = 1,
                                    CoordX = Connection.ActiveCharacter.Position.CoordX,
                                    CoordY = (float)(Connection.ActiveCharacter.Position.CoordY + i),
                                    Rotation = 0
                                },
                                BodyTemplate = new BodyTemplate
                                {
                                    Width = 7,
                                    Chest = 119,
                                    Leg = 119,
                                    Body = 219
                                }
                            };
                            Connection.SendPacket(new SendUnitSpawn(npc));
                            Connection.SendPacket(new SetEffectOnHead(npc.Id, i));
                        }

                        break;
                }
            }
        }
    }
}