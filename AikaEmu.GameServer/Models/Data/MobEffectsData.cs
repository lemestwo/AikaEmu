using System.Collections.Generic;
using AikaEmu.GameServer.Models.Data.JsonModel;
using AikaEmu.Shared.Utils;

namespace AikaEmu.GameServer.Models.Data
{
	public class MobEffectsData
	{
		private readonly Dictionary<ushort, MobEffectsJson> _mobEffects;

		public int Count => _mobEffects.Count;

		public MobEffectsData(string path)
		{
			_mobEffects = new Dictionary<ushort, MobEffectsJson>();
			JsonUtil.DeserializeFile(path, out List<MobEffectsJson> mobEffectsData);
			foreach (var effectsList in mobEffectsData)
				_mobEffects.Add(effectsList.Id, effectsList);
		}

		public ushort GetFace(ushort id)
		{
			return _mobEffects.ContainsKey(id) ? _mobEffects[id].Face : (ushort) 0;
		}
	}
}