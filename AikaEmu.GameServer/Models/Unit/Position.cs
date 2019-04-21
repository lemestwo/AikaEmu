using System;

namespace AikaEmu.GameServer.Models.Unit
{
	public class Position : ICloneable
	{
		public byte WorldId { get; set; }
		public float CoordX { get; set; }
		public float CoordY { get; set; }
		public int Rotation { get; set; }

		public object Clone()
		{
			return MemberwiseClone();
		}
	}
}