using System;
using System.Collections.Generic;
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

        private JsonSerializer _serializer;

        public DataService()
        {
            _serializer = new JsonSerializer();
            _serializer.Formatting = Formatting.Indented;
            _serializer.Converters.Add(new IsoDateTimeConverter()
            {
                DateTimeFormat = "yyyy-MM-dd"
            });
        }

        public PileDto GetNew()
        {
            return new PileDto()
            {
                Lists = new List<ListDto>
                {
                    new ListDto { Name = "Pile" },
                    new ListDto { Name = "Wishlist" },
                    new ListDto { Name = "Ignored" }
                }
            };
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
            
            using (var sr = new StreamReader(_lastFileName))
            using (var reader = new JsonTextReader(sr))
            {
                pile = _serializer.Deserialize<PileDto>(reader);
                return true;
            }
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
