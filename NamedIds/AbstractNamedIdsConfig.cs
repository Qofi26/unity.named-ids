#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NamedIds
{
    public abstract class AbstractNamedIdsConfig : ScriptableObject
    {
        [Tooltip("{0} - ID, {1} - Name, {2} - Description")]
        [SerializeField]
        private string _entryFormat = "{1}";

        [SerializeField] private string _groupSeparator = "/";

        [SerializeField] private Value[] _values = Array.Empty<Value>();

        public string GroupSeparator => _groupSeparator;

        public IReadOnlyList<Value> Values => _values;

        public virtual void Initialize(IEnumerable<Value> entries)
        {
            _values = entries.ToArray();
        }

        public virtual Value CreateEntry(int id, string entryName, string description)
        {
            return new Value { Id = id, Name = entryName, Description = description };
        }

        public virtual IEnumerable<string> GetNames()
        {
            return Values.Select(x => x.Name);
        }

        public virtual IEnumerable<int> GetIds()
        {
            return Values.Select(x => x.Id);
        }

        public virtual string GetEntryAsString(Value value)
        {
            return string.Format(_entryFormat, value.Id, value.Name, value.Description);
        }

        [Serializable]
        public class Value
        {
            public string Name = string.Empty;
            public int Id = -1;
            public string Description = string.Empty;

            public Value() { }

            public Value(string name, int id, string description)
            {
                Name = name;
                Id = id;
                Description = description;
            }
        }
    }
}
