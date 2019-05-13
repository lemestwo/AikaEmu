using AikaEmu.GameServer.Helpers;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Client
{
    public class BuySkillFromNpc : GamePacket
    {
        protected override void Read(PacketStream stream)
        {
            var skillId = (ushort) stream.ReadUInt32();
            var npcConId = stream.ReadUInt32();

            NpcDialogHelper.BuySkillFromNpc(Connection.ActiveCharacter, npcConId, skillId);
        }
    }
}