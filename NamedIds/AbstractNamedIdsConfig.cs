#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NamedIds
{
    public abstract class AbstractNamedIdsConfig : ScriptableObject
    {
        [Tooltip("{0} - ID, {1} - Name")]
        [SerializeField]
        private string _entryFormat = "{1}";

        [SerializeField]
        private string _groupSeparator = "/";

        [SerializeField]
        private Entry[] _entries = Array.Empty<Entry>();

        public string GroupSeparator => _groupSeparator;

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
            return string.Format(_entryFormat, entry.Id, entry.Name);
        }

        [Serializable]
        public class Entry
        {
            public string Name = string.Empty;
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
