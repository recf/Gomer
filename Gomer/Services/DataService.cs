﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gomer.Dto;
using Newtonsoft.Json;

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
        }

        public PileDto GetNew()
        {
            return new PileDto();
        }

        public bool TryOpen(out PileDto pile)
        {
            var dialog = new OpenFileDialog
            {
                DefaultExt = _defaultExt,
                Filter = _filter
            };

            if (dialog.ShowDialog() != DialogResult.OK)
            {
                pile = null;
                return false;
            }

            _lastFileName = dialog.FileName;
            
            using (var sr = new StreamReader(_lastFileName))
            using (var reader = new JsonTextReader(sr))
            {
                pile = _serializer.Deserialize<PileDto>(reader);
                return true;
            }
        }

        public bool TrySave(PileDto pile)
        {
            if (_lastFileName == null)
            {
                return TrySaveAs(pile);
            }

            using (var sw = new StreamWriter(_lastFileName))
            using (var writer = new JsonTextWriter(sw))
            {
                _serializer.Serialize(writer, pile);
            }

            return true;
        }

        public bool TrySaveAs(PileDto pile)
        {
            var dialog = new SaveFileDialog()
            {
                DefaultExt = _defaultExt,
                Filter = _filter
            };

            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return false;
            }

            _lastFileName = dialog.FileName;
            
            using (var sw = new StreamWriter(_lastFileName))
            using (var writer = new JsonTextWriter(sw))
            {
                _serializer.Serialize(writer, pile);
            }

            return true;
        }
    }
}
