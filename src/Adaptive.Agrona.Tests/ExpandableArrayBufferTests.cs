using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Adaptive.Agrona.BitUtil;

namespace Adaptive.Agrona.Tests
{
    [TestFixture]
    public class ExpandableArrayBufferTests
    {
        [Category("ExpandableArrayBuffer")]
        [TestCase(ByteOrder.LittleEndian, 0, 0)]
        [TestCase(ByteOrder.LittleEndian, 0, 1)]
        [TestCase(ByteOrder.LittleEndian, 0, -1)]
        [TestCase(ByteOrder.LittleEndian, 0, int.MinValue)]
        [TestCase(ByteOrder.LittleEndian, 0, int.MaxValue)]
        [TestCase(ByteOrder.BigEndian, 0, 0)]
        [TestCase(ByteOrder.BigEndian, 0, 1)]
        [TestCase(ByteOrder.BigEndian, 0, -1)]
        [TestCase(ByteOrder.BigEndian, 0, int.MinValue)]
        [TestCase(ByteOrder.BigEndian, 0, int.MaxValue)]
        public void Test_Int(ByteOrder byteOrder, int index, int input)
        {
            var buffer = new ExpandableArrayBuffer();
            buffer.PutInt(index, input, byteOrder);

            var result = (byteOrder == ExpandableArrayBuffer.NATIVE_BYTE_ORDER) ? buffer.GetInt(index) : buffer.GetInt(index, byteOrder);

            Assert.AreEqual(input, result);
        }

        [Category("ExpandableArrayBuffer")]
        [TestCase(ByteOrder.LittleEndian, 0, 0)]
        [TestCase(ByteOrder.LittleEndian, 0, 1)]
        [TestCase(ByteOrder.LittleEndian, 0, -1)]
        [TestCase(ByteOrder.LittleEndian, 0, long.MinValue)]
        [TestCase(ByteOrder.LittleEndian, 0, long.MaxValue)]
        [TestCase(ByteOrder.BigEndian, 0, 0)]
        [TestCase(ByteOrder.BigEndian, 0, 1)]
        [TestCase(ByteOrder.BigEndian, 0, -1)]
        [TestCase(ByteOrder.BigEndian, 0, long.MinValue)]
        [TestCase(ByteOrder.BigEndian, 0, long.MaxValue)]
        public void Test_Long(ByteOrder byteOrder, int index, long input)
        {
            var buffer = new ExpandableArrayBuffer();
            buffer.PutLong(index, input, byteOrder);

            var result = (byteOrder == ExpandableArrayBuffer.NATIVE_BYTE_ORDER) ? buffer.GetLong(index) : buffer.GetLong(index, byteOrder);

            Assert.AreEqual(input, result);
        }

