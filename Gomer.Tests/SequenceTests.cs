using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gomer.DataAccess;
using Gomer.DataAccess.Implementation;
using NUnit.Framework;

namespace Gomer.Tests
{
    [TestFixture]
    public class SequenceTests
    {
        [Test]
        public void TestCurrentWithoutNext()
        {
            var sequence = new Sequence();

            Assert.Throws<InvalidOperationException>(() =>
            {
                var current = sequence.CurrentValue();
            });
        }

        [TestCase(1, 1, 1, 2, 3, 4 )]
        [TestCase(2, 2, 2, 4, 6, 8 )]
        [TestCase(1000, 1, 1000, 1001, 1002, 1003, 1004 )]
        public void TestNextValue(int startValue, int step, params int[] expected)
        {
            var sequence = new Sequence(startValue, step);

            var actual = expected.Select(_ => sequence.NextValue());

            Assert.That(actual, Is.EquivalentTo(expected));
        }
    }
}
