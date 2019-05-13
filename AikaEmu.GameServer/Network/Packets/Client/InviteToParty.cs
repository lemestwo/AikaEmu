using AikaEmu.GameServer.Managers;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.GameServer.Network.Packets.Game;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Client
{
    public class InviteToParty : GamePacket
    {
        protected override void Read(PacketStream stream)
        {
            var conId = stream.ReadUInt16();
            stream.ReadInt16();
            // stream.ReadBytes(16);

            var target = WorldManager.Instance.GetCharacter(conId);
            target?.SendPacket(new SendPartyInvite(target.Connection.Id, Connection.Id, Connection.ActiveCharacter.Name));
        }
    }
}