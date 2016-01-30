using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Gomer.Models;

namespace Gomer.DataAccess.Implementation
{
    public abstract class RepositoryBase<TModel> : IRepository<TModel> where TModel : ModelBase<TModel>
    {
        protected abstract ISet<TModel> Set { get; }

        protected abstract IEnumerable<TModel> DefaultSort(IEnumerable<TModel> models);

        protected IDataContext Context { get; private set; }

        protected Sequence Sequence { get; private set; }

        protected RepositoryBase(IDataContext context)
        {
            Context = context;
            Sequence = new Sequence();
        }

        public TModel Get(int id)
        {
            return Set.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<TModel> GetAll()
        {
            return DefaultSort(Set);
        }

        public IEnumerable<TModel> Find(Expression<Func<TModel, bool>> predicate)
        {
            return DefaultSort(Set.Where(predicate.Compile()));
        }

        public void Add(TModel model)
        {
            var presentIds = Set.Select(x => x.Id).ToArray();

            do
            {
                model.Id = Sequence.NextValue();
            } while (presentIds.Contains(model.Id));

            Set.Add(model);
        }

        public bool Remove(TModel model)
        {
            return Set.Remove(model);
        }
    }
}