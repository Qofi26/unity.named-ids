using UnityEngine;

namespace NamedIds.Examples
{
    public class ExampleNamedIdsAttribute : NamedIdsAttribute
    {
        public ExampleNamedIdsAttribute() : base(typeof(ExampleNamedIds)) { }
    }

    [CreateAssetMenu(menuName = "NamedIds/" + nameof(ExampleNamedIds), fileName = nameof(ExampleNamedIds))]
    public class ExampleNamedIds : AbstractNamedIdsConfig { }
}
