using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Gomer.Models;

namespace Gomer.DataAccess
{
    public interface IListRepository : IRepository<ListModel>
    {
        IEnumerable<ListModel> FindByName(string name, params string[] names);
    }
}