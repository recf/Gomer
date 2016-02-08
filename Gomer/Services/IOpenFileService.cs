namespace Gomer.Services
{
    public interface IOpenFileService
    {
        string GetFileName(string defaultExtension, string filter);
    }
}