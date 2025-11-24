using UnityEngine;

namespace NamedIds.Examples
{
    public class ExampleUsage : MonoBehaviour
    {
        [ExampleNamedIds]
        public int IntValue;

        [ExampleNamedIds]
        public string StringValue;
    }
}
