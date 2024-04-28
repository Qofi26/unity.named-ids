#nullable enable

using System;
using System.Linq;
using UnityEditor;

namespace Erem.NamedIds.Editor
{
    public static class NamedIdsUtils
    {
        public static AbstractNamedIdsConfig.Entry[]? LoadEntries(Type forType)
        {
            var guids = AssetDatabase.FindAssets($"t:{forType.Name}");
            var assets = guids
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(entry => AssetDatabase.LoadAssetAtPath(entry, forType))
                .Where(c => c != null)
                .ToArray();

            var container = assets.FirstOrDefault() as AbstractNamedIdsConfig;
            if (container == null)
            {
                return null;
            }

            return container.Entries.ToArray();
        }

        public static AbstractNamedIdsConfig? LoadContainer(Type forType)
        {
            var guids = AssetDatabase.FindAssets($"t:{forType.Name}");
            var assets = guids
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(entry => AssetDatabase.LoadAssetAtPath(entry, forType))
                .Where(c => c != null)
                .ToArray();

            var container = assets.FirstOrDefault() as AbstractNamedIdsConfig;
            return container;
        }
    }
}
