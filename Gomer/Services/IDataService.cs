using System.Collections.Generic;
using System.Collections.ObjectModel;
using Gomer.Dto;

namespace Gomer.Services
{
    public interface IDataService
    {
        ObservableCollection<string> RecentFiles { get; }
        PileDto OpenFile(string fileName);

        PileDto GetNew();
        bool TryOpen(out PileDto pile, out string fileName);

        bool TrySave(PileDto pile, out string fileName);
        bool TrySaveAs(PileDto pile, out string fileName);
    }
}