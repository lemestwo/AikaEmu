using System;
using System.Collections.Generic;
using AikaEmu.GameServer.Models.Group;
using AikaEmu.Shared.Utils;

namespace AikaEmu.GameServer.Managers
{
    public class PartyManager : Singleton<PartyManager>
    {
        private readonly Dictionary<uint, Party> _partiesList;
        private readonly Dictionary<ushort, PartyInvite> _partyInvites; // invitedConId, PartyInvite

        public PartyManager()
        {
            _partiesList = new Dictionary<uint, Party>();
            _partyInvites = new Dictionary<ushort, PartyInvite>();
        }

        public PartyInvite GetInvite(ushort invited)
        {
            if (!_partyInvites.ContainsKey(invited)) return null;

            var found = _partyInvites[invited];
            if (found.Time > DateTime.UtcNow)
                return _partyInvites[invited];

            _partyInvites.Remove(invited);
            return null;
        }

        public void RemoveInvite(ushort invited)
        {
            if (_partyInvites.ContainsKey(invited)) _partyInvites.Remove(invited);
        }

        public bool AddInvitation(ushort inviter, ushort invited)
        {
            if (GetInvite(invited) != null) return false;

            var inviteTemplate = new PartyInvite
            {
                ConInviter = inviter,
                ConInvited = invited,
                Time = DateTime.UtcNow.AddMinutes(5)
            };
            _partyInvites.Add(invited, inviteTemplate);
            return true;
        }

        public Party GetParty(uint id)
        {
            return _partiesList.ContainsKey(id) ? _partiesList[id] : null;
        }

        public Party GetParty(ushort conId)
        {
            foreach (var party in _partiesList.Values)
            {
                foreach (var member in party.Members.Values)
                {
                    if (member.Connection?.Id == conId) return party;
                }
            }

            return null;
        }

        public bool AddParty(Party party)
        {
            if (_partiesList.ContainsKey(party.Id)) return false;

            _partiesList.Add(party.Id, party);
            return true;
        }

        public void RemoveParty(uint id)
        {
            if (_partiesList.ContainsKey(id)) _partiesList.Remove(id);
        }
    }
}