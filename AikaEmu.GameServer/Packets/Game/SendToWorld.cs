using System;
using AikaEmu.GameServer.Models;
using AikaEmu.GameServer.Models.Char.Inventory;
using AikaEmu.GameServer.Network;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Packets.Game
{
    public class SendToWorld : GamePacket
    {
        private readonly Character _character;

        public SendToWorld(Character character)
        {
            _character = character;

            Opcode = (ushort) GameOpcode.SendToWorld;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(_character.AccountId);
            stream.Write((uint) _character.ConnectionId); // charObjId
            stream.Write(0);
            stream.Write(_character.Id);
            stream.Write(_character.Name, 16);
            stream.Write((byte) 5); // server
            stream.Write((ushort) _character.CharClass);
            stream.Write((byte) 0);
            stream.Write(_character.CharAttributes);
            stream.Write((ushort) 0); // status
            stream.Write(_character.BodyTemplate);
            stream.Write(_character.Hp);
            stream.Write(_character.Hp); // TODO - MAX
            stream.Write(_character.Mp);
            stream.Write(_character.Mp); // TODO - MAX
            stream.Write((byte) 144);
            stream.Write((byte) 83);
            stream.Write((byte) 176);
            stream.Write((byte) 92);
            stream.Write(_character.HonorPoints);
            stream.Write(_character.PvpPoints);
            stream.Write(_character.InfamyPoints);
            stream.Write((ushort) 0);
            stream.Write((ushort) 0);
            stream.Write((ushort) 0);
            stream.Write("", 60);
            stream.Write((ushort) 0);
            stream.Write((ushort) 120); // pAtt
            stream.Write((ushort) 120); // pDef
            stream.Write((ushort) 120); // mAtt
            stream.Write((ushort) 120); // mDef
            stream.Write((ushort) 10);
            stream.Write(0);
            stream.Write((byte) 15);
            stream.Write((byte) 22);
            stream.Write(0);
            stream.Write((ushort) 17); // crit
            stream.Write((byte) 15); // dodge
            stream.Write((byte) 90); // acerto
            stream.Write((ushort) 0);
            stream.Write((ushort) 0);
            stream.Write(_character.Experience);
            stream.Write(_character.Level);
            stream.Write("", 154);

            var equips = _character.Inventory.GetItemsBySlotType(SlotType.Equipments);
            for (ushort i = 0; i < 16; i++)
            {
                if (equips.ContainsKey(i))
                {
                    stream.Write(equips[i]);
                }
                else
                {
                    stream.Write("", 20);
                }
            }

            stream.Write(0);

            var inv = _character.Inventory.GetItemsBySlotType(SlotType.Inventory);
            for (ushort i = 0; i < 84; i++)
            {
                if (inv.ContainsKey(i))
                {
                    stream.Write(inv[i]);
                }
                else
                {
                    stream.Write("", 20);
                }
            }

            stream.Write(_character.Money);
            stream.Write("", 384);
            stream.Write("", 212);
            stream.Write(Convert.ToUInt16(_character.Position.CoordX));
            stream.Write(Convert.ToUInt16(_character.Position.CoordY));
            stream.Write((byte) 0); // world?

            stream.Write("", 131);
            stream.Write((byte) 157);
            stream.Write((byte) 216);
            stream.Write((byte) 175);
            stream.Write((byte) 92);
            stream.Write("", 436);

            stream.Write(_character.Token, 4);

            stream.Write("", 212);
            stream.Write((ushort) 2);
            stream.Write((ushort) 2);
            stream.Write((ushort) 2);
            stream.Write((ushort) 2);
            stream.Write(0);
            stream.Write((ushort) 2);
            stream.Write("", 106);

            stream.Write((byte) 18);
            stream.Write((byte) 180);
            stream.Write((ushort) 0);
            stream.Write((byte) 18);
            stream.Write((byte) 186);
            stream.Write((ushort) 0);
            stream.Write((byte) 18);
            stream.Write((byte) 182);
            stream.Write((ushort) 0);
            stream.Write((byte) 18);
            stream.Write((byte) 183);
            stream.Write((ushort) 0);
            stream.Write(0);
            stream.Write(0);
            stream.Write(0);
            stream.Write((byte) 18);
            stream.Write((byte) 181);
            stream.Write((ushort) 0);
            stream.Write("", 612);

            stream.Write((byte) 48);
            stream.Write((byte) 255);
            stream.Write((byte) 175);
            stream.Write((byte) 92);
            stream.Write(0);
            stream.Write(3600);

            stream.Write("", 48);

            stream.Write((byte) 42);
            stream.Write((byte) 72);
            stream.Write((byte) 180);
            stream.Write((byte) 92);
            stream.Write((byte) 233);
            stream.Write((byte) 13);
            stream.Write((byte) 173);
            stream.Write((byte) 92);

            stream.Write("", 48);
            stream.Write((ushort) 55477);
            stream.Write((ushort) 23727);
            stream.Write("", 44);
            stream.Write(18771);
            return stream;
        }
    }
}