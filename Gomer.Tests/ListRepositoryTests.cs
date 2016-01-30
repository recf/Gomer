using System.Configuration;
using System.Linq;
using Gomer.DataAccess;
using Gomer.DataAccess.Implementation;
using Gomer.Models;
using NUnit.Framework;

namespace Gomer.Tests
{
    [TestFixture]
    public class ListRepositoryTests
    {
        [Test]
        public void TestAddEmpty()
        {
            var context = new DataContext();

            var repo = new ListRepository(context);

            var list = new ListModel();

            Assert.That(list.Id, Is.EqualTo(0));

            repo.Add(list);
            
            Assert.That(list.Id, Is.EqualTo(1));

            Assert.That(context.Lists.Contains(list), Is.True);
        }

        [Test]
        public void TestAddNonEmpty()
        {
            var context = new DataContext();
            context.Lists.Add(new ListModel
            {
                Id = 1,
                Name = "Foo"
            });
            context.Lists.Add(new ListModel
            {
                Id = 2,
                Name = "Bar"
            });

            var repo = new ListRepository(context);

            var list = new ListModel { Name = "Test" };

            Assert.That(list.Id, Is.EqualTo(0));

            repo.Add(list);

            Assert.That(list.Id, Is.EqualTo(3));

            Assert.That(context.Lists.Contains(list), Is.True);
        }

        [Test]
        public void TestGetWhenExists()
        {
            var context = new DataContext();
            context.Lists.Add(new ListModel
            {
                Id = 1,
                Name = "Foo"
            });
            
            var repo = new ListRepository(context);
            var actual = repo.Get(1);

            Assert.That(actual, Is.EqualTo(context.Lists.First()));
        }

        [Test]
        public void TestGetWhenDoesNotExist()
        {
            var context = new DataContext();

            var repo = new ListRepository(context);
            var actual = repo.Get(1);

            Assert.That(actual, Is.Null);
        }

        [Test]
        public void GetAllEmpty()
        {
            var context = new DataContext();

            var repo = new ListRepository(context);

            var actual = repo.GetAll();

            Assert.That(actual, Is.EquivalentTo(context.Lists));
        }
        
        [Test]
        public void GetAllNonEmpty()
        {
            var context = new DataContext();
            context.Lists.Add(new ListModel
            {
                Id = 1,
                Name = "Foo"
            });
            context.Lists.Add(new ListModel
            {
                Id = 2,
                Name = "Bar"
            });

            var repo = new ListRepository(context);

            var actual = repo.GetAll();

            Assert.That(actual, Is.EquivalentTo(context.Lists));
        }

        [Test]
        public void TestFindWhenExists()
        {
            var context = new DataContext();
            var list1 = new ListModel
            {
                Id = 1,
                Name = "Foo"
            };
            context.Lists.Add(list1);
            context.Lists.Add(new ListModel
            {
                Id = 2,
                Name = "Bar"
            });

            var repo = new ListRepository(context);

            var actual = repo.Find(x => x.Name == "Foo").ToList();

            Assert.That(actual.Count, Is.EqualTo(1));
            Assert.That(actual.First(), Is.EqualTo(list1));
        }

        [Test]
        public void TestFindWhenDoesNotExist()
        {
            var context = new DataContext();

            var repo = new ListRepository(context);

            var actual = repo.Find(x => x.Name == "Foo").ToList();

            Assert.That(actual.Count, Is.EqualTo(0));
        }

        [Test]
        public void TestRemoveWhenExists()
        {
            var context = new DataContext();
            var list = new ListModel
            {
                Id = 1,
                Name = "Foo"
            };
            context.Lists.Add(list);

            var repo = new ListRepository(context);

            var actual = repo.Remove(list);
            Assert.That(actual, Is.True);
            Assert.That(context.Lists, Does.Not.Contains(list));
        }
        
        [Test]
        public void TestRemoveWhenDoesNotExist()
        {
            var context = new DataContext();

            var list = new ListModel
            {
                Id = 1,
                Name = "Foo"
            };

            var repo = new ListRepository(context);

            var actual = repo.Remove(list);
            Assert.That(actual, Is.False);
        }
    }
}