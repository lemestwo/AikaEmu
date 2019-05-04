using AikaEmu.GameServer.Models.Units.Character;

namespace AikaEmu.GameServer.Models.Item
{
    public interface IUseItem
    {
        void Execute(Character character, Item item, int data);
    }
}