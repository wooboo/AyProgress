using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace AyProgress
{
    public class ProgressState
    {
        private readonly string _fileName;
        private IList<StateItem> _items;

        public ProgressState(string fileName)
        {
            _fileName = fileName;
            Load();
        }

        private void Load()
        {
            if (File.Exists(_fileName))
            {
                _items = JsonConvert.DeserializeObject<List<StateItem>>(File.ReadAllText(_fileName));
            }
            else
            {
                _items = new List<StateItem>();
            }
        }

        public void Save()
        {
            var json = JsonConvert.SerializeObject(_items, Formatting.Indented);
            File.WriteAllText(_fileName, json);
        }


        public IList<StateItem> GetStates(string key)
        {
            return _items.Where(o => o.Key == key).OrderBy(o => o.Value).ToList();
        }

        public void Add(StateItem stateItem)
        {
            _items.Add(stateItem);
        }
    }
}