using Gomer.Dto;

namespace Gomer.Services
{
    public interface IDataService
    {
        PileDto GetNew();
        bool TryOpen(out PileDto pile);
        bool TrySave(PileDto pile);
        bool TrySaveAs(PileDto pile);
    }
}