#nullable enable

using System;
using UnityEngine;

namespace Erem.NamedIds
{
    public class NamedIdsAttribute : PropertyAttribute
    {
        public Type ContainedType { get; private set; }

        public NamedIdsAttribute(Type type)
        {
            if (!type.IsSubclassOf(typeof(AbstractNamedIdsConfig)))
            {
                throw new ArgumentException($"Type {type.Name} should be inherited from NamedIds");
            }

            ContainedType = type;
        }

        public NamedIdsAttribute(string typeName)
        {
            var type = Type.GetType(typeName);
            if (type == null)
            {
                throw new ArgumentException($"Can't find {typeName} type");
            }

            if (!type.IsSubclassOf(typeof(AbstractNamedIdsConfig)))
            {
                throw new ArgumentException($"Type {typeName} should be inherited from NamedIds");
            }

            ContainedType = type;
        }
    }
}
