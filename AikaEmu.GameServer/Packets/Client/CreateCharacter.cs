using System.Configuration;
using AikaEmu.GameServer.Models.Char;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Packets.Client
{
    public class CreateCharacter : GamePacket
    {
        protected override void Read(PacketStream stream)
        {
            var accId = stream.ReadUInt32();
            var slot = stream.ReadUInt32();
            var name = stream.ReadString(16);
            var face = (CharFace) stream.ReadUInt16();
            var hair = (CharHair) stream.ReadUInt16();
            stream.ReadBytes(12);
            var isRanch = stream.ReadInt32() == 1;

            var acc = Connection.Account;
            if (acc.Id != accId)
            {
                Connection.Close();
                // TODO - BAN
                return;
            }
            
            Connection.Account.CreateCharacter(slot, name, face, hair, isRanch);
        }
    }
}