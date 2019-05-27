namespace AikaEmu.GameServer.Models
{
    public class Effect
    {
        public ushort Id { get; set; }
        public ushort Value { get; set; }

        public Effect()
        {
        }

        public Effect(ushort id, ushort value)
        {
            Id = id;
            Value = value;
        }
    }
}