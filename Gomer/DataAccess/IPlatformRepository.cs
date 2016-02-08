using System.Collections.Generic;
using Gomer.Models;

namespace Gomer.DataAccess
{
    public interface IPlatformRepository : IRepository<PlatformModel>
    {
        IEnumerable<PlatformModel> FindByName(string name, params string[] names);
    }
}