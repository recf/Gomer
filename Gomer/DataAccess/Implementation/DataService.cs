using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Windows.Forms;
using AutoMapper;
using Gomer.DataAccess.Dto;
using Gomer.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Gomer.DataAccess.Implementation
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

        public PileModel GetNew()
        {
            var pileDto = new PileDto()
            {
                Lists = new List<ListDto>
                {
                    new ListDto { Name = "Pile", IncludeInStats = true },
                    new ListDto { Name = "Subscription" },
                    new ListDto { Name = "Wishlist" },
                    new ListDto { Name = "Ignored" }
                },

                Platforms = new List<PlatformDto>()
                {
                    new PlatformDto { Name = "Playstation 4" },
                    new PlatformDto { Name = "Xbox One" },
                    new PlatformDto { Name = "Wii U" },
                    new PlatformDto { Name = "PC" },
                },

                Statuses = new List<StatusDto>()
                {
                    new StatusDto { Name = "Not Started", Order = 1 },
                    new StatusDto { Name = "In Progress", Order = 2 },
                    new StatusDto { Name = "Finished", Order = 3, AlwaysIncludeInStats = true },
                    new StatusDto { Name = "Retired", Order = 4 },
                },
            };

            var pile = Mapper.Map<PileModel>(pileDto);

            return pile;
        }

        public PileModel OpenFile(string fileName)
        {
            _lastFileName = fileName;
            using (var sr = new StreamReader(_lastFileName))
            using (var reader = new JsonTextReader(sr))
            {
                var pileDto = _serializer.Deserialize<PileDto>(reader);

                EnsureRecentFile(fileName);
                Properties.Settings.Default.RecentFiles.Insert(0, fileName);
                Properties.Settings.Default.Save();

                var pile = Mapper.Map<PileModel>(pileDto);

                return pile;
            }
        }

        public bool TryOpen(out PileModel pile, out string fileName)
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

            fileName = dialog.FileName;
            pile = OpenFile(dialog.FileName);
            return true;
        }

        public bool TrySave(PileModel pile, out string fileName)
        {
            if (_lastFileName == null)
            {
                return TrySaveAs(pile, out fileName);
            }

            fileName = _lastFileName;

            SaveFile(pile, fileName);

            return true;
        }

        public bool TrySaveAs(PileModel pile, out string fileName)
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

            fileName = dialog.FileName;

            SaveFile(pile, fileName);

            return true;
        }

        private void EnsureRecentFile(string fileName)
        {
            if (RecentFiles.Contains(fileName))
            {
                RecentFiles.Remove(fileName);
                Properties.Settings.Default.RecentFiles.Remove(fileName);
            }

            RecentFiles.Insert(0, fileName);
        }

        private void SaveFile(PileModel pile, string fileName)
        {
            var pileDto = Mapper.Map<PileDto>(pile);
            _lastFileName = fileName;

            using (var sw = new StreamWriter(fileName))
            using (var writer = new JsonTextWriter(sw))
            {
                EnsureRecentFile(fileName);
                _serializer.Serialize(writer, pileDto);
            }
        }
    }
}
