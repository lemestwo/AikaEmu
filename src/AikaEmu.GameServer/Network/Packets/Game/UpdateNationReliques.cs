using AikaEmu.GameServer.Managers;
using AikaEmu.GameServer.Models.World.Nation;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class UpdateNationReliques : GamePacket
    {
        private readonly NationId _nationId;

        public UpdateNationReliques(NationId nationId)
        {
            _nationId = nationId;

            Opcode = (ushort) GameOpcode.UpdateNationReliques;
            SenderId = 0;
        }

        public override PacketStream Write(PacketStream stream)
        {
            var devirs = NationManager.Instance.GetNationDevirs(_nationId);
            if (devirs == null) return stream;

            stream.Write((uint) _nationId);
            foreach (var reliques in devirs.Values)
            foreach (var slots in reliques.Slots.Values)
            {
                stream.Write(slots.ItemId);
                stream.Write(slots.ItemId);
                stream.Write(slots.ItemData?.GearCoreLevel ?? 0u);
                // TODO - if < than now time = 0
                // TODO - this time is time + charge time (6 hours)
                stream.Write(slots.Time);
                stream.Write((short) 0);
                stream.Write((byte) 2);
                stream.Write((byte) 1);
                stream.Write(0);
            }

            foreach (var reliques in devirs.Values)
            foreach (var slots in reliques.Slots.Values)
            {
                stream.Write((uint) slots.ItemId);
                stream.Write(slots.Name, 16);
                stream.Write(slots.Time);
                stream.Write(slots.IsActive, true);
            }

            stream.Write("", 24);
            return stream;
        }
    }
}