using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.GameServer.Network.Packets.Game;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Client
{
    public class RequestCastSkill : GamePacket
    {
        protected override void Read(PacketStream stream)
        {
            var skillId = stream.ReadUInt16();
            stream.ReadInt16();
            var targetConId = stream.ReadUInt16();
            stream.ReadInt16();
            var targetCoordX = stream.ReadSingle();
            var targetCoordY = stream.ReadSingle();

            Log.Debug("RequestUseSkill: skillId: {0}, targetId: {1}, targetCoord: {2}/{3}", skillId, targetConId, targetCoordX, targetCoordY);
            Connection.SendPacket(new SendCastSkill(Connection.Id, skillId, targetConId, targetCoordX, targetCoordY));
        }
    }
}