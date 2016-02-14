using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using AutoMapper.Internal;
using Gomer.Models;
using NUnit.Framework;

namespace Gomer.Tests.ModelTests
{
    [TestFixture]
    public abstract class ModelTestsBase<TModel> where TModel : ModelBase<TModel>, new()
    {

        [TestCase(0, true)]
        [TestCase(1000, false)]
        public void TestIsNew(int id, bool expected)
        {
            var model = new TModel { Id = id };

            Assert.That(model.IsNew, Is.EqualTo(expected));
        }

        protected abstract TModel TestSetFrom_GetExpectedModel();

        [Test]
        public void TestSetFrom()
        {
            var type = typeof (TModel);
            var properties = type.GetProperties();

            var expectedModel = TestSetFrom_GetExpectedModel();

            var actualModel = new TModel();
            actualModel.SetFrom(expectedModel);

            Console.WriteLine("Getter / Setter properties");
            Console.WriteLine("--------------------------");

            foreach (var property in properties.Where(p => p.CanWrite))
            {
                Console.WriteLine(property.Name);

                var expected = property.GetValue(expectedModel);
                var actual = property.GetValue(actualModel);

                Assert.That(actual, Is.EqualTo(expected), "SetFrom failed for property '{0}'.", property.Name);
            }

            Console.WriteLine();
            Console.WriteLine("Collection properties");
            Console.WriteLine("---------------------");

            foreach (var property in properties.Where(p => p.PropertyType.IsCollectionType()))
            {
                Console.WriteLine(property.Name);

                var expected = property.GetValue(expectedModel) as IEnumerable;
                var actual = property.GetValue(actualModel) as IEnumerable;

                Assert.That(actual, Is.EquivalentTo(expected), "SetFrom failed for property '{0}'.", property.Name);
            }
        }
    }
}
