using UnityEngine;

namespace Erem.NamedIds.Examples.Scripts
{
    public class ExampleUsage : MonoBehaviour
    {
        [ExampleNamedIds]
        public int IntValue;

        [ExampleNamedIds]
        public string StringValue;
    }
}
