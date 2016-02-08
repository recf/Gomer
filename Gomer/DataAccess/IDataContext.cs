using System.Collections.Generic;
using Gomer.Models;

namespace Gomer.DataAccess
{
    public interface IDataContext
    {
        ISet<ListModel> Lists { get; }
        ISet<PlatformModel> Platforms { get; }

        ISet<GameModel> Games { get; }
    }
}