namespace Gomer.Services
{
    public interface ISaveFileService
    {
        string GetFileName(string defaultExtension, string filter); 
    }
}