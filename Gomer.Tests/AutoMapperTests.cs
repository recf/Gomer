using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Gomer.Tests
{
    [TestFixture]
    public class AutoMapperTests
    {
        [Test]
        public void TestAutoMapperConfiguration()
        {
            AutoMapperConfiguration.Configure();

            AutoMapper.Mapper.AssertConfigurationIsValid();
        }
    }
}
