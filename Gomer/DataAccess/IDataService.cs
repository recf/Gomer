using System.Collections.ObjectModel;
using Gomer.Models;

namespace Gomer.DataAccess
{
    public interface IDataService
    {
        IListRepository Lists { get; }
        IPlatformRepository Platforms { get; }
        IGameRepository Games { get; }

        ObservableCollection<string> RecentFiles { get; }
        void OpenFile(string fileName);

        void PopulateDefaultData();
        bool TryOpen(out string fileName);

        bool TrySave(out string fileName);
        bool TrySaveAs(out string fileName);
    }
}