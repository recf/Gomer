using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Gomer.Models;

namespace Gomer.DataAccess.Implementation
{
    public abstract class RepositoryBase<TModel> : IRepository<TModel> where TModel : ModelBase<TModel>, new()
    {
        protected abstract ISet<TModel> Set { get; }

        protected abstract IEnumerable<TModel> DefaultSort(IEnumerable<TModel> models);

        protected IDataContext Context { get; private set; }

        protected abstract TModel PopulateSecondaryData(TModel model);

        protected Sequence Sequence { get; private set; }

        protected virtual void OnBeforeAdd(TModel entity)
        {
            
        }

        protected virtual void OnBeforeUpdate(TModel entity, TModel previousState)
        {
        }

        protected RepositoryBase(IDataContext context)
        {
            Context = context;
            Sequence = new Sequence();
        }

        protected TModel CopyAndPopulateSecondaryData(TModel model)
        {
            var copy = new TModel();
            copy.SetFrom(model);

            return PopulateSecondaryData(copy);
        }

        public TModel Get(int id)
        {
            var model = Set.FirstOrDefault(x => x.Id == id);
            if (model == null)
            {
                return null;
            }

            return CopyAndPopulateSecondaryData(model);
        }

        public IEnumerable<TModel> GetAll()
        {
            return DefaultSort(Set.Select(CopyAndPopulateSecondaryData));
        }

        public IEnumerable<TModel> Find(Expression<Func<TModel, bool>> predicate)
        {
            return DefaultSort(Set.Select(CopyAndPopulateSecondaryData).Where(predicate.Compile()));
        }

        public void Add(TModel model)
        {
            var presentIds = Set.Select(x => x.Id).ToArray();

            do
            {
                model.Id = Sequence.NextValue();
            } while (presentIds.Contains(model.Id));

            OnBeforeAdd(model);
            Set.Add(model);
        }

        public bool Remove(TModel entity)
        {
            return Set.Remove(entity);
        }

        public bool Update(TModel entity)
        {
            var oldState = Set.FirstOrDefault(x => x.Id == entity.Id);
            if (oldState == null)
            {
                return false;
            }

            OnBeforeUpdate(entity, oldState);
            Set.Remove(oldState);
            Set.Add(entity);

            return true;
        }
    }
}