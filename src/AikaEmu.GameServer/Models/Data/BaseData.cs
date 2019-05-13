using System.Collections.Generic;

namespace AikaEmu.GameServer.Models.Data
{
    public abstract class BaseData<T>
    {
        protected readonly Dictionary<ushort, T> Objects;
        public int Count => Objects.Count;

        protected BaseData()
        {
            Objects = new Dictionary<ushort, T>();
        }
    }
}