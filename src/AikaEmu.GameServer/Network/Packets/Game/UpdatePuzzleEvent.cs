using AikaEmu.GameServer.Models.Puzzle.Const;
using AikaEmu.GameServer.Models.Units.Character;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
	public class UpdatePuzzleEvent : GamePacket
	{
		private readonly Character _character;

		public UpdatePuzzleEvent(Character character)
		{
			_character = character;
			Opcode = (ushort) GameOpcode.UpdatePuzzleEvent;
			SenderId = character.Connection.Id;
		}

		public override PacketStream Write(PacketStream stream)
		{
			var type = (PuzzleUpdateAction) 2;
			stream.Write((int) type); // typeId
			stream.Write(0); // unk
			stream.Write(_character.Account.DbId);
			stream.Write(1); // unk
			switch (type)
			{
				case PuzzleUpdateAction.Info:
				{
					for (var i = 0; i < 27; i++)
					{
						stream.Write((byte) i); // letters
					}

					stream.Write((byte) 0); // empty fill

					for (var i = 0; i < 24; i++)
					{
						stream.Write((byte) i); // words
					}
				}
					break;
				case PuzzleUpdateAction.AddLetter:
					stream.Write(0); // letter index 0-26
					stream.Write(0); // quantity
					stream.Write("", 44); // fill
					break;
				case PuzzleUpdateAction.AddCoupon: 
					stream.Write(0); // word index 0-20 (last ones does not exist)
					stream.Write(0); // quantity
					for (var i = 0; i < 27; i++)
					{
						stream.Write((byte)i); // letters
					}

					stream.Write("", 17); // fill?
					break;
				case PuzzleUpdateAction.AddCoupon2: // Dont update letters
					stream.Write(0);
					stream.Write(0); // word index 0-20
					stream.Write(0); // quantity
					stream.Write("", 40); // fill
					break;
			}

			return stream;
		}
	}
}