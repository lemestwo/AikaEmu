using AikaEmu.GameServer.Managers;
using AikaEmu.GameServer.Models.Chat;
using AikaEmu.GameServer.Models.Units.Character;
using AikaEmu.GameServer.Models.Units.Character.Const;
using AikaEmu.GameServer.Network.Packets.Game;
using NLog;

namespace AikaEmu.GameServer.Models.Item.UseItem
{
    public class GoldCoin : IUseItem
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();

        public void Execute(Character character, Item item, int data)
        {
            _log.Info("UseItem: GoldCoin, Amount: {0}", item.ItemData.GearCoreLevel);

            var maxMoney = DataManager.Instance.CharacterData.Data.MaxGold;
            var amount = item.ItemData.GearCoreLevel;
            if (character.Money + amount > maxMoney)
            {
                character.SendPacket(new SendMessage(new Message($"Can't have more than {maxMoney:#.0} gold.")));
                return;
            }

            if (!character.Inventory.RemoveItem(item.SlotType, item.Slot, 1, false)) return;

            character.Money += amount;
            character.SendPacket(new UpdateCharGold(character));
            character.Save(SaveType.Inventory);
        }
    }
}