using System.Windows.Forms;

namespace Gomer.Services
{
    public class OpenFileService : IOpenFileService
    {
        public string GetFileName(string defaultExtension, string filter)
        {
            var dialog = new OpenFileDialog
            {
                DefaultExt = defaultExtension,
                Filter = filter
            };

            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return null;
            }

            return dialog.FileName;
        }
    }
}