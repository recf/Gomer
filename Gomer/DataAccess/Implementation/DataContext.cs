using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gomer.Models;

namespace Gomer.DataAccess.Implementation
{
    public class DataContext : IDataContext
    {
        public ISet<ListModel> Lists { get; private set; }
        public ISet<PlatformModel> Platforms { get; private set; }
        public ISet<StatusModel> Statuses { get; private set; }

        public ISet<GameModel> Games { get; private set; }

        public DataContext()
        {
            Platforms = new HashSet<PlatformModel>(new ModelEqualityComparer<PlatformModel>());
            Lists = new HashSet<ListModel>(new ModelEqualityComparer<ListModel>());
            Statuses = new HashSet<StatusModel>(new ModelEqualityComparer<StatusModel>());

            Games = new HashSet<GameModel>(new ModelEqualityComparer<GameModel>());
        }

        public class ModelEqualityComparer<TModel> : IEqualityComparer<TModel> where TModel : ModelBase<TModel>
        {
            public bool Equals(TModel x, TModel y)
            {
                return x.Id == y.Id;
            }

            public int GetHashCode(TModel obj)
            {
                return obj.Id.GetHashCode();
            }
        }
    }
}
