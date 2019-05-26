using System.Collections.Generic;
using AikaEmu.GameServer.Models.Account;
using AikaEmu.GameServer.Models.Units.Character;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class SendCharacterList : GamePacket
    {
        private readonly Dictionary<byte, Character> _characters;

        public SendCharacterList(Account acc)
        {
            _characters = acc.AccCharLobby;

            Opcode = (ushort) GameOpcode.SendCharacterList;
            SenderId = acc.Connection.Id;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(Connection.Account.DbId);
            stream.Write(1); // Always 1?
            stream.WriteCc(4);

            for (byte i = 0; i < 3; i++)
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
            stream.Write((ushort) 2); // nationId?
            stream.Write((ushort) character.Profession);
            stream.Write(character.BodyTemplate.Width);
            stream.Write(character.BodyTemplate.Chest);
            stream.Write(character.BodyTemplate.Leg);
            stream.Write(character.BodyTemplate.Body);

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

            stream.Write(character.Attributes.Strenght);
            stream.Write(character.Attributes.Agility);
            stream.Write(character.Attributes.Intelligence);
            stream.Write(character.Attributes.Constitution); // visual bug in aikaBR (health)
            stream.Write(character.Attributes.Spirit); // visual bug in aikaBR (mana)
            stream.Write((short) 0); // always 0?
            stream.Write(character.Level);
            stream.WriteCc(6);
            stream.Write(character.Experience);
            stream.Write(character.Money);
            stream.WriteCc(4);
            stream.Write(0); // Time to delete
            stream.Write((byte) 0); // token error count
            stream.Write(!string.IsNullOrEmpty(character.Token) ? (byte) 1 : (byte) 0); // hasToken
            stream.WriteCc(6);
        }
    }
}