#nullable enable

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Erem.NamedIds
{
    public abstract class AbstractNamedIdsConfig : ScriptableObject
    {
        [SerializeField]
        private Entry[] _entries = null!;

        public IReadOnlyList<Entry> Entries => _entries;

        public virtual string GetEntryAsString(Entry entry)
        {
            return $"{entry.Id} : {entry.Name}";
        }

        [Serializable]
        public class Entry
        {
            public string Name = null!;
            public int Id = -1;
        }
    }
}
