using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Gomer.Core.Tests
{
    [TestFixture]
    public class DateRangeTest
    {
        [Test]
        public void TestParse()
        {
            var expectedStart = new DateTime(2015, 1, 1);
            var expectedEnd = new DateTime(2015, 1, 31);

            var actualBoth = DateRange.Parse("2015-01-01...2015-01-31");

            Assert.That(actualBoth.Start.HasValue, Is.True);
            Assert.That(actualBoth.Start.Value, Is.EqualTo(expectedStart));

            Assert.That(actualBoth.End.HasValue, Is.True);
            Assert.That(actualBoth.End.Value, Is.EqualTo(expectedEnd));

            var actualJustStart = DateRange.Parse("2015-01-01...");

            Assert.That(actualJustStart.Start.HasValue, Is.True);
            Assert.That(actualJustStart.Start.Value, Is.EqualTo(expectedStart));

            Assert.That(actualJustStart.End.HasValue, Is.False);

            var actualJustEnd = DateRange.Parse("...2015-01-31");

            Assert.That(actualJustEnd.Start.HasValue, Is.False);

            Assert.That(actualJustEnd.End.HasValue, Is.True);
            Assert.That(actualJustEnd.End.Value, Is.EqualTo(expectedEnd));
        }

        [Test]
        public void TestToString()
        {
            var start = new DateTime(2015, 1, 1);
            var end = new DateTime(2015, 1, 31);

            var both = new DateRange(start, end);

            Assert.That(both.ToString(), Is.EqualTo("2015-01-01...2015-01-31"));

            var justStart = new DateRange(start, null);
            Assert.That(justStart.ToString(), Is.EqualTo("2015-01-01..."));

            var justEnd = new DateRange(null, end);
            Assert.That(justEnd.ToString(), Is.EqualTo("...2015-01-31"));
        }
    }
}
