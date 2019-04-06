using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AikaEmu.Shared.Conversions;
using AikaEmu.Shared.Network.Packets;

namespace AikaEmu.Shared.Network
{
    public class PacketStream
    {
        #region Data

        private const int DefaultSize = 128;

        #endregion // Data

        #region Properties

        private Stream stream;
        public byte[] Buffer { get; private set; }
        public int Count { get; private set; }
        public int Pos { get; set; }
        public bool IsLittleEndian { get; set; }
        private EndianBitConverter Converter => (IsLittleEndian ? EndianBitConverter.Little : (EndianBitConverter) EndianBitConverter.Big);

        #endregion

        #region Operators & Casts

        public byte this[int index]
        {
            set => Buffer[index] = value;
            get => Buffer[index];
        }

        public static explicit operator PacketStream(byte[] o)
        {
            return new PacketStream(o);
        }

        public static implicit operator byte[](PacketStream o)
        {
            return o.GetBytes();
        }

        #endregion // Operators & Casts

        #region Constructor

        public PacketStream() : this(DefaultSize)
        {
        }

        public PacketStream(int count)
        {
            IsLittleEndian = true;
            Reserve(count);
        }

        public PacketStream(PacketStream sourcePacketStream)
        {
            IsLittleEndian = sourcePacketStream.IsLittleEndian;
            Replace(sourcePacketStream);
        }

        public PacketStream(byte[] sourcebytes)
        {
            IsLittleEndian = true;
            Replace(sourcebytes);
        }

        #endregion // Constructor

        #region Replace

        public PacketStream Replace(PacketStream stream)
        {
            return Replace(stream.Buffer, 0, stream.Count);
        }

        public PacketStream Replace(byte[] bytes)
        {
            return Replace(bytes, 0, bytes.Length);
        }

        public PacketStream Replace(PacketStream stream, int offset, int count)
        {
            return Replace(stream.Buffer, offset, count);
        }

        public PacketStream Replace(byte[] bytes, int offset, int count)
        {
            Reserve(count);
            var newBuff = new byte[count];
            System.Buffer.BlockCopy(bytes, offset, newBuff, 0, count);
            Buffer = newBuff;
            Count = count;

            return this;
        }

        #endregion // Replace

        #region Misc

        private byte[] Roundup(int length)
        {
            var i = 16;
            while (length > i)
                i <<= 1;
            return new byte[i];
        }

        public void Reserve(int count)
        {
            if (Buffer == null)
            {
                Buffer = Roundup(count);
            }
            else if (count > Buffer.Length)
            {
                var newBuffer = Roundup(count);
                System.Buffer.BlockCopy(Buffer, 0, newBuffer, 0, Count);
                Buffer = newBuffer;
            }
        }

        public PacketStream Insert(int offset, byte[] copyArray)
        {
            return Insert(offset, copyArray, 0, copyArray.Length);
        }

        public PacketStream PushBack(byte b)
        {
            Reserve(Count + 1);
            Buffer[(Count++)] = b;
            return this;
        }

        public PacketStream Insert(int offset, byte[] copyArray, int copyArrayOffset, int count)
        {
            Reserve(Count + count);
            System.Buffer.BlockCopy(Buffer, offset, Buffer, offset + count, Count - offset);
            System.Buffer.BlockCopy(copyArray, copyArrayOffset, Buffer, offset, count);
            Count += count;
            return this;
        }

        #endregion

        #region GetBytes

        public byte[] GetBytes()
        {
            var temp = new byte[Count];
            System.Buffer.BlockCopy(Buffer, 0, temp, 0, Count);
            return temp;
        }

        #endregion // GetBytes

        #region Write Primitive Types

        public PacketStream Write(bool value)
        {
            return Write(value ? (byte) 0x01 : (byte) 0x00);
        }

        public PacketStream Write(byte value)
        {
            PushBack(value);
            return this;
        }

        public PacketStream Write(byte[] value, bool appendSize = false)
        {
            if (appendSize)
                Write((ushort) (value.Length + 2));
            return Insert(Count, value);
        }

        public PacketStream Write(sbyte value)
        {
            return Write((byte) value);
        }

