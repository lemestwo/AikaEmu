using AikaEmu.GameServer.Models.Puzzle.Const;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.GameServer.Network.Packets.Game;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Client
{
	public class PuzzleEventRequest : GamePacket
	{
		protected override void Read(PacketStream stream)
		{
			var action = (PuzzleRequestAction) stream.ReadInt32();

			switch (action)
			{
				case PuzzleRequestAction.Update:
					Connection.SendPacket(new UpdatePuzzleEvent(Connection.ActiveCharacter)); // TODO
					break;
				case PuzzleRequestAction.BuyWord:
				{
					var wordIndex = stream.ReadInt32();
					var quantity = stream.ReadInt32();
					// TODO
				}
					break;
				case PuzzleRequestAction.BuyItem:
				{
					var itemIndex = stream.ReadInt32();
					// TODO
				}
					break;
				default:
					Log.Debug("PuzzleEventRequest, Action not found: {0}", (int) action);
					break;
			}
		}
	}
}