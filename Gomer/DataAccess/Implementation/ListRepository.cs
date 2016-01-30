using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Gomer.Models;

namespace Gomer.DataAccess.Implementation
{
    public class ListRepository : RepositoryBase<ListModel>, IListRepository
    {
        public ListRepository(IDataContext context) : base(context) { }

        protected override ISet<ListModel> Set
        {
            get { return Context.Lists; }
        }

        protected override IEnumerable<ListModel> DefaultSort(IEnumerable<ListModel> models)
        {
            return models.OrderBy(x => x.Name);
        }
    }
}