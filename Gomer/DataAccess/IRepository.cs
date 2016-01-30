using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Gomer.Models;

namespace Gomer.DataAccess
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity Get(int id);

        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        void Add(TEntity model);
        bool Remove(TEntity model);
    }
}