using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Client
{
    public class CreateCharacter : GamePacket
    {
        protected override void Read(PacketStream stream)
        {
            var accId = stream.ReadUInt32();
            var slot = (byte) stream.ReadUInt32();
            var name = stream.ReadString(16);
            var face = stream.ReadUInt16();
            var hair = stream.ReadUInt16();
            stream.ReadBytes(12);
            var isRanch = stream.ReadInt32() == 1;

            var acc = Connection.Account;
            if (acc.DbId != accId)
            {
                Connection.Close();
                // TODO - BAN
                return;
            }

            Connection.Account.CreateCharacter(slot, name, face, hair, isRanch);
        }
    }
}