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
    public class StatusRepositoryTests : RepositoryTestsBase<IStatusRepository, StatusModel>
    {
        protected override IDataContext GetContext(bool includeDataUnderTest)
        {
            var context = new DataContext();

            if (includeDataUnderTest)
            {
                context.Statuses.Add(new StatusModel
                {
                    Id = 1,
                    Order = 2,
                    Code = StatusCodes.NotStarted
                });
                context.Statuses.Add(new StatusModel
                {
                    Id = 2,
                    Order = 1,
                    Code = StatusCodes.InProgress
                });
            }

            return context;
        }

        protected override IStatusRepository GetRepository(IDataContext context)
        {
            return new StatusRepository(context);
        }

        protected override ISet<StatusModel> Set(IDataContext context)
        {
            return context.Statuses;
        }

        protected override StatusModel GetNewModel(IDataContext context)
        {
            return new StatusModel();
        }

        protected override Expression<Func<StatusModel, bool>> GetKnownItemPredicate()
        {
            return x => x.Code == StatusCodes.NotStarted;
        }

        protected override void ModifyModel(StatusModel model, IDataContext context)
        {
            model.Order += 1;
        }

        protected override void AssertEqual(StatusModel actual, StatusModel expected)
        {
            Assert.That(actual.Id, Is.EqualTo(expected.Id));
            Assert.That(actual.Name, Is.EqualTo(expected.Name));
            Assert.That(actual.Order, Is.EqualTo(expected.Order));
        }
    }
}
