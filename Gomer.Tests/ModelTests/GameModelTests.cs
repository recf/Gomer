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
    public class GameModelTests : ModelTestsBase<GameModel>
    {
        protected override GameModel TestSetFrom_GetExpectedModel()
        {
            var list = new ListModel { Name = "Foo", IncludeInStats = true };
            var platform = new PlatformModel { Name = "Bar" };

            var game = new GameModel
            {
                Name = "Game",
                List = list,
                IsDigital = true,
                Note="Note",
                EstimatedHours = 10,
                PlayedHours = 5,
                StartedOn = DateTime.Today,
                FinishedOn = DateTime.Today,
                RetiredOn = DateTime.Today
            };
            game.Platforms.Add(platform);

            return game;
        }

        [Test]
        public void TestSetDatesCalcStatus()
        {
            var model = new GameModel()
            {
                AddedOn = DateTime.Today.AddDays(-14)
            };

            Assert.That(model.StatusCode, Is.EqualTo(StatusCodes.NotStarted));

            model.StartedOn = DateTime.Today.AddDays(-12);

            Assert.That(model.StatusCode, Is.EqualTo(StatusCodes.InProgress));

            model.FinishedOn = DateTime.Today;

            Assert.That(model.StatusCode, Is.EqualTo(StatusCodes.Finished));

            model.RetiredOn = DateTime.Today;
            Assert.That(model.StatusCode, Is.EqualTo(StatusCodes.Finished));

            model.FinishedOn = null;
            Assert.That(model.StatusCode, Is.EqualTo(StatusCodes.Retired));
        }
    }
}
