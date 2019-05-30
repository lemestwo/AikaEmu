using System.Collections.Generic;

namespace AikaEmu.GameServer.Models.Data
{
    public abstract class BaseData<T> where T : class
    {
        protected readonly Dictionary<ushort, T> Objects;
        public int Count => Objects.Count;

        protected BaseData()
        {
            Objects = new Dictionary<ushort, T>();
        }

        public virtual T GetData(ushort id)
        {
            return Objects.ContainsKey(id) ? Objects[id] : null;
        }
    }
}