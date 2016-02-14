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
    public class ListModelTests : ModelTestsBase<ListModel>
    {
        protected override ListModel TestSetFrom_GetExpectedModel()
        {
            return new ListModel
            {
                Name = "Foo",
                IncludeInStats = true
            };
        }
    }
}
