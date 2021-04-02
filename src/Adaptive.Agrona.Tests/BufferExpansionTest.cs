using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Adaptive.Agrona.Tests
{
    [TestFixture]
    public class BufferExpansionTest
    {
        [Test]
        [Category("ExpandableArrayBuffer")]
        public void ShouldExpand()
        {
            IMutableDirectBuffer buffer = new ExpandableArrayBuffer();

            int capacity = buffer.Capacity;

            int index = capacity + 50;
            int value = 777;
            buffer.PutInt(index, value);

            Assert.That(buffer.Capacity, Is.GreaterThan(capacity));
            Assert.AreEqual(buffer.GetInt(index), value);
        }
        [Test]
        [Category("ExpandableArrayBuffer")]
        public void ShouldExpandArrayBufferFromZeroCapacity()
        {
            IMutableDirectBuffer buffer = new ExpandableArrayBuffer(0);
            buffer.PutByte(0, (byte)4);

            Assert.That(buffer.Capacity, Is.GreaterThan(0));
        }

        [Test]
        [Category("ExpandableArrayBuffer")]
        public void ShouldExpandArrayBufferFromOneCapacity()
        {
            IMutableDirectBuffer buffer = new ExpandableArrayBuffer(1);
            buffer.PutByte(0, (byte)4);
            buffer.PutByte(1, (byte)2);
        }
    }
}