        [Category("ExpandableArrayBuffer")]
        [TestCase(ByteOrder.LittleEndian, new byte[] { 0, 0, 0, 0 }, 0, 0)]
        [TestCase(ByteOrder.LittleEndian, new byte[] { 1, 0, 0, 0 }, 0, 1)]
        [TestCase(ByteOrder.LittleEndian, new byte[] { 0, 0, 0, 0, 0 }, 1, 0)]
        [TestCase(ByteOrder.LittleEndian, new byte[] { 0, 1, 0, 0, 0 }, 1, 1)]
        [TestCase(ByteOrder.LittleEndian, new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 }, 4, 0)]
        [TestCase(ByteOrder.LittleEndian, new byte[] { 0, 0, 0, 0, 1, 0, 0, 0 }, 4, 1)]
        [TestCase(ByteOrder.LittleEndian, new byte[] { 255, 255, 255, 255 }, 0, -1)]
        [TestCase(ByteOrder.LittleEndian, new byte[] { 0, 255, 255, 255, 255 }, 1, -1)]
        [TestCase(ByteOrder.LittleEndian, new byte[] { 0, 0, 0, 0, 255, 255, 255, 255 }, 4, -1)]
        [TestCase(ByteOrder.LittleEndian, new byte[] { 0, 0, 0, 128 }, 0, int.MinValue)]
        [TestCase(ByteOrder.LittleEndian, new byte[] { 0, 0, 0, 0, 128 }, 1, int.MinValue)]
        [TestCase(ByteOrder.LittleEndian, new byte[] { 0, 0, 0, 0, 0, 0, 0, 128 }, 4, int.MinValue)]
        [TestCase(ByteOrder.LittleEndian, new byte[] { 255, 255, 255, 127 }, 0, int.MaxValue)]
        [TestCase(ByteOrder.LittleEndian, new byte[] { 0, 255, 255, 255, 127 }, 1, int.MaxValue)]
        [TestCase(ByteOrder.LittleEndian, new byte[] { 0, 0, 0, 0, 255, 255, 255, 127 }, 4, int.MaxValue)]
        [TestCase(ByteOrder.BigEndian, new byte[] { 0, 0, 0, 0 }, 0, 0)]
        [TestCase(ByteOrder.BigEndian, new byte[] { 0, 0, 0, 1 }, 0, 1)]
        [TestCase(ByteOrder.BigEndian, new byte[] { 0, 0, 0, 0, 0 }, 1, 0)]
        [TestCase(ByteOrder.BigEndian, new byte[] { 0, 0, 0, 0, 1 }, 1, 1)]
        [TestCase(ByteOrder.BigEndian, new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 }, 4, 0)]
        [TestCase(ByteOrder.BigEndian, new byte[] { 0, 0, 0, 0, 0, 0, 0, 1 }, 4, 1)]
        [TestCase(ByteOrder.BigEndian, new byte[] { 255, 255, 255, 255 }, 0, -1)]
        [TestCase(ByteOrder.BigEndian, new byte[] { 0, 255, 255, 255, 255 }, 1, -1)]
        [TestCase(ByteOrder.BigEndian, new byte[] { 0, 0, 0, 0, 255, 255, 255, 255 }, 4, -1)]
        [TestCase(ByteOrder.BigEndian, new byte[] { 128, 0, 0, 0 }, 0, int.MinValue)]
        [TestCase(ByteOrder.BigEndian, new byte[] { 0, 128, 0, 0, 0 }, 1, int.MinValue)]
        [TestCase(ByteOrder.BigEndian, new byte[] { 0, 0, 0, 0, 128, 0, 0, 0 }, 4, int.MinValue)]
        [TestCase(ByteOrder.BigEndian, new byte[] { 127, 255, 255, 255 }, 0, int.MaxValue)]
        [TestCase(ByteOrder.BigEndian, new byte[] { 0, 127, 255, 255, 255 }, 1, int.MaxValue)]
        [TestCase(ByteOrder.BigEndian, new byte[] { 0, 0, 0, 0, 127, 255, 255, 255 }, 4, int.MaxValue)]
        public void Test_Int(ByteOrder byteOrder, byte[] expectedResult, int index, int input)
        {
            var buffer = new ExpandableArrayBuffer();
            buffer.PutInt(index, input, byteOrder);

            var expectedResultArray = new ExpandableArrayBuffer();
            expectedResultArray.PutBytes(index, expectedResult);

            Assert.AreEqual(expectedResultArray, buffer);

            var result = (byteOrder == ExpandableArrayBuffer.NATIVE_BYTE_ORDER) ? buffer.GetInt(index) : buffer.GetInt(index, byteOrder);
            Assert.AreEqual(result, input);
        }

        [Category("ExpandableArrayBuffer")]
        [TestCase(ByteOrder.LittleEndian, new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 }, 0, 0)]
        [TestCase(ByteOrder.LittleEndian, new byte[] { 1, 0, 0, 0, 0, 0, 0, 0 }, 0, 1)]
        [TestCase(ByteOrder.LittleEndian, new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 1, 0)]
        [TestCase(ByteOrder.LittleEndian, new byte[] { 0, 1, 0, 0, 0, 0, 0, 0, 0 }, 1, 1)]
        [TestCase(ByteOrder.LittleEndian, new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 4, 0)]
        [TestCase(ByteOrder.LittleEndian, new byte[] { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0 }, 4, 1)]
        [TestCase(ByteOrder.LittleEndian, new byte[] { 255, 255, 255, 255, 255, 255, 255, 255 }, 0, -1)]
        [TestCase(ByteOrder.LittleEndian, new byte[] { 0, 255, 255, 255, 255, 255, 255, 255, 255 }, 1, -1)]
        [TestCase(ByteOrder.LittleEndian, new byte[] { 0, 0, 0, 0, 255, 255, 255, 255, 255, 255, 255, 255 }, 4, -1)]
        [TestCase(ByteOrder.LittleEndian, new byte[] { 0, 0, 0, 0, 0, 0, 0, 128 }, 0, long.MinValue)]
        [TestCase(ByteOrder.LittleEndian, new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 128 }, 1, long.MinValue)]
        [TestCase(ByteOrder.LittleEndian, new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 128 }, 4, long.MinValue)]
        [TestCase(ByteOrder.LittleEndian, new byte[] { 255, 255, 255, 255, 255, 255, 255, 127 }, 0, long.MaxValue)]
        [TestCase(ByteOrder.LittleEndian, new byte[] { 0, 255, 255, 255, 255, 255, 255, 255, 127 }, 1, long.MaxValue)]
        [TestCase(ByteOrder.LittleEndian, new byte[] { 0, 0, 0, 0, 255, 255, 255, 255, 255, 255, 255, 127 }, 4, long.MaxValue)]
        [TestCase(ByteOrder.BigEndian, new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 }, 0, 0)]
        [TestCase(ByteOrder.BigEndian, new byte[] { 0, 0, 0, 0, 0, 0, 0, 1 }, 0, 1)]
        [TestCase(ByteOrder.BigEndian, new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 1, 0)]
        [TestCase(ByteOrder.BigEndian, new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 1 }, 1, 1)]
        [TestCase(ByteOrder.BigEndian, new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 4, 0)]
        [TestCase(ByteOrder.BigEndian, new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 }, 4, 1)]
        [TestCase(ByteOrder.BigEndian, new byte[] { 255, 255, 255, 255, 255, 255, 255, 255 }, 0, -1)]
        [TestCase(ByteOrder.BigEndian, new byte[] { 0, 255, 255, 255, 255, 255, 255, 255, 255 }, 1, -1)]
        [TestCase(ByteOrder.BigEndian, new byte[] { 0, 0, 0, 0, 255, 255, 255, 255, 255, 255, 255, 255 }, 4, -1)]
        [TestCase(ByteOrder.BigEndian, new byte[] { 128, 0, 0, 0, 0, 0, 0, 0 }, 0, long.MinValue)]
        [TestCase(ByteOrder.BigEndian, new byte[] { 0, 128, 0, 0, 0, 0, 0, 0, 0 }, 1, long.MinValue)]
        [TestCase(ByteOrder.BigEndian, new byte[] { 0, 0, 0, 0, 128, 0, 0, 0, 0, 0, 0, 0 }, 4, long.MinValue)]
        [TestCase(ByteOrder.BigEndian, new byte[] { 127, 255, 255, 255, 255, 255, 255, 255 }, 0, long.MaxValue)]
        [TestCase(ByteOrder.BigEndian, new byte[] { 0, 127, 255, 255, 255, 255, 255, 255, 255 }, 1, long.MaxValue)]
        [TestCase(ByteOrder.BigEndian, new byte[] { 0, 0, 0, 0, 127, 255, 255, 255, 255, 255, 255, 255 }, 4, long.MaxValue)]
        public void Test_Long(ByteOrder byteOrder, byte[] expectedResult, int index, long input)
        {
            var buffer = new ExpandableArrayBuffer();
            buffer.PutLong(index, input, byteOrder);

            var expectedResultArray = new ExpandableArrayBuffer();
            expectedResultArray.PutBytes(index, expectedResult);

            Assert.AreEqual(expectedResultArray, buffer, $"PutLong of input ({input}) does not match PutBytes of provided Reference Array ({byteOrder} at index {index}).");

            var result = (byteOrder == ExpandableArrayBuffer.NATIVE_BYTE_ORDER) ? buffer.GetLong(index) : buffer.GetLong(index, byteOrder);
            Assert.AreEqual(result, input, $"PutLong ({input}) does not match GetLong ({result}) ({byteOrder} at index {index}).");
        }

        [Category("ExpandableArrayBuffer")]
        [TestCase(ByteOrder.LittleEndian, 0, 0)]
        [TestCase(ByteOrder.LittleEndian, 0, 1)]
        [TestCase(ByteOrder.LittleEndian, 0, -1)]
        [TestCase(ByteOrder.LittleEndian, 0, float.MinValue)]
        [TestCase(ByteOrder.LittleEndian, 0, float.MaxValue)]
        [TestCase(ByteOrder.LittleEndian, 0, float.Epsilon)]
        [TestCase(ByteOrder.BigEndian, 0, 0)]
        [TestCase(ByteOrder.BigEndian, 0, 1)]
        [TestCase(ByteOrder.BigEndian, 0, -1)]
        [TestCase(ByteOrder.BigEndian, 0, float.MinValue)]
        [TestCase(ByteOrder.BigEndian, 0, float.MaxValue)]
        [TestCase(ByteOrder.BigEndian, 0, float.Epsilon)]
        public void Test_Float(ByteOrder byteOrder, int index, float input)
        {
            var buffer = new ExpandableArrayBuffer();
            buffer.PutFloat(index, input, byteOrder);
            
            var result = (byteOrder == ExpandableArrayBuffer.NATIVE_BYTE_ORDER) ? buffer.GetFloat(index) : buffer.GetFloat(index, byteOrder);

            Assert.AreEqual(input, result);

        }

        [Category("ExpandableArrayBuffer")]
        [TestCase(ByteOrder.LittleEndian, 0, 0)]
        [TestCase(ByteOrder.LittleEndian, 0, 1)]
        [TestCase(ByteOrder.LittleEndian, 0, -1)]
        [TestCase(ByteOrder.LittleEndian, 0, double.MinValue)]
        [TestCase(ByteOrder.LittleEndian, 0, double.MaxValue)]
        [TestCase(ByteOrder.LittleEndian, 0, double.Epsilon)]
        [TestCase(ByteOrder.BigEndian, 0, 0)]
        [TestCase(ByteOrder.BigEndian, 0, 1)]
        [TestCase(ByteOrder.BigEndian, 0, -1)]
        [TestCase(ByteOrder.BigEndian, 0, double.MinValue)]
        [TestCase(ByteOrder.BigEndian, 0, double.MaxValue)]
        [TestCase(ByteOrder.BigEndian, 0, double.Epsilon)]
        public void Test_Double(ByteOrder byteOrder, int index, double input)
        {
            var buffer = new ExpandableArrayBuffer();
            buffer.PutDouble(index, input, byteOrder);

            var result = (byteOrder == ExpandableArrayBuffer.NATIVE_BYTE_ORDER) ? buffer.GetDouble(index) : buffer.GetDouble(index, byteOrder);

            Assert.AreEqual(input, result);
        }

        [Test]
        [Category("ExpandableArrayBuffer")]
        public void Test_Equals001()
        {
            var eab1 = new ExpandableArrayBuffer();
            eab1.PutBytes(0, new byte[] { 1, 0, 0, 0 });

            var eab2 = new ExpandableArrayBuffer();
            eab2.PutInt(0, 1, ByteOrder.LittleEndian);

            Assert.AreEqual(eab1, eab2);
            Assert.IsTrue(eab1.Equals(eab2));
            Assert.IsTrue(eab2.Equals(eab1));
        }

        [Test]
        [Category("ExpandableArrayBuffer")]
        public void Test_Equals002()
        {
            var eab1 = new ExpandableArrayBuffer();
            eab1.PutBytes(0, new byte[] { 0, 0, 0, 1 });

            var eab2 = new ExpandableArrayBuffer();
            eab2.PutInt(0, 1, ByteOrder.BigEndian);

            Assert.AreEqual(eab1, eab2);
            Assert.IsTrue(eab1.Equals(eab2));
            Assert.IsTrue(eab2.Equals(eab1));
        }

        [Test]
        [Category("ExpandableArrayBuffer")]
        public void Test_Equals003()
        {
            var eab1 = new ExpandableArrayBuffer();
            eab1.PutBytes(0, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });

            var eab2 = new ExpandableArrayBuffer();
            for (int ix = 0; ix < 10; ix++)
            {
                eab2.PutByte(ix, (byte)ix);
            }

            Assert.AreEqual(eab1, eab2);
            Assert.IsTrue(eab1.Equals(eab2));
            Assert.IsTrue(eab2.Equals(eab1));
        }

        [Category("ExpandableArrayBuffer")]
        [TestCase("ABC", "ABC")]
        [TestCase("A\0B\0C", "A?B?C")]
        [TestCase("\0\0\0AB\0C\0DE\0\0\0", "???AB?C?DE")]
        [TestCase("A_B-C", "A_B-C")]
        public void Test_ToString(string input, string output)
        {
            var testData = Encoding.UTF8.GetBytes(input);

            var size = 64;
            var buffer = new ExpandableArrayBuffer(size);
            buffer.PutBytes(0, testData);

            var bufferAsString = buffer.ToString();

            var expectedResult = "ExpandableArrayBuffer{byteArray.Length==" + size + ", byteArray==\"" + output + "\", byteArray=[";

            Assert.IsTrue(bufferAsString.StartsWith(expectedResult));
        }
    }
}