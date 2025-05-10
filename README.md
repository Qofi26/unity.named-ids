# Named ID for int/string fields

## Install via UPM (using Git URL)

````
https://github.com/Qofi26/unity.named-ids.git?path=/NamedIdsProject/Assets/NamedIds
````

For use specific version (e.g. `v/1.0.0`)

````
https://github.com/Qofi26/unity.named-ids.git?path=/NamedIdsProject/Assets/NamedIds#v/1.0.0
````

OR download the [release package](https://github.com/Qofi26/unity.named-ids/releases) and add it to your project 

# What is

Using the int or string properties, you only see the values and can write the value manually
```` c#
public int IntValue;
public string StringValue;
````

![default.png](imgs%2Fdefault.png)

#### With this package you can use popup window as when using enum, but the values will be defined in ScriptableObject
This means you can enter new values without editing the code, or even import them from json/google sheets

![pupup.png](imgs%2Fpupup.png)

# Basic Usage (3 steps)

## Step 1: Create new `Attribute` and `ScriptableObject`

Create attribute class and inherit from `NamedIdsAttribute`. 

Create ScriptableObject class and inherit from `AbstractNamedIdsConfig`.

This empty classes and do not require implementation.

### For quick creation you can use menu `Assets/Create/NamedIds/New Named Ids script`

```` c#
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
````

## Step 2: Create you ScriptableObject asset

Create you ScriptableObject asset (in example this is `ExampleNamedIds`) => `Assets/Create/NamedIds/ExampleNamedIds`

Add entry values. Name and ID must be unique for correct work

Use value from ScriptableObject
![values.png](imgs%2Fvalues.png)

You can use `Validate` for check duplicates and `Fix duplicates` for fix non unique ids

## Step 3: Use you attributes for field

In you class use you attribute (in example this is `ExampleNamedIdsAttribute` / `ExampleNamedIds`) for you `int` or `string` fields

```` c#
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
````
