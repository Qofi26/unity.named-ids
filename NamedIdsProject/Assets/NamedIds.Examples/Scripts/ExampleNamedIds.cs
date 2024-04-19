using UnityEngine;

namespace Erem.NamedIds.Examples.Scripts
{
    public class ExampleNamedIdsAttribute : NamedIdsAttribute
    {
        public ExampleNamedIdsAttribute() : base(typeof(ExampleNamedIds)) { }
    }

    [CreateAssetMenu(menuName = "NamedIds/" + nameof(ExampleNamedIds), fileName = nameof(ExampleNamedIds))]
    public class ExampleNamedIds : AbstractNamedIdsConfig { }
}
