using System;
using AikaEmu.GameServer.Models.Units.Character;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
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
            stream.Write(_character.Account.DbId);
            stream.Write((uint) _character.Connection.Id);
            stream.Write(0);
            stream.Write(_character.DbId);
            stream.Write(_character.Name, 16);
            stream.Write((byte) 2); // nation?
            stream.Write((ushort) _character.Profession);
            stream.Write((byte) 0); // unk
            stream.Write(_character.Attributes);
            stream.Write((ushort) 0); // status?
            stream.Write(_character.BodyTemplate.Width);
            stream.Write(_character.BodyTemplate.Chest);
            stream.Write(_character.BodyTemplate.Leg);
            stream.Write(_character.BodyTemplate.Body);
            stream.Write(_character.Hp);
            stream.Write(_character.Hp); // TODO - MAX
            stream.Write(_character.Mp);
            stream.Write(_character.Mp); // TODO - MAX
            stream.Write(0); // TODO - nextDate at 09:00 AM (epochTime)
            stream.Write(_character.HonorPoints);
            stream.Write(_character.PvpPoints);
            stream.Write(_character.InfamyPoints);
            stream.Write((ushort) 0);
            stream.Write((ushort) 10); // skillPoint
            stream.Write((ushort) 0);
            stream.Write("", 60);
            stream.Write((ushort) 0);
            stream.Write((ushort) 120); // pAtt
            stream.Write((ushort) 120); // pDef
            stream.Write((ushort) 120); // mAtt
            stream.Write((ushort) 120); // mDef
            stream.Write((ushort) 0); // bonusDmg?
            stream.Write((ushort) 0);
            stream.Write((ushort) 0); // unk 3A 00
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
            stream.Write((ushort) 5); // guildLogo?
            stream.Write("", 32);
            stream.Write("", 40); // buffId ushort
            stream.Write("", 80); // buffTime uint

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

            stream.Write("", 186); // unknown
            stream.Write((ushort) 2);
            stream.Write("", 196); // unknown
            stream.Write("", 212); // quests

            stream.Write(Convert.ToUInt16(_character.Position.CoordX));
            stream.Write(Convert.ToUInt16(_character.Position.CoordY));
            stream.Write((short) _character.Position.Rotation); // rotation

            stream.Write("", 92); // unk
            stream.Write(equips.ContainsKey(1) ? equips[1].ItemId : (ushort) 0);
            stream.Write(equips.ContainsKey(2) ? equips[2].ItemId : (ushort) 0);
            stream.Write(equips.ContainsKey(3) ? equips[3].ItemId : (ushort) 0);
            stream.Write(equips.ContainsKey(4) ? equips[4].ItemId : (ushort) 0);
            stream.Write(equips.ContainsKey(5) ? equips[5].ItemId : (ushort) 0);
            stream.Write(equips.ContainsKey(6) ? equips[6].ItemId : (ushort) 0);
            stream.Write(equips.ContainsKey(7) ? equips[7].ItemId : (ushort) 0);
            stream.Write((ushort) 0);
            stream.Write((ushort) 0); // unk 75 01
            stream.Write(0);
            stream.Write(equips.ContainsKey(2) ? equips[2].ItemId : (ushort) 0);
            stream.Write(equips.ContainsKey(3) ? equips[3].ItemId : (ushort) 0);
            stream.Write(equips.ContainsKey(4) ? equips[4].ItemId : (ushort) 0);
            stream.Write(equips.ContainsKey(5) ? equips[5].ItemId : (ushort) 0);
            stream.Write(equips.ContainsKey(6) ? equips[6].ItemId : (ushort) 0);
            stream.Write(equips.ContainsKey(7) ? equips[7].ItemId : (ushort) 0);
            stream.Write((ushort) 0);
            stream.Write((ushort) 0); // unk 75 01

            stream.Write(0); // TODO - character creation date (epoch time)
            stream.Write("", 352); // unk
            stream.Write(0); // sometimes has date (epoch time)
            stream.Write("", 80); // unk

            stream.Write(_character.Token, 4);

            stream.Write("", 212); // unk

            stream.Write(_character.Skills.WriteSkills());
            stream.Write(_character.SkillsBar.WriteSkillsBar());

            // info from packet 0x1086 fits here
            for (var i = 0; i < 32; i++)
            {
                if (i == 2) stream.Write(272);
                else if (i == 3) stream.Write(16);
                else stream.Write(0);
            }

            stream.Write("", 416);

            stream.Write(0); // TODO - next possible date at 03:00 AM (epochTime)
            stream.Write(0); // always empty?
            stream.Write(3600); // unk - fixed value

            stream.Write("", 48);

            stream.Write(0); // next monday 9:00 AM date (epoch)
            stream.Write(0); // TODO - nextLoginCoin (+10min from loginTime)

            stream.Write("", 20);

            stream.Write((short) 1); // some chars have this value
            stream.Write((short) 1); // some chars have this value

            stream.Write("", 20);

            stream.Write(2); // some chars have this value

            stream.Write(0); // TODO - Login time (Epoch)

            stream.Write("", 12);

            stream.Write("TestPran", 16); // pran name?
            stream.Write("TestPran2", 16); // pran name?

            stream.Write((byte) 83); // pranTypeId
            stream.Write((byte) 73); // pranTypeId
            stream.Write((short) 0); // empty
            return stream;
        }
    }
}