using System.Collections.Generic;
using AikaEmu.AuthServer.Models;
using AikaEmu.Shared.Utils;

namespace AikaEmu.AuthServer.Managers
{
    public class AuthAccountsManager : Singleton<AuthAccountsManager>
    {
        private readonly Dictionary<uint, LobbyAccount> _accounts;

        protected AuthAccountsManager()
        {
            _accounts = new Dictionary<uint, LobbyAccount>();
        }

        public void Add(Account account, uint key)
        {
            if (_accounts.ContainsKey(account.Id)) Remove(account.Id);

            _accounts.Add(account.Id, new LobbyAccount(account, key));
        }

        public void Remove(uint accId)
        {
            if (_accounts.ContainsKey(accId)) _accounts.Remove(accId);
        }

        public LobbyAccount GetAccount(uint accId)
        {
            return _accounts.ContainsKey(accId) ? _accounts[accId] : null;
        }
    }

    public class LobbyAccount
    {
        public Account Account { get; }
        public uint Key { get; }

        public LobbyAccount(Account account, uint key)
        {
            Key = key;
            Account = account;
        }
    }
}