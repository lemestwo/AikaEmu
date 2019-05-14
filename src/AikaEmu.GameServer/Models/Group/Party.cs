using System.Collections.Generic;
using AikaEmu.GameServer.Managers;
using AikaEmu.GameServer.Managers.Id;
using AikaEmu.GameServer.Models.Group.Const;
using AikaEmu.GameServer.Models.Units.Character;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.GameServer.Network.Packets.Game;

namespace AikaEmu.GameServer.Models.Group
{
    public class Party
    {
        public uint Id { get; set; }
        public Dictionary<byte, Character> Members { get; set; }
        public XpType XpType { get; set; }
        public LootType LootType { get; set; }
        public ushort LeaderConId { get; set; }

        public Party()
        {
            Members = new Dictionary<byte, Character>();
        }

        public void CreateNewParty(Character owner, Character member)
        {
            Id = IdPartyManager.Instance.GetNextId();
            Members.Add(0, owner);
            Members.Add(1, member);
            XpType = XpType.Equally;
            LootType = LootType.Order;
            LeaderConId = owner.Connection.Id;

            if (PartyManager.Instance.AddParty(this))
                SendPacketAll(new SendPartyInfo(this));
        }

        public bool IsMember(ushort id)
        {
            foreach (var member in Members.Values)
            {
                if (member.Connection?.Id == id) return true;
            }

            return false;
        }

        public bool AddMember(Character character)
        {
            var slot = GetNextSlot();
            if (slot == 255) return false;
            Members.Add(slot, character);
            UpdateParty();
            return true;
        }

        public void RemoveMember(ushort id)
        {
            var (slot, member) = GetSlot(id);
            if (member == null) return;

            Members.Remove(slot);
            member.SendPacket(new SendPartyInfo(null));
            UpdateParty();

            if (Members.Count <= 0)
                PartyManager.Instance.RemoveParty(Id);
        }

        public void SendPacketAll(GamePacket packet)
        {
            foreach (var character in Members.Values)
            {
                character.Connection?.SendPacket(packet);
            }
        }

        public void UpdateParty()
        {
            SendPacketAll(new SendPartyInfo(this));
        }

        private byte GetNextSlot()
        {
            for (byte i = 0; i < 6; i++)
            {
                if (!Members.ContainsKey(i)) return i;
            }

            return 255;
        }

        private (byte slot, Character character) GetSlot(ushort id)
        {
            foreach (var (key, value) in Members)
            {
                if (value.Connection?.Id == id) return (key, value);
            }

            return (255, null);
        }
    }
}