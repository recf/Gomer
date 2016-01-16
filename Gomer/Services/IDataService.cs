using Gomer.Dto;

namespace Gomer.Services
{
    public interface IDataService
    {
        PileDto GetNew();
        bool TryOpen(out PileDto pile, out string fileName);
        bool TrySave(PileDto pile, out string fileName);
        bool TrySaveAs(PileDto pile, out string fileName);
    }
}