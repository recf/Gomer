using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gomer.Dto;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Gomer.Services
{
    public class DataService : IDataService
    {
        private string _defaultExt = ".pile";
        private string _filter = "Gomer Pile File (*.pile)|*.pile";
        private string _lastFileName;

        private readonly JsonSerializer _serializer;

        public ObservableCollection<string> RecentFiles { get; private set; }

        public DataService()
        {
            _serializer = new JsonSerializer();
            _serializer.Formatting = Formatting.Indented;
            _serializer.Converters.Add(new IsoDateTimeConverter()
            {
                DateTimeFormat = "yyyy-MM-dd"
            });

            RecentFiles = new ObservableCollection<string>();
            if (Properties.Settings.Default.RecentFiles == null)
            {
                Properties.Settings.Default.RecentFiles = new StringCollection();
            }

            foreach (var recentFile in Properties.Settings.Default.RecentFiles)
            {
                RecentFiles.Add(recentFile);
            }
        }

        public PileDto GetNew()
        {
            return new PileDto()
            {
                Lists = new List<ListDto>
                {
                    new ListDto { Name = "Pile" },
                    new ListDto { Name = "Wishlist" },
                    new ListDto { Name = "Hidden" }
                }
            };
        }

        public PileDto OpenFile(string fileName)
        {
            _lastFileName = fileName;
            using (var sr = new StreamReader(_lastFileName))
            using (var reader = new JsonTextReader(sr))
            {
                var pile = _serializer.Deserialize<PileDto>(reader);

                if (!RecentFiles.Contains(fileName))
                {
                    RecentFiles.Add(fileName);
                    Properties.Settings.Default.RecentFiles.Add(fileName);
                    Properties.Settings.Default.Save();
                }

                return pile;
            }
        }

        public bool TryOpen(out PileDto pile, out string fileName)
        {
            var dialog = new OpenFileDialog
            {
                DefaultExt = _defaultExt,
                Filter = _filter
            };

            if (dialog.ShowDialog() != DialogResult.OK)
            {
                fileName = null;
                pile = null;
                return false;
            }

            _lastFileName = dialog.FileName;
            fileName = _lastFileName;

            pile = OpenFile(dialog.FileName);
            return true;
        }

        public bool TrySave(PileDto pile, out string fileName)
        {
            if (_lastFileName == null)
            {
                return TrySaveAs(pile, out fileName);
            }

            fileName = _lastFileName;

            using (var sw = new StreamWriter(_lastFileName))
            using (var writer = new JsonTextWriter(sw))
            {
                _serializer.Serialize(writer, pile);
            }

            return true;
        }

        public bool TrySaveAs(PileDto pile, out string fileName)
        {
            var dialog = new SaveFileDialog()
            {
                DefaultExt = _defaultExt,
                Filter = _filter
            };

            if (dialog.ShowDialog() != DialogResult.OK)
            {
                fileName = null;
                return false;
            }

            _lastFileName = dialog.FileName;
            fileName = _lastFileName;

            using (var sw = new StreamWriter(_lastFileName))
            using (var writer = new JsonTextWriter(sw))
            {
                _serializer.Serialize(writer, pile);
            }

            return true;
        }
    }
}
