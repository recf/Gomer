using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gomer.Models;
using NUnit.Framework;

namespace Gomer.Tests.ModelTests
{
    [TestFixture]
    public class PlatformModelTests : ModelTestsBase<PlatformModel>
    {
        protected override PlatformModel TestSetFrom_GetExpectedModel()
        {
            return new PlatformModel
            {
                Name = "Foo"
            };
        }
    }
}
