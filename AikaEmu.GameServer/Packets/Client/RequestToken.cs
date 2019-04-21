using AikaEmu.GameServer.Managers;
using AikaEmu.GameServer.Models;
using AikaEmu.GameServer.Models.Unit;
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
			Log.Debug("Token: ({0}) {2}, Action: {3}.", charSlot, token2, tokenStage);

			var character = Connection.Account.GetSlotCharacter(charSlot);
			if (character == null) return;

			Connection.Account.ActiveCharacter = character;
			WorldManager.Instance.Spawn(character);


			for (var i = 0; i < 4; i++)
			{
				Connection.SendPacket(new UnkTitleLink(character));
				Connection.SendPacket(new UnkTitleLink2());
			}

			var pran = new Pran
			{
				Id = 10270,
				Hp = 2000,
				Mp = 2000,
				MaxHp = 2000,
				MaxMp = 2000,
				Name = "TestPran",
				Position = new Position
				{
					WorldId = 1,
					CoordX = Connection.ActiveCharacter.Position.CoordX + 2.0f,
					CoordY = Connection.ActiveCharacter.Position.CoordY + 2.0f
				},
				BodyTemplate = new BodyTemplate
				{
					Width = 7,
					Chest = 118,
					Leg = 119,
					Body = 0
				},
				Account = Connection.Account,
				Experience = 1000000000,
				MDef = 100,
				PDef = 100,
				Level = 50,
			};

			Connection.SendPacket(new SendUnitSpawn(pran));
			Connection.SendPacket(new SendPranToWorld(pran));

			Connection.SendPacket(new Unk1031());
			Connection.SendPacket(new Unk102C());
//
//            Connection.SendPacket(new UpdateItem(84));
//            Connection.SendPacket(new PranEffect(108, 0));
//            Connection.SendPacket(new UpdateItem(85));
//            Connection.SendPacket(new PranEffect(108, 0));
//            Connection.SendPacket(new Unk101F());
//            Connection.SendPacket(new UpdateDungeonTimer());

			Connection.SendPacket(new SendToWorld(character));
			Connection.SendPacket(new SendUnitSpawn(pran, true));

			Connection.SendPacket(new UpdatePranExperience(pran));
			Connection.SendPacket(new SetEffectOnHead(10000, 2));

//            Connection.SendPacket(new UpdateStatus());
//            Connection.SendPacket(new UpdateAttributes(character.CharAttributes));
//            Connection.SendPacket(new UpdateLevel(character));
//
//
//            Connection.SendPacket(new Unk1C41());
			Connection.SendPacket(new InitialUnk2027());
			Connection.SendPacket(new Unk303D(character));
			Connection.SendPacket(new CurNationInfo());

			Connection.SendPacket(new UpdateSiegeInfo()); // TODO - move better place
			Connection.SendPacket(new UpdateReliques()); // TODO - move better place

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