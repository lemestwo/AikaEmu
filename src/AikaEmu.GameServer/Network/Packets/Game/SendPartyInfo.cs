using AikaEmu.GameServer.Models.Group;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class SendPartyInfo : GamePacket
    {
        private readonly Party _party;

        public SendPartyInfo(Party party)
        {
            _party = party;

            Opcode = (ushort) GameOpcode.SendPartyInfo;
        }

        public override PacketStream Write(PacketStream stream)
        {
            if (_party == null)
            {
                stream.Write("", 236);
                return stream;
            }

            var members = _party.Members;

            for (byte i = 0; i < 6; i++)
                stream.Write(members.ContainsKey(i) ? members[i].Name : "", 16);

            for (byte i = 0; i < 6; i++)
                stream.Write(members.ContainsKey(i) ? members[i].Connection.Id : (ushort) 0);

            for (byte i = 0; i < 6; i++)
                stream.Write((byte) (members.ContainsKey(i) ? members[i].Profession : 0));

            for (byte i = 0; i < 6; i++)
                stream.Write((byte) (members.ContainsKey(i) ? members[i].Level : 0));

            for (byte i = 0; i < 6; i++)
                stream.Write((uint) (members.ContainsKey(i) ? members[i].Hp : 0));

            for (byte i = 0; i < 6; i++)
                stream.Write((uint) (members.ContainsKey(i) ? members[i].MaxHp : 0));

            for (byte i = 0; i < 6; i++)
                stream.Write((uint) (members.ContainsKey(i) ? members[i].Mp : 0));

            for (byte i = 0; i < 6; i++)
                stream.Write((uint) (members.ContainsKey(i) ? members[i].MaxMp : 0));

            stream.Write(_party.LeaderConId); // leaderId

            stream.Write((byte) _party.XpType); // xpType
            stream.Write((byte) _party.LootType); // lootType

            stream.Write((byte) 0); //unk
            stream.Write((byte) 1); //unk
            stream.Write((byte) 0); //unk
            stream.Write((byte) 0); //unk
            return stream;
        }
    }
}