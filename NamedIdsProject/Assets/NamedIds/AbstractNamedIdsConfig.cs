#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Erem.NamedIds
{
    public abstract class AbstractNamedIdsConfig : ScriptableObject
    {
        [SerializeField]
        private Entry[] _entries = null!;

        public IReadOnlyList<Entry> Entries => _entries;

        public void Initialize(IEnumerable<Entry> entries)
        {
            _entries = entries.ToArray();
        }

        public Entry CreateEntry(int id, string entryName)
        {
            return new Entry { Id = id, Name = entryName };
        }

        public IEnumerable<string> GetNames()
        {
            return Entries.Select(x => x.Name);
        }

        public IEnumerable<int> GetIds()
        {
            return Entries.Select(x => x.Id);
        }

        public virtual string GetEntryAsString(Entry entry)
        {
            return $"{entry.Id} : {entry.Name}";
        }

        [Serializable]
        public class Entry
        {
            public string Name = null!;
            public int Id = -1;

            public Entry() { }

            public Entry(string name, int id)
            {
                Name = name;
                Id = id;
            }
        }
    }
}
