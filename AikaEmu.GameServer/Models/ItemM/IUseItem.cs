using AikaEmu.GameServer.Models.CharacterM;

namespace AikaEmu.GameServer.Models.ItemM
{
    public interface IUseItem
    {
        void Init(Character character, Item item, int data);
    }
}