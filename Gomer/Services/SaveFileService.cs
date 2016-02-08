using System.Windows.Forms;

namespace Gomer.Services
{
    public class SaveFileService : ISaveFileService
    {
        public string GetFileName(string defaultExtension, string filter)
        {
            var dialog = new SaveFileDialog()
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