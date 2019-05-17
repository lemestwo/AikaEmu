namespace AikaEmu.GameServer.Models.World.Nation
{
	public class Nation
	{
		public byte Id { get; set; }
		public string Name { get; set; }

		public byte TaxCitizen { get; set; }
		public byte TaxVisitor { get; set; }
		public ulong Settlement { get; set; }
		public short StabilizationTime { get; set; }
	}
}