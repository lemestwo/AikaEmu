using AikaEmu.GameServer.Models;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.GameServer.Packets.Game;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Packets.Client
{
    public class RequestToken : GamePacket
    {
        protected override void Read(PacketStream stream)
        {
            var charSlot = stream.ReadUInt16();
            var tokenStage = stream.ReadUInt16();
            var token = stream.ReadString(4);
            var token2 = stream.ReadString(4); // right one
            Log.Debug("Token: ({0}) {1}/{2}, Action: {3}.", charSlot, token, token2, tokenStage);

            var character = Connection.Account.GetSlotCharacter(charSlot);
            if (character == null) return;

            character.Connection = Connection;
            Connection.ActiveCharacter = character;

//            Connection.SendPacket(new Unk30A6());
//            Connection.SendPacket(new Unk1086());
//            Connection.SendPacket(new Unk30A6());
//            Connection.SendPacket(new Unk1086());
//            Connection.SendPacket(new Unk30A6());
//            Connection.SendPacket(new Unk1086());
//            Connection.SendPacket(new Unk30A6());
//            Connection.SendPacket(new Unk1086());
//            Connection.SendPacket(new Unk1031());
//            Connection.SendPacket(new Unk303D());
//            Connection.SendPacket(new Unk102C());
//
//            Connection.SendPacket(new UpdateItem(84));
//            Connection.SendPacket(new PranEffect(108, 0));
//            Connection.SendPacket(new UpdateItem(85));
//            Connection.SendPacket(new PranEffect(108, 0));
//            Connection.SendPacket(new Unk101F());
//            Connection.SendPacket(new UpdateDungeonTimer());

            Connection.SendPacket(new SendToWorld(character));

//            Connection.SendPacket(new UpdateStatus());
//            Connection.SendPacket(new UpdateAttributes(character.CharAttributes));
//            Connection.SendPacket(new UpdateLevel(character));
//
//
//            Connection.SendPacket(new Unk1C41());
//            Connection.SendPacket(new Unk2027());
//            Connection.SendPacket(new Unk303D());
//            Connection.SendPacket(new Unk1028());

            Connection.SendPacket(new SendUnitSpawn(character));

//            Connection.SendPacket(new Unk106F());
//            Connection.SendPacket(new UpdateHpMp(character));
//            Connection.SendPacket(new UpdateAttributes(character.CharAttributes));
//            Connection.SendPacket(new UpdateHpMp(character));
//            Connection.SendPacket(new UpdateStatus());
//            Connection.SendPacket(new UpdateStoreItem());
//            Connection.SendPacket(new UpdateAccountLevel(0));
//            Connection.SendPacket(new UpdatePremiumStash());
//
//            Connection.SendPacket(new Unk3057());
//            Connection.SendPacket(new Unk30A6());
//            Connection.SendPacket(new Unk1086());
//            Connection.SendPacket(new Unk30A6());
//            Connection.SendPacket(new UpdateAccountLevel(0));
//            Connection.SendPacket(new XTrap(0));
//            Connection.SendPacket(new Unk1086());
//
//            Connection.SendPacket(new Unk3CBE());
//            Connection.SendPacket(new Unk3F34());
//            Connection.SendPacket(new Unk3F1B());
        }
    }
}