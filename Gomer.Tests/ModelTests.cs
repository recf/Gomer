using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Gomer.Models;
using NUnit.Framework;

namespace Gomer.Tests
{
    [TestFixture]
    public class ModelTests
    {
        public class TestModel : ModelBase<TestModel>
        {
            public override void SetFrom(TestModel other)
            {
                throw new NotImplementedException();
            }
        }

        [TestCase(0, true)]
        [TestCase(1000, false)]
        public void TestIsNew(int id, bool expected)
        {
            var model = new TestModel();
            model.Id = id;

            Assert.That(model.IsNew, Is.EqualTo(expected));
        }
    }
}
