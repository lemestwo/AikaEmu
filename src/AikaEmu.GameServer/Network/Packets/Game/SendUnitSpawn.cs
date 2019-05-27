using System;
using AikaEmu.GameServer.Models.Units;
using AikaEmu.GameServer.Models.Units.Character;
using AikaEmu.GameServer.Models.Units.Const;
using AikaEmu.GameServer.Models.Units.Npc;
using AikaEmu.GameServer.Models.Units.Pran;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class SendUnitSpawn : GamePacket
    {
        private readonly BaseUnit _unit;
        private readonly byte _spawnType;
        private readonly Character _character;

        public SendUnitSpawn(BaseUnit unit, byte spawnType = 0, Character character = null)
        {
            _unit = unit;
            _spawnType = spawnType;
            _character = character;

            Opcode = (ushort) GameOpcode.SendUnitSpawn;
            SetSenderIdWithUnit(_unit);
        }

        public override PacketStream Write(PacketStream stream)
        {
            if (_unit is Character character)
            {
                stream.Write(character.Name, 16);

                var equips = character.Inventory.GetItemsBySlotType(SlotType.Equipments);
                stream.Write(equips.ContainsKey(0) ? equips[0].ItemId : (ushort) 0);
                stream.Write(equips.ContainsKey(1) ? equips[1].ItemId : (ushort) 0);
                stream.Write(equips.ContainsKey(2) ? equips[2].ItemId : (ushort) 0);
                stream.Write(equips.ContainsKey(3) ? equips[3].ItemId : (ushort) 0);
                stream.Write(equips.ContainsKey(4) ? equips[4].ItemId : (ushort) 0);
                stream.Write(equips.ContainsKey(5) ? equips[5].ItemId : (ushort) 0);
                stream.Write(equips.ContainsKey(6) ? equips[6].ItemId : (ushort) 0);
                stream.Write(equips.ContainsKey(7) ? equips[7].ItemId : (ushort) 0);
                stream.Write((byte) 0); // accessories
                stream.Write((byte) 0); // accessories
                stream.Write((byte) (equips.ContainsKey(2) ? equips[2].Quantity << 4 : 0));
                stream.Write((byte) (equips.ContainsKey(3) ? equips[3].Quantity << 4 : (ushort) 0));
                stream.Write((byte) 0); // accessories
                stream.Write((byte) (equips.ContainsKey(4) ? equips[4].Quantity << 4 : (ushort) 0));
                stream.Write((byte) (equips.ContainsKey(5) ? equips[5].Quantity << 4 : (ushort) 0));
                stream.Write((byte) (equips.ContainsKey(6) ? equips[6].Quantity << 4 : (ushort) 0));
                stream.Write((byte) (equips.ContainsKey(7) ? equips[7].Quantity << 4 : (ushort) 0));
                stream.WriteCc(3);

                stream.Write(character.Position.CoordX);
                stream.Write(character.Position.CoordY);
                stream.Write((ushort) 0); // rotation (data+64 * 3,1415 / 180)
                stream.Write((ushort) 0);
                stream.Write(character.Hp);
                stream.Write(character.Hp); // TODO - MAX
                stream.Write(character.Mp);
                stream.Write(character.Mp); // TODO - MAX
                stream.Write((byte) 10); // unk - Can go up to 70
                stream.Write((byte) 61); // unk
                stream.Write(_spawnType); //spawnType
                stream.Write(character.BodyTemplate.Width);
                stream.Write(character.BodyTemplate.Chest);
                stream.Write(character.BodyTemplate.Leg);
                stream.Write((ushort) 0); // headEffect
                stream.Write((ushort) 0); // isStore
                stream.Write(equips.ContainsKey(2) ? equips[2].ItemId : (ushort) 0); // helmet image?

                stream.Write("", 120); // buff id (ushort)
                stream.Write("", 240); // buff duration (uint epoch time)

                stream.Write("Title", 32); // title/guild

                stream.Write((byte) 0); // citId?
                stream.Write((byte) ((byte) character.Account.NationId << 4)); // nation?
                stream.Write((byte) 0);
                stream.Write((byte) 0);

                stream.Write((ushort) 70);
                stream.Write((ushort) 0); // unk
                stream.Write((ushort) 0); // emoticon?
                stream.Write((ushort) 0); // unk (byte) >0 <64 unique behavior
                stream.Write(0);
                if (character.Titles.ActiveTitle != null)
                {
                    stream.Write(character.Titles.ActiveTitle.Id);
                    stream.Write((ushort) character.Titles.ActiveTitle.Level);
                }
                else
                {
                    stream.Write(0);
                }
            }
            else if (_unit is Npc npc)
            {
                stream.Write(Convert.ToString(npc.NpcId), 16);
                // if 221 (Tower) different behavior
                stream.Write(npc.Hair);
                stream.Write(npc.Face);
                stream.Write(npc.Helmet);
                stream.Write(npc.Armor);
                stream.Write(npc.Gloves);
                stream.Write(npc.Pants);
                stream.Write(npc.Weapon);
                stream.Write(npc.Shield);

                if (npc.Refinements.Length < 12)
                    stream.Write("", 12);
                else
                    for (var i = 0; i < 12; i++)
                        stream.Write(npc.Refinements[i]);


                stream.Write(npc.Position.CoordX);
                stream.Write(npc.Position.CoordY);
                stream.Write(npc.Position.Rotation);

                stream.Write(npc.Hp);
                stream.Write(npc.Mp);
                stream.Write(npc.MaxHp);
                stream.Write(npc.MaxMp);
                stream.Write(npc.Unk); // unk - Can go up to 70
                stream.Write(_spawnType); // spawnType (pran is usualy 2)
                stream.Write(npc.BodyTemplate.Width);
                stream.Write(npc.BodyTemplate.Chest);
                stream.Write(npc.BodyTemplate.Leg);
                stream.Write(npc.UnkId); // body?
                stream.Write((ushort) (npc.AvailableQuests(_character)?.Count > 0 ? EffectType.QuestStartGreen : EffectType.Default)); // markOnHead
                stream.Write((short) 0); // unk
                stream.Write("", 120); // buffs
                stream.Write("", 240);
                stream.Write(string.IsNullOrEmpty(npc.Title) ? "" : npc.Title, 32);
                stream.Write(0); // unk
                stream.Write(npc.Unk3);
                stream.Write("", 14);
            }
            else if (_unit is Pran pran)
            {
                stream.Write(pran.Name, 16);

                var equips = pran.Account.ActiveCharacter.Inventory.GetItemsBySlotType(SlotType.PranEquipments);
                stream.Write(equips.ContainsKey(0) ? equips[0].ItemId : (ushort) 0);
                stream.Write(equips.ContainsKey(1) ? equips[1].ItemId : (ushort) 0); // TODO - Remove placeholder items
                stream.Write(equips.ContainsKey(2) ? equips[2].ItemId : (ushort) 0);
                stream.Write(equips.ContainsKey(3) ? equips[3].ItemId : (ushort) 0);
                stream.Write(equips.ContainsKey(4) ? equips[4].ItemId : (ushort) 0);
                stream.Write(equips.ContainsKey(5) ? equips[5].ItemId : (ushort) 0);
                stream.Write(equips.ContainsKey(6) ? equips[6].ItemId : (ushort) 0);
                stream.Write(equips.ContainsKey(7) ? equips[7].ItemId : (ushort) 0);
                stream.Write("", 12);

                stream.Write(pran.Position.CoordX);
                stream.Write(pran.Position.CoordY);
                stream.Write(pran.Position.Rotation); // rotation?

                // TODO - Maybe inverted
                stream.Write(pran.Hp); // hp
                stream.Write(pran.Mp); // mp
                stream.Write(pran.MaxHp); // maxHp
                stream.Write(pran.MaxMp); // maxMp

                stream.Write((byte) 0); // unk 
                stream.Write((byte) 61); // unk fixed value
                stream.Write(_spawnType); // (pran is usualy 2) spawnType

                stream.Write(pran.BodyTemplate.Width);
                stream.Write(pran.BodyTemplate.Chest);
                stream.Write(pran.BodyTemplate.Leg);
                stream.Write(pran.Id);

                stream.Write("", 204);
                stream.Write(24); // unk
                stream.Write("", 156);

                stream.Write(pran.Account.ActiveCharacter.Name + " 's Pran", 32);
                stream.Write("", 20);
            }


            return stream;
        }
    }
}