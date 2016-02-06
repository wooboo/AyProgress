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
        private IList<State> _states;

        public ProgressState(string fileName)
        {
            _fileName = fileName;
            Load();
        }

        public event EventHandler<ProgressEventArgs> ProgressChanged;

        public void OnProgressChanged(ProgressContext context, ProgressEventArgs e)
        {
            ProgressChanged?.Invoke(context, e);
        }

        private void Load()
        {
            if (File.Exists(_fileName))
            {
                _states = JsonConvert.DeserializeObject<List<State>>(File.ReadAllText(_fileName));
            }
            else
            {
                _states = new List<State>();
            }
        }

        public void Save()
        {
            var json = JsonConvert.SerializeObject(_states, Formatting.Indented);
            File.WriteAllText(_fileName, json);
        }


        public IList<State> GetStates(string key)
        {
            return _states.Where(o => o.Key == key).OrderBy(o => o.Value).ToList();
        }

        public void Add(State state)
        {
            _states.Add(state);
        }
    }
}