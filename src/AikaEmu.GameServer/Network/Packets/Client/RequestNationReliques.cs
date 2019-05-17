using System;
using AikaEmu.GameServer.Models.World.Nation;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.GameServer.Network.Packets.Game;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Client
{
    public class RequestNationReliques : GamePacket
    {
        protected override void Read(PacketStream stream)
        {
            var nationId = (NationId) (stream.ReadByte() + 1);
            
            if (Enum.IsDefined(typeof(NationId), nationId))
                Connection.SendPacket(new UpdateNationReliques(nationId));
        }
    }
}