using AikaEmu.GameServer.Managers;
using AikaEmu.GameServer.Models.NpcM.Dialog;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Client
{
    public class RequestNpcChat : GamePacket
    {
        protected override void Read(PacketStream stream)
        {
            var npcId = stream.ReadUInt32();
            var chatOption = (DialogType) stream.ReadUInt32();
            var subChatOption = stream.ReadUInt32();

            NpcDialogManager.Instance.StartDialog(Connection.ActiveCharacter, npcId, chatOption, subChatOption);
        }
    }
}