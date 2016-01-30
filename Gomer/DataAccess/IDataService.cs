using System.Collections.ObjectModel;
using Gomer.Models;

namespace Gomer.DataAccess
{
    public interface IDataService
    {
        ObservableCollection<string> RecentFiles { get; }
        PileModel OpenFile(string fileName);

        PileModel GetNew();
        bool TryOpen(out PileModel pile, out string fileName);

        bool TrySave(PileModel pile, out string fileName);
        bool TrySaveAs(PileModel pile, out string fileName);
    }
}