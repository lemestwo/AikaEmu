using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.GameServer.Network.Packets.Game;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Client
{
    public class RequestUseSkill : GamePacket
    {
        protected override void Read(PacketStream stream)
        {
            var targetConId = stream.ReadUInt16();
            stream.ReadInt16();
            stream.ReadUInt32(); // unk
            stream.ReadUInt32(); // unk
            stream.ReadUInt32(); // unk
            var unkType = stream.ReadUInt16();
            var skillId = stream.ReadUInt16();
            var coordX = stream.ReadSingle();
            var coordY = stream.ReadSingle();
            var tCoordX = stream.ReadSingle();
            var tCoordY = stream.ReadSingle();

            Connection.SendPacket(new UpdateWithSkillEffect(Connection.Id, targetConId, unkType, skillId, Connection.ActiveCharacter.Position));
        }
    }
}