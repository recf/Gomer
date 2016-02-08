using System;
using Gomer.Models;
using NUnit.Framework;

namespace Gomer.Tests
{
    [TestFixture]
    public class GameModelTests
    {
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
