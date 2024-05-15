#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Erem.NamedIds.Editor
{
    public static class NamedIdsUtils
    {
        public static AbstractNamedIdsConfig.Entry[]? LoadEntries(Type forType)
        {
#if UNITY_EDITOR
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
#else
            return null;
#endif
        }

        public static AbstractNamedIdsConfig? LoadContainer(Type forType)
        {
#if UNITY_EDITOR

            var guids = AssetDatabase.FindAssets($"t:{forType.Name}");
            var assets = guids
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(entry => AssetDatabase.LoadAssetAtPath(entry, forType))
                .Where(c => c != null)
                .ToArray();

            var container = assets.FirstOrDefault() as AbstractNamedIdsConfig;
            return container;
#else
            return null;
#endif
        }

        public static void UpdateNamedIds<T>(IEnumerable<string> ids, bool setupIds = true, string defaultId = null!)
            where T : AbstractNamedIdsConfig
        {
#if UNITY_EDITOR
            var entries = new List<AbstractNamedIdsConfig.Entry>();

            entries.AddRange(ids.Select(id => new AbstractNamedIdsConfig.Entry { Name = id }));

            if (!string.IsNullOrEmpty(defaultId) && entries.All(x => x.Name != defaultId))
            {
                entries.Insert(0, new AbstractNamedIdsConfig.Entry { Name = defaultId });
            }

            UpdateNamedIds<T>(entries, setupIds);
#endif
        }

        public static void UpdateNamedIds<T>(IEnumerable<AbstractNamedIdsConfig.Entry> entries, bool setupIds = true)
            where T : AbstractNamedIdsConfig
        {
#if UNITY_EDITOR
            var container = LoadContainer(typeof(T));

            if (container == null)
            {
                return;
            }

            var list = entries.ToList();

            if (setupIds)
            {
                var id = 0;
                foreach (var entry in list)
                {
                    entry.Id = id++;
                }
            }

            container.Initialize(list);

            EditorUtility.SetDirty(container);
            AssetDatabase.SaveAssetIfDirty(container);
            AssetDatabase.Refresh();
#endif
        }
    }
}