        public PacketStream Write(char value)
        {
            return Write(Converter.GetBytes(value));
        }

        public PacketStream Write(char[] value)
        {
            foreach (var ch in value)
                Write(ch);
            return this;
        }

        public PacketStream Write(short value)
        {
            return Write(Converter.GetBytes(value));
        }

        public PacketStream Write(int value)
        {
            return Write(Converter.GetBytes(value));
        }

        public PacketStream Write(long value)
        {
            return Write(Converter.GetBytes(value));
        }

        public PacketStream Write(ushort value)
        {
            return Write(Converter.GetBytes(value));
        }

        public PacketStream Write(uint value)
        {
            return Write(Converter.GetBytes(value));
        }

        public PacketStream Write(ulong value)
        {
            return Write(Converter.GetBytes(value));
        }

        public PacketStream Write(float value)
        {
            return Write(Converter.GetBytes(value));
        }

        public PacketStream Write(double value)
        {
            return Write(Converter.GetBytes(value));
        }

        public PacketStream WriteBc(uint value)
        {
            return Write(Converter.GetBytes(value, 3));
        }

        #endregion // Write Primitive Types

        #region Write Complex Types

        public PacketStream Write(BasePacket value)
        {
            return value.Write(this);
        }

        public PacketStream Write(PacketStream value, bool appendSize = false)
        {
            return Write(value.GetBytes(), appendSize);
        }

        #endregion // Write Complex Types

        #region Read Primitive Types

        public bool ReadBoolean()
        {
            return ReadByte() == 1;
        }

        public byte ReadByte()
        {
            if (Pos + 1 > Count)
                throw new Exception();
            return this[Pos++];
        }

        public byte[] ReadBytes(int count)
        {
            if (Pos + count > Count)
                throw new Exception();

            var result = new byte[count];
            System.Buffer.BlockCopy(Buffer, Pos, result, 0, count);
            Pos += count;
            return result;
        }

        public char ReadChar()
        {
            if (Pos + 2 > Count)
                throw new Exception();

            var result = Converter.ToChar(Buffer, Pos);
            Pos += 2;

            return result;
        }

        public char[] ReadChars(int count)
        {
            if (Pos + 2 * count > Count)
                throw new Exception();

            var result = new char[count];
            for (var i = 0; i < count; i++)
                result[i] = ReadChar();

            return result;
        }

        public short ReadInt16()
        {
            if (Pos + 2 > Count)
                throw new Exception();

            var result = Converter.ToInt16(Buffer, Pos);
            Pos += 2;

            return result;
        }

        public int ReadInt32()
        {
            if (Pos + 4 > Count)
                throw new Exception();

            var result = Converter.ToInt32(Buffer, Pos);
            Pos += 4;

            return result;
        }

        public long ReadInt64()
        {
            if (Pos + 8 > Count)
                throw new Exception();

            var result = Converter.ToInt64(Buffer, Pos);
            Pos += 8;

            return result;
        }

        public ushort ReadUInt16()
        {
            if (Pos + 2 > Count)
                throw new Exception();

            var result = Converter.ToUInt16(Buffer, Pos);
            Pos += 2;

            return result;
        }

        public uint ReadUInt32()
        {
            if (Pos + 4 > Count)
                throw new Exception();

            var result = Converter.ToUInt32(Buffer, Pos);
            Pos += 4;

            return result;
        }

        public uint ReadBc()
        {
            if (Pos + 3 > Count)
                throw new Exception();

            var result = ReadUInt16() + (ReadByte() << 16);

            return (uint) result;
        }

        public ulong ReadUInt64()
        {
            if (Pos + 8 > Count)
                throw new Exception();

            var result = Converter.ToUInt64(Buffer, Pos);
            Pos += 8;

            return result;
        }

        public float ReadSingle()
        {
            if (Pos + 4 > Count)
                throw new Exception();

            var result = Converter.ToSingle(Buffer, Pos);
            Pos += 4;

            return result;
        }

        public double ReadDouble()
        {
            if (Pos + 8 > Count)
                throw new Exception();

            var result = Converter.ToDouble(Buffer, Pos);
            Pos += 8;

            return result;
        }

        #endregion // Read Primitive Types
    }
}