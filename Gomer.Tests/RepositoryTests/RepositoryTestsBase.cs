using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Gomer.DataAccess;
using Gomer.Models;
using NUnit.Framework;

namespace Gomer.Tests.RepositoryTests
{
    public abstract class RepositoryTestsBase<TRepository, TModel> 
        where TRepository : IRepository<TModel>
        where TModel : ModelBase<TModel>
    {
        protected abstract IDataContext GetContext(bool includeDataUnderTest);

        protected abstract TRepository GetRepository(IDataContext context);

        protected abstract ISet<TModel> Set(IDataContext context);

        protected abstract TModel GetNewModel(IDataContext context);

        protected abstract Expression<Func<TModel, bool>> GetKnownItemPredicate();

        protected abstract void ModifyModel(TModel model, IDataContext context);

        protected abstract void AssertEqual(TModel actual, TModel expected);

        [Test]
        public void TestAddEmpty()
        {
            var context = GetContext(false);
            var repo = GetRepository(context);

            var model = GetNewModel(context);

            Assert.That(model.Id, Is.EqualTo(0));

            repo.Add(model);

            Assert.That(model.Id, Is.EqualTo(1));

            Assert.That(Set(context), Does.Contain(model));
        }

        [Test]
        public void TestAddNonEmpty()
        {
            var context = GetContext(true);

            var repo = GetRepository(context);

            var model = GetNewModel(context);

            Assert.That(model.Id, Is.EqualTo(0));

            repo.Add(model);

            Assert.That(model.Id, Is.EqualTo(3));

            Assert.That(Set(context), Does.Contain(model));
        }

        [Test]
        public void TestGetWhenExists()
        {
            var context = GetContext(true);

            var repo = GetRepository(context);

            var expected = Set(context).First();

            var actual = repo.Get(expected.Id);
            
            AssertEqual(actual, expected);
        }

        [Test]
        public void TestGetWhenDoesNotExist()
        {
            var context = GetContext(false);

            var repo = GetRepository(context);
            var actual = repo.Get(1);

            Assert.That(actual, Is.Null);
        }
        
        [Test]
        public void GetAllEmpty()
        {
            var context = GetContext(false);

            var repo = GetRepository(context);

            var expected = Set(context);
            var actual = repo.GetAll();

            Assert.That(actual, Is.EquivalentTo(expected));
        }

        [Test]
        public void GetAllNonEmpty()
        {
            var context = GetContext(true);

            var repo = GetRepository(context);

            var expectedSet = Set(context);
            var actualSet = repo.GetAll().ToList();

            Assert.That(actualSet.Count(), Is.EqualTo(expectedSet.Count));
            foreach (var actual in actualSet)
            {
                var expected = expectedSet.FirstOrDefault(x => x.Id == actual.Id);
                AssertEqual(actual, expected);
            }
        }

        [Test]
        public void TestFindWhenExists()
        {
            var context = GetContext(true);
            var expected = Set(context).First();

            var repo = GetRepository(context);

            var actual = repo.Find(GetKnownItemPredicate()).ToList();

            Assert.That(actual.Count, Is.EqualTo(1));

            AssertEqual(actual.First(), expected);
        }

        [Test]
        public void TestFindWhenDoesNotExist()
        {
            var context = GetContext(false);

            var repo = GetRepository(context);

            var actual = repo.Find(GetKnownItemPredicate()).ToList();

            Assert.That(actual.Count, Is.EqualTo(0));
        }

        [Test]
        public void TestRemoveWhenExists()
        {
            var context = GetContext(true);
            var repo = GetRepository(context);

            var toRemove = Set(context).First();
            var actual = repo.Remove(toRemove);

            Assert.That(actual, Is.True);
            Assert.That(context.Lists, Does.Not.Contains(toRemove));
        }

        [Test]
        public void TestRemoveWhenDoesNotExist()
        {
            var context = GetContext(false);
            var repo = GetRepository(context);

            var toRemove = GetNewModel(context);
            toRemove.Id = 1;

            var actual = repo.Remove(toRemove);
            Assert.That(actual, Is.False);
        }

        [Test]
        public void TestUpdateWhenExists()
        {
            var context = GetContext(true);
            var repo = GetRepository(context);

            var toUpdate = repo.Get(1);
            ModifyModel(toUpdate, context);

            var updated = repo.Update(toUpdate);

            Assert.That(updated, Is.True);
            AssertEqual(repo.Get(1), toUpdate);
        }

        [Test]
        public void TestUpdateWhenDoesNotExist()
        {
            var context = GetContext(false);
            var repo = GetRepository(context);

            var toUpdate = GetNewModel(context);

            var updated = repo.Update(toUpdate);

            Assert.That(updated, Is.False);
        }
    }
}
