using System.Collections.Generic;
using AikaEmu.GameServer.Models;
using AikaEmu.GameServer.Models.Char.Inventory;
using AikaEmu.GameServer.Network;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Packets.Game
{
    public class SendCharacterList : GamePacket
    {
        private readonly ushort _conId;
        private readonly uint _accId;
        private readonly Dictionary<uint, Character> _characters;

        public SendCharacterList(ushort conId, uint accId, Dictionary<uint, Character> characters)
        {
            _conId = conId;
            _accId = accId;
            _characters = characters;

            Opcode = (ushort) GameOpcode.SendCharacterList;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(_accId);
            stream.Write(1);
            stream.WriteCc(4);

            for (var i = 0u; i < 3; i++)
            {
                if (_characters.ContainsKey(i))
                {
                    WriteChar(stream, _characters[i]);
                }
                else
                {
                    stream.Write(string.Empty, 16, true);
                    stream.Write(string.Empty, 33);
                    stream.WriteCc(3);
                    stream.Write(string.Empty, 14);
                    stream.WriteCc(6);
                    stream.Write(string.Empty, 16);
                    stream.WriteCc(4);
                    stream.Write(string.Empty, 6);
                    stream.WriteCc(6);
                }
            }

            return stream;
        }

        private static void WriteChar(PacketStream stream, Character character)
        {
            stream.Write(character.Name, 16, true);
            stream.Write((ushort) 0);
            stream.Write((ushort) character.CharClass);
            stream.Write(character.BodyTemplate);
            
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
            stream.Write((byte) (equips.ContainsKey(2) ? equips[2].Refinement << 4 : 0));
            stream.Write((byte) (equips.ContainsKey(3) ? equips[3].Refinement << 4 : (ushort) 0));
            stream.Write((byte) 0); // accessories
            stream.Write((byte) (equips.ContainsKey(4) ? equips[4].Refinement << 4 : (ushort) 0));
            stream.Write((byte) (equips.ContainsKey(5) ? equips[5].Refinement << 4 : (ushort) 0));
            stream.Write((byte) (equips.ContainsKey(6) ? equips[6].Refinement << 4 : (ushort) 0));
            stream.Write((byte) (equips.ContainsKey(7) ? equips[7].Refinement << 4 : (ushort) 0));
            stream.WriteCc(3);
            
            stream.Write(character.CharAttributes.Strenght);
            stream.Write(character.CharAttributes.Agility);
            stream.Write(character.CharAttributes.Intelligence);
            stream.Write(character.CharAttributes.Constitution); // visual bug in aikaBR (health)
            stream.Write(character.CharAttributes.Spirit); // visual bug in aikaBR (mana)
            stream.Write((short) 0);
            stream.Write(character.Level);
            stream.WriteCc(6);
            stream.Write(character.Experience);
            stream.Write(character.Money);
            stream.WriteCc(4);
            stream.Write(0); // Time to delete
            stream.Write((byte) 0); // token error count
            stream.Write(!string.IsNullOrEmpty(character.Token) ? (byte) 1 : (byte) 0);
            stream.WriteCc(6);
        }
    }
}