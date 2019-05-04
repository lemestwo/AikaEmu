using System.Collections.Concurrent;
using System.Net.Sockets;

namespace AikaEmu.Shared.Network
{
    public class BufferControl
    {
        private readonly int _byteLenght;
        private readonly int _bufferLenght;
        private readonly ConcurrentStack<int> _freeIndexPool;
        private int _posIndex;

        private byte[] _buffer;

        public BufferControl(int byteLenght, int bufferLenght)
        {
            _byteLenght = byteLenght;
            _bufferLenght = bufferLenght;

            _freeIndexPool = new ConcurrentStack<int>();
            _posIndex = 0;
        }

        public void Init()
        {
            _buffer = new byte[_byteLenght];
        }

        public void Empty(SocketAsyncEventArgs args)
        {
            _freeIndexPool.Push(args.Offset);
        }

        public void Set(SocketAsyncEventArgs args)
        {
            lock (_freeIndexPool)
            {
                if (_freeIndexPool.Count > 0)
                {
                    _freeIndexPool.TryPop(out var offset);
                    args.SetBuffer(_buffer, offset, _bufferLenght);
                }
                else
                {
                    if (_byteLenght - _bufferLenght < _posIndex) return;
                    args.SetBuffer(_buffer, _posIndex, _bufferLenght);
                    _posIndex += _bufferLenght;
                }
            }
        }
    }
}