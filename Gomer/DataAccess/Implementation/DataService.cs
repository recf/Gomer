using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
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

        private readonly IDataContext _context;
        public IListRepository Lists { get; private set; }
        public IPlatformRepository Platforms { get; private set; }
        public IStatusRepository Statuses { get; private set; }
        public IGameRepository Games { get; private set; }

        public DataService()
        {
            // Repositories
            _context = new DataContext();
            Lists = new ListRepository(_context);
            Platforms = new PlatformRepository(_context);
            Statuses = new StatusRepository(_context);
            Games = new GameRepository(_context);

            // Serializer
            _serializer = new JsonSerializer();
            _serializer.Formatting = Formatting.Indented;
            _serializer.Converters.Add(new IsoDateTimeConverter()
            {
                DateTimeFormat = "yyyy-MM-dd"
            });

            // Recent Files
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

        public void PopulateDefaultData()
        {
            Lists.Add(new ListModel { Name = "Pile", IncludeInStats = true });
            Lists.Add(new ListModel { Name = "Subscription" });
            Lists.Add(new ListModel { Name = "Wishlist" });
            Lists.Add(new ListModel { Name = "Ignored" });

            Platforms.Add(new PlatformModel { Name = "Playstation 4" });
            Platforms.Add(new PlatformModel { Name = "Xbox One" });
            Platforms.Add(new PlatformModel { Name = "Wii U" });
            Platforms.Add(new PlatformModel { Name = "PC" });

            Statuses.Add(new StatusModel { Name = "Not Started", Order = 1 });
            Statuses.Add(new StatusModel { Name = "In Progress", Order = 2 });
            Statuses.Add(new StatusModel { Name = "Finished", Order = 3, AlwaysIncludeInStats = true });
            Statuses.Add(new StatusModel { Name = "Retired", Order = 4 });
        }

        public void OpenFile(string fileName)
        {
            _lastFileName = fileName;
            using (var sr = new StreamReader(_lastFileName))
            using (var reader = new JsonTextReader(sr))
            {
                var pileDto = _serializer.Deserialize<PileDto>(reader);

                EnsureRecentFile(fileName);
                Properties.Settings.Default.RecentFiles.Insert(0, fileName);
                Properties.Settings.Default.Save();

                _context.Lists.Clear();
                _context.Platforms.Clear();
                _context.Statuses.Clear();
                _context.Games.Clear();

                foreach (var dto in pileDto.Lists)
                {
                    var model = Mapper.Map<ListModel>(dto);
                    Lists.Add(model);
                }

                foreach (var dto in pileDto.Platforms)
                {
                    var model = Mapper.Map<PlatformModel>(dto);
                    Platforms.Add(model);
                }

                foreach (var dto in pileDto.Statuses)
                {
                    var model = Mapper.Map<StatusModel>(dto);
                    Statuses.Add(model);
                }

                foreach (var dto in pileDto.Games)
                {
                    var model = Mapper.Map<GameModel>(dto);

                    model.List = Lists.Find(l => l.Name == dto.ListName).First();
                    model.Status = Statuses.Find(s => s.Name == dto.StatusName).First();

                    foreach (var platformName in dto.PlatformNames)
                    {
                        var platform = Platforms.Find(p => p.Name == platformName).First();
                        model.Platforms.Add(platform);
                    }

                    Games.Add(model);
                }
            }
        }

        public bool TryOpen(out string fileName)
        {
            var dialog = new OpenFileDialog
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
            OpenFile(dialog.FileName);
            return true;
        }

        public bool TrySave(out string fileName)
        {
            if (_lastFileName == null)
            {
                return TrySaveAs(out fileName);
            }

            fileName = _lastFileName;

            SaveFile(fileName);

            return true;
        }

        public bool TrySaveAs(out string fileName)
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

            SaveFile(fileName);

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

        private void SaveFile(string fileName)
        {
            var pileDto = new PileDto
            {
                Lists = Mapper.Map<ICollection<ListDto>>(Lists.GetAll()),
                Platforms = Mapper.Map<ICollection<PlatformDto>>(Platforms.GetAll()),
                Statuses = Mapper.Map<ICollection<StatusDto>>(Statuses.GetAll()),

                Games = Mapper.Map<ICollection<GameDto>>(Games.GetAll())
            };

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
