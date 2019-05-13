using System.Collections.Concurrent;
using AikaEmu.Shared.Utils;

namespace AikaEmu.Shared.Model
{
    public class IdFactory<T> : Singleton<T> where T : class
    {
        private volatile uint _nextId;
        private readonly ConcurrentQueue<uint> _freeIdList = new ConcurrentQueue<uint>();

        public void Init(uint firstId = 1)
        {
            _nextId = firstId;
        }

        public uint GetNextId()
        {
            return _freeIdList.TryDequeue(out var result) ? result : _nextId++;
        }

        public void ReseleseId(uint id)
        {
            if (id <= 0) return;
            _freeIdList.Enqueue(id);
        }
    }
}