using System.Collections.Generic;
using AikaEmu.GameServer.Models;
using AikaEmu.Shared.Utils;

namespace AikaEmu.GameServer.Managers
{
    public class AccountsManager : Singleton<AccountsManager>
    {
        private readonly Dictionary<ushort, Account> _accounts;

        public AccountsManager()
        {
            _accounts = new Dictionary<ushort, Account>();
        }

        public bool AddAccount(Account account)
        {
            if (GetAccount(account.Id) != null)
            {
                // TODO
                return false;
            }

            _accounts.Add(account.Connection.Id, account);
            return true;
        }

        public void RemoveAccount(ushort accId)
        {
            if (!_accounts.ContainsKey(accId)) return;

            _accounts.Remove(accId);
        }

        public void RemoveAccount(uint accId)
        {
            foreach (var (key, account) in _accounts)
            {
                if (account.Id == accId) RemoveAccount(key);
            }
        }

        public Account GetAccount(ushort accId)
        {
            return _accounts.ContainsKey(accId) ? _accounts[accId] : null;
        }

        public Account GetAccount(uint accId)
        {
            foreach (var (_, account) in _accounts)
            {
                if (account.Id == accId) return account;
            }

            return null;
        }
    }
}