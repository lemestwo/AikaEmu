using AikaEmu.GameServer.Models.Data.JsonModel;
using AikaEmu.Shared.Utils;

namespace AikaEmu.GameServer.Models.Data
{
	public class CharacterData
	{
		public CharacterConfigJson Data { get; }

		public CharacterData(string path)
		{
			JsonUtil.DeserializeFile(path, out CharacterConfigJson data);
			Data = data;
		}

		public Classes GetInitial(ushort id)
		{
			foreach (var c in Data.Classes)
			{
				if (c.Class == id) return c;
			}

			return null;
		}
	}
}