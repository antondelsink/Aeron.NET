using Adaptive.Agrona.Collections;
using Adaptive.Agrona.Util;
using System;
using System.Buffers.Binary;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Adaptive.Agrona
{
    public class ExpandableArrayBuffer : IMutableDirectBuffer
    {
        /// <summary>
        /// Maximum length to which the underlying buffer can grow. Some JVMs store state in the last few bytes.
        /// </summary>
        /// <remarks>
        /// public static final int MAX_ARRAY_LENGTH = Integer.MAX_VALUE - 8;
        /// </remarks>
        public const int MAX_ARRAY_LENGTH = int.MaxValue - 8;

        /// <summary>
        /// Initial capacity of the buffer from which it will expand as necessary.
        /// </summary>
        /// <remarks>
        /// public static final int INITIAL_CAPACITY = 128;
        /// </remarks>
        public const int INITIAL_CAPACITY = 128;

        public static readonly ByteOrder NATIVE_BYTE_ORDER = BitConverter.IsLittleEndian ? ByteOrder.LittleEndian : ByteOrder.BigEndian;

        private byte[] byteArray;

        public ExpandableArrayBuffer()
            : this(INITIAL_CAPACITY)
        {
        }

        public ExpandableArrayBuffer(int initialCapacity)
        {
            if (initialCapacity < 0 || initialCapacity > MAX_ARRAY_LENGTH)
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(initialCapacity));

            byteArray = new byte[initialCapacity];
        }

        bool IMutableDirectBuffer.IsExpandable => true;

        IntPtr IDirectBuffer.BufferPointer => throw new NotImplementedException();

        public byte[] ByteArray => byteArray;

        ByteBuffer IDirectBuffer.ByteBuffer => null;

        public int Capacity => byteArray.Length;

        public void BoundsCheck(int index, int length)
        {
            BufferUtil.BoundsCheck(byteArray, index, length);
        }

        public void CheckLimit(int limit)
        {
            EnsureCapacity(0, limit);
        }

        private void EnsureCapacity(int index, int length)
        {
            if (index < 0 || length < 0 || index + length > MAX_ARRAY_LENGTH)
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(length));

            byteArray = ArrayUtil.EnsureCapacity(byteArray, index + length);
        }

        int IComparable<IDirectBuffer>.CompareTo(IDirectBuffer that)
        {
            int thisCapacity = this.Capacity;
            int thatCapacity = that.Capacity;

            byte[] thisByteArray = this.ByteArray;
            byte[] thatByteArray = that.ByteArray;

            for (int i = 0, length = Math.Min(thisCapacity, thatCapacity); i < length; i++)
            {
                int cmp = thisByteArray[i].CompareTo(thatByteArray[i]);
                if (cmp != 0)
                {
                    return cmp;
                }
            }

            if (thisCapacity != thatCapacity)
            {
                return (thisCapacity - thatCapacity);
            }

            return 0;
        }

        #region Single Precision & Double Precision Floating Point (float & double)
        public float GetFloat(int index)
        {
            return BitConverter.ToSingle(byteArray, index);
        }
        public double GetDouble(int index)
        {
            return BitConverter.ToDouble(byteArray, index);
        }
        public float GetFloat(int index, ByteOrder byteOrder)
        {
            var source = byteArray.AsSpan(index, sizeof(float));

            if (byteOrder == ByteOrder.LittleEndian)
            {
                return BinaryPrimitives.ReadSingleLittleEndian(source);
            }

            if (byteOrder == ByteOrder.BigEndian)
            {
                return BinaryPrimitives.ReadSingleBigEndian(source);
            }

            return float.NaN;
        }
        public double GetDouble(int index, ByteOrder byteOrder)
        {
            var source = byteArray.AsSpan(index, sizeof(double));

            if (byteOrder == ByteOrder.LittleEndian)
            {
                return BinaryPrimitives.ReadDoubleLittleEndian(source);
            }

            if (byteOrder == ByteOrder.BigEndian)
            {
                return BinaryPrimitives.ReadDoubleBigEndian(source);
            }

            return double.NaN;
        }
        public void Put(int index, float value)
        {
            PutFloat(index, value, NATIVE_BYTE_ORDER);
        }
        public void PutFloat(int index, float value)
        {
            PutFloat(index, value, NATIVE_BYTE_ORDER);
        }
        public void PutFloat(int index, float value, ByteOrder byteOrder)
        {
            EnsureCapacity(index, sizeof(float));

            var destination = byteArray.AsSpan(index, sizeof(float));

            switch (byteOrder)
            {
                case ByteOrder.LittleEndian:
                    BinaryPrimitives.WriteSingleLittleEndian(destination, value);
                    break;
                case ByteOrder.BigEndian:
                    BinaryPrimitives.WriteSingleBigEndian(destination, value);
                    break;
                default:
                    ThrowHelper.ThrowInvalidEnumArgumentException(nameof(byteOrder), (int)byteOrder, typeof(ByteOrder));
                    break;
            }
        }
        public void Put(int index, double value)
        {
            PutDouble(index, value);
        }
        public void PutDouble(int index, double value)
        {
            PutDouble(index, value, NATIVE_BYTE_ORDER);
        }
        public void PutDouble(int index, double value, ByteOrder byteOrder)
        {
            EnsureCapacity(index, sizeof(double));

            var destination = byteArray.AsSpan(index, sizeof(double));

            switch (byteOrder)
            {
                case ByteOrder.LittleEndian:
                    BinaryPrimitives.WriteDoubleLittleEndian(destination, value);
                    break;
                case ByteOrder.BigEndian:
                    BinaryPrimitives.WriteDoubleBigEndian(destination, value);
                    break;
                default:
                    ThrowHelper.ThrowInvalidEnumArgumentException(nameof(byteOrder), (int)byteOrder, typeof(ByteOrder));
                    break;
            }
        }
        #endregion

        #region short
        public short GetShort(int index)
        {
            return BitConverter.ToInt16(byteArray, index);
        }
        public short GetShort(int index, ByteOrder byteOrder)
        {
            var source = byteArray.AsSpan(index);

            if (byteOrder == ByteOrder.LittleEndian)
            {
                return BinaryPrimitives.ReadInt16LittleEndian(source);
            }

            if (byteOrder == ByteOrder.BigEndian)
            {
                return BinaryPrimitives.ReadInt16BigEndian(source);
            }

            throw new InvalidEnumArgumentException(nameof(byteOrder), (int)byteOrder, typeof(ByteOrder));
        }
        public void Put(int index, short value)
        {
            PutShort(index, value);
        }
        public void PutShort(int index, short value)
        {
            PutShort(index, value, NATIVE_BYTE_ORDER);
        }
        public void PutShort(int index, short value, ByteOrder byteOrder)
        {
            EnsureCapacity(index, sizeof(short));

            var target = byteArray.AsSpan(index, sizeof(short));

            switch (byteOrder)
            {
                case ByteOrder.LittleEndian:
                    BinaryPrimitives.WriteInt16LittleEndian(target, value);
                    break;
                case ByteOrder.BigEndian:
                    BinaryPrimitives.WriteInt16BigEndian(target, value);
                    break;
                default:
                    ThrowHelper.ThrowInvalidEnumArgumentException(nameof(byteOrder), (int)byteOrder, typeof(ByteOrder));
                    break;
            }
        }
        #endregion

        #region UTF8
        public string GetStringUtf8(int index)
        {
            return GetStringUtf8(index, BitConverter.IsLittleEndian ? ByteOrder.LittleEndian : ByteOrder.BigEndian);
        }

        public string GetStringUtf8(int index, int length)
        {
            return Encoding.UTF8.GetString(byteArray, index, length);
        }

        public string GetStringUtf8(int index, ByteOrder byteOrder)
        {
            var stringLength = GetInt(index, byteOrder);

            return Encoding.UTF8.GetString(byteArray, index + 4, stringLength);
        }
        public string GetStringWithoutLengthUtf8(int index, int length)
        {
            throw new NotImplementedException();
        }
        public int PutStringUtf8(int index, string value)
        {
            throw new NotImplementedException();
        }

        public int PutStringUtf8(int index, string value, int maxEncodedSize)
        {
            throw new NotImplementedException();
        }
        public int PutStringWithoutLengthUtf8(int index, string value)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ASCII
        public int PutIntAscii(int index, int value)
        {
            throw new NotImplementedException();
        }
        public string GetStringWithoutLengthAscii(int index, int length)
        {
            throw new NotImplementedException();
        }
        public string GetStringAscii(int index, int length)
        {
            throw new NotImplementedException();
        }

        public string GetStringAscii(int index, ByteOrder byteOrder)
        {
            var stringLength = GetInt(index, byteOrder);

            return Encoding.ASCII.GetString(byteArray, index + 4, stringLength);
        }
        public string GetStringAscii(int index)
        {
            return GetStringAscii(index, BitConverter.IsLittleEndian ? ByteOrder.LittleEndian : ByteOrder.BigEndian);

        }
        public int GetStringAscii(int index, StringBuilder appendable)
        {
            throw new NotImplementedException();
        }
        public int GetStringAscii(int index, int length, StringBuilder appendable)
        {
            throw new NotImplementedException();
        }
        public int PutLongAscii(int index, long value)
        {
            throw new NotImplementedException();
        }
        public int PutStringAscii(int index, string value)
        {
            throw new NotImplementedException();
        }
        public int PutStringWithoutLengthAscii(int index, string value)
        {
            throw new NotImplementedException();
        }

        public int PutStringWithoutLengthAscii(int index, string value, int valueOffset, int length)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region byte
        public byte GetByte(int index)
        {
            return byteArray[index];
        }
        public void GetBytes(int index, byte[] dst)
        {
            Array.Copy(byteArray, index, dst, 0, dst.Length);
        }
        public void GetBytes(int index, byte[] dst, int offset, int length)
        {
            Array.Copy(byteArray, index, dst, offset, length);
        }
        public void Put(int index, byte value)
        {
            PutByte(index, value);
        }
        public void PutByte(int index, byte value)
        {
            EnsureCapacity(index, sizeof(byte));

            byteArray[index] = value;
        }
        public void PutBytes(int index, byte[] src)
        {
            EnsureCapacity(index, src.Length);

            Array.Copy(src, 0, byteArray, 0, src.Length);
        }
        public void PutBytes(int index, byte[] src, int offset, int length)
        {
            EnsureCapacity(index, length);

            Array.Copy(src, offset, byteArray, index, length);
        }
        public void GetBytes(int index, IMutableDirectBuffer dstBuffer, int dstIndex, int length)
        {
            Array.Copy(byteArray, index, dstBuffer.ByteArray, dstIndex, length);
        }
        public void PutBytes(int index, IDirectBuffer srcBuffer, int srcIndex, int length)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region char
        public char GetChar(int index)
        {
            return (char)byteArray[index];
        }
        public void Put(int index, char value)
        {
            PutChar(index, value);
        }
        public void PutChar(int index, char value)
        {
            EnsureCapacity(index, BitUtil.SIZE_OF_CHAR); // SIZE_OF_CHAR == 1 and .Net C# sizeof(char) == 2

            byteArray[index] = (byte)value;
        }
        #endregion

        #region Int64 aka long
        public long GetLong(int index, ByteOrder byteOrder)
        {
            var source = byteArray.AsSpan(index, sizeof(long));

            if (byteOrder == ByteOrder.LittleEndian)
            {
                return BinaryPrimitives.ReadInt64LittleEndian(source);
            }

            if (byteOrder == ByteOrder.BigEndian)
            {
                return BinaryPrimitives.ReadInt64BigEndian(source);
            }

            throw new InvalidEnumArgumentException(nameof(byteOrder), (int)byteOrder, typeof(ByteOrder));
        }
        public long GetLong(int index)
        {
            return BitConverter.ToInt64(byteArray, index);
        }
        public void Put(int index, long value)
        {
            PutLong(index, value);
        }
        public void PutLong(int index, long value)
        {
            PutLong(index, value, NATIVE_BYTE_ORDER);
        }
        public void PutLong(int index, long value, ByteOrder byteOrder)
        {
            EnsureCapacity(index, sizeof(long));

            var target = byteArray.AsSpan(index, sizeof(long));

            switch (byteOrder)
            {
                case ByteOrder.BigEndian:
                    BinaryPrimitives.WriteInt64BigEndian(target, value);
                    break;
                case ByteOrder.LittleEndian:
                    BinaryPrimitives.WriteInt64LittleEndian(target, value);
                    break;
                default:
                    ThrowHelper.ThrowInvalidEnumArgumentException(nameof(byteOrder), (int)byteOrder, typeof(ByteOrder));
                    break;
            }
        }
        #endregion

        #region Int32 aka int
        public int GetInt(int index, ByteOrder byteOrder)
        {
            var source = byteArray.AsSpan(index, sizeof(int));

            if (byteOrder == ByteOrder.LittleEndian)
            {
                return BinaryPrimitives.ReadInt32LittleEndian(source);
            }

            if (byteOrder == ByteOrder.BigEndian)
            {
                return BinaryPrimitives.ReadInt32BigEndian(source);
            }

            throw new InvalidEnumArgumentException(nameof(byteOrder), (int)byteOrder, typeof(ByteOrder));
        }
        public int GetInt(int index)
        {
            return BitConverter.ToInt32(byteArray, index);
        }
        public void Put(int index, int value)
        {
            PutInt(index, value);
        }
        public void PutInt(int index, int value)
        {
            PutInt(index, value, NATIVE_BYTE_ORDER);
        }
        public void PutInt(int index, int value, ByteOrder byteOrder)
        {
            EnsureCapacity(index, sizeof(int));

            var target = byteArray.AsSpan(index, sizeof(int));

            switch (byteOrder)
            {
                case ByteOrder.BigEndian:
                    BinaryPrimitives.WriteInt32BigEndian(target, value);
                    break;
                case ByteOrder.LittleEndian:
                    BinaryPrimitives.WriteInt32LittleEndian(target, value);
                    break;
                default:
                    ThrowHelper.ThrowInvalidEnumArgumentException(nameof(byteOrder), (int)byteOrder, typeof(ByteOrder));
                    break;
            }
        }
        #endregion

        public void SetMemory(int index, int length, byte value)
        {
            BufferUtil.BoundsCheck(byteArray, index, length);

            Array.Fill(byteArray, value, index, length);
        }

        #region Java implementation throws UnsupportedOperationException
        void IDirectBuffer.Wrap(byte[] buffer)
        {
            throw new InvalidOperationException();
        }

        void IDirectBuffer.Wrap(byte[] buffer, int offset, int length)
        {
            throw new InvalidOperationException();
        }

        void IDirectBuffer.Wrap(IDirectBuffer buffer)
        {
            throw new InvalidOperationException();
        }

        void IDirectBuffer.Wrap(IDirectBuffer buffer, int offset, int length)
        {
            throw new InvalidOperationException();
        }

        void IDirectBuffer.Wrap(IntPtr pointer, int length)
        {
            throw new InvalidOperationException();
        }

        void IDirectBuffer.Wrap(IntPtr pointer, int offset, int length)
        {
            throw new InvalidOperationException();
        }
        #endregion

        public override string ToString()
        {
            if (byteArray is null ||
                byteArray.Length == 0 ||
                byteArray.All(b => b == 0))
            {
                return string.Empty;
            }
            
            const char placeholderChar = '?';
            var printableChars = Encoding.ASCII.GetChars(byteArray).Select((c) => char.IsLetterOrDigit(c) || char.IsPunctuation(c) ? c : placeholderChar);
            var truncatedByteArrayAsString = string.Concat(printableChars).TrimEnd(placeholderChar);

            const char separator = ',';
            var numbers = from b in byteArray
                          select b.ToString();

            var trimLength = 20;
            var truncatedByteArrayAsNumbersString = string.Join(separator, numbers.Take(trimLength));

            if (trimLength < numbers.Count())
                truncatedByteArrayAsNumbersString += ", ...";

            return $"ExpandableArrayBuffer{{byteArray.Length=={byteArray.Length}, byteArray==\"{truncatedByteArrayAsString}\", byteArray=[{truncatedByteArrayAsNumbersString}]}}";
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
                return true;

            var that = obj as ExpandableArrayBuffer;

            if (that is null)
                return false;

            return Enumerable.SequenceEqual(this.byteArray, that.byteArray);
        }
    }
}