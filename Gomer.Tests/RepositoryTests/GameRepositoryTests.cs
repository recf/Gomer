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
    public class GameRepositoryTests : RepositoryTestsBase<IGameRepository, GameModel>
    {
        protected override IDataContext GetContext(bool includeDataUnderTest)
        {
            var context = new DataContext();

            context.Lists.Add(new ListModel
            {
                Id = 1,
                Name = "List 1"
            });
            context.Lists.Add(new ListModel
            {
                Id = 2,
                Name = "List 2"
            });

            context.Platforms.Add(new PlatformModel
            {
                Id = 1,
                Name = "Platform 1"
            });
            context.Platforms.Add(new PlatformModel
            {
                Id = 2,
                Name = "Platform 2"
            });

            context.Statuses.Add(new StatusModel
            {
                Id = 1,
                Order = 1,
                Code = StatusCodes.Finished
            });
            context.Statuses.Add(new StatusModel
            {
                Id = 2,
                Order = 2,
                Code = StatusCodes.NotStarted
            });

            if (includeDataUnderTest)
            {
                var game1 = new GameModel
                {
                    Id = 1,
                    Name = "Test",
                    List = context.Lists.First()
                };
                game1.Platforms.Add(context.Platforms.First());

                var game2 = new GameModel
                {
                    Id = 2,
                    Name = "Test 2",
                    List = context.Lists.First()
                };
                game2.Platforms.Add(context.Platforms.First(x => x.Name == "Platform 2"));

                context.Games.Add(game1);
                context.Games.Add(game2);
            }

            return context;
        }

        protected override IGameRepository GetRepository(IDataContext context)
        {
            return new GameRepository(context);
        }

        protected override ISet<GameModel> Set(IDataContext context)
        {
            return context.Games;
        }

        protected override GameModel GetNewModel(IDataContext context)
        {
            return new GameModel();
        }

        protected override Expression<Func<GameModel, bool>> GetKnownItemPredicate()
        {
            return x => x.Name == "Test";
        }

        protected override void ModifyModel(GameModel model, IDataContext context)
        {
            model.Name = (model.Name ?? string.Empty) + " Modified";
        }

        protected override void AssertEqual(GameModel actual, GameModel expected)
        {
            Assert.That(actual.Id, Is.EqualTo(expected.Id));
            Assert.That(actual.Name, Is.EqualTo(expected.Name));
        }
    }
}
