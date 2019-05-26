using AikaEmu.GameServer.Managers;
using AikaEmu.GameServer.Models.World.Nation;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class UpdateReliques : GamePacket
    {
        private readonly NationId _nationId;

        public UpdateReliques(NationId nationId)
        {
            _nationId = nationId;

            Opcode = (ushort) GameOpcode.UpdateReliques;
            SenderId = 0;
        }

        public override PacketStream Write(PacketStream stream)
        {
            var reliques = NationManager.Instance.GetReliquesList(_nationId);

            foreach (var relique in reliques)
                stream.Write(relique);

            for (var i = 0; i < 24 - reliques.Count; i++)
                stream.Write((ushort) 0);

            stream.Write((ushort) 0); // unk
            stream.Write((ushort) 0); // triggers event when =1 or =2
            // if 1 - lock all effects and open the special effect tab
            // 2 - unlock all effects and special effects
            // 0 - hide special effects
            return stream;
        }
    }
}