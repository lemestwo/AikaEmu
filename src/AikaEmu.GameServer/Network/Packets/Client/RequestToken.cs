using AikaEmu.GameServer.Managers;
using AikaEmu.GameServer.Models.Units.Const;
using AikaEmu.GameServer.Models.World.Nation;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.GameServer.Network.Packets.Game;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Client
{
    public class RequestToken : GamePacket
    {
        protected override void Read(PacketStream stream)
        {
            var charSlot = (byte) stream.ReadUInt16();
            var tokenStage = stream.ReadUInt16();
            var token = stream.ReadString(4);
            var token2 = stream.ReadString(4); // right one

            var character = Connection.Account.GetSlotCharacter(charSlot);
            if (character == null) return;

            Connection.Account.ActiveCharacter = character;
            character.Init();
            character.ActivatePran();

            for (var i = 0; i < 4; i++)
            {
                Connection.SendPacket(new UnkTitleLink(character));
                character.Titles.SendTitles();
            }

            Connection.SendPacket(new Unk1031());
            Connection.SendPacket(new Unk303D(character, 0));
            Connection.SendPacket(new Unk102C());
            // Client -> 3CBE

            if (character.ActivePran != null)
            {
                var pran = character.ActivePran;
                pran.Spawn();
                Connection.SendPacket(new SendUnitSpawn(pran, 2));
                Connection.SendPacket(new SetEffectOnHead(pran.Id, EffectType.Default));
                Connection.SendPacket(new UpdatePranExperience(pran));
                Connection.SendPacket(new SendPranToWorld(pran));
            }


            Connection.SendPacket(new Unk303D(character, 1));

            // TODO - Update pran Stone in Bank (84/85)
            Connection.SendPacket(new Unk101F(Connection.Id));
            Connection.SendPacket(new UpdateDungeonTimer());

            // Send to world

            Connection.SendPacket(new SendToWorld(character));

            Connection.SendPacket(new UpdateStatus());
            Connection.SendPacket(new UpdateAttributes(character));
            Connection.SendPacket(new UpdateExperience(character));

            Connection.SendPacket(new Unk1C41(character.Account));
            Connection.SendPacket(new InitialUnk2027());

            // TODO - Sends quests
            Connection.SendPacket(new Unk303D(character, 0));

            var nation = new Nation
            {
                Id = 2,
                Name = "test",
                Settlement = 10000,
                TaxCitizen = 10,
                TaxVisitor = 15,
                StabilizationTime = 78
            };

            // if have nation send all 4, otherwise only CurNationInfo
            Connection.SendPacket(new UpdateNationGovernment(Connection.Id, nation));
            Connection.SendPacket(new UpdateSiegeInfo());
            if (character.Account.NationId > 0)
                Connection.SendPacket(new UpdateReliques(character.Account.NationId));
            Connection.SendPacket(new CurNationInfo(nation));
            Connection.SendPacket(new Unk303D(character, 1));

            WorldManager.Instance.Spawn(character);
            Connection.SendPacket(new SendUnitSpawn(character, 1));
            WorldManager.Instance.ShowVisibleUnits(character);

            //            Connection.SendPacket(new SendUnitSpawn(pran, true));
            //            Connection.SendPacket(new SetEffectOnHead(pran.Id, 1));

            character.Friends.SendFriends();
            Connection.SendPacket(new ApplyBuff(0));
            Connection.SendPacket(new UpdateHpMp(character));
            Connection.SendPacket(new UpdateAttributes(character));
            Connection.SendPacket(new UpdateHpMp(character));
            Connection.SendPacket(new UpdateStatus());
            // UpdateStoreItems npc 2053 ????
            Connection.SendPacket(new UpdateAccountLevel(Connection.Account));
            Connection.SendPacket(new UpdatePremiumStash());

            Connection.SendPacket(new Unk3057());
            Connection.SendPacket(new UnkTitleLink(character));
            character.Titles.SendTitles();
            Connection.SendPacket(new UnkTitleLink(character));
            character.Titles.SendTitles();

            Connection.SendPacket(new UpdateAccountLevel(Connection.Account));
            Connection.SendPacket(new UpdateCash(10000));
            Connection.SendPacket(new FinishedInGameState());
            Connection.SendPacket(new Unk3C7C(Connection.Id));
            Connection.SendPacket(new UpdatePuzzleEvent(character));
        }
    }
}