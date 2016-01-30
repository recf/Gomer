using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Gomer.DataAccess;
using Gomer.DataAccess.Implementation;
using Gomer.Models;
using NUnit.Framework;

namespace Gomer.Tests.RepositoryTests
{
    [TestFixture]
    public class PlatformRepositoryTests : RepositoryTestsBase<IPlatformRepository, PlatformModel>
    {
        protected override IDataContext GetContext(bool includeDataUnderTest)
        {
            var context = new DataContext();

            if (includeDataUnderTest)
            {
                context.Platforms.Add(new PlatformModel
                {
                    Id = 1,
                    Name = "Foo"
                });
                context.Platforms.Add(new PlatformModel
                {
                    Id = 2,
                    Name = "Bar"
                });
            }

            return context;
        }

        protected override IPlatformRepository GetRepository(IDataContext context)
        {
            return new PlatformRepository(context);
        }

        protected override ISet<PlatformModel> Set(IDataContext context)
        {
            return context.Platforms;
        }

        protected override PlatformModel GetNewModel(IDataContext context)
        {
            return new PlatformModel { Name = "Test" };
        }

        protected override Expression<Func<PlatformModel, bool>> GetKnownItemPredicate()
        {
            return x => x.Name == "Foo";
        }
    }
}
