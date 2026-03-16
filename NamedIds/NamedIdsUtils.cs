#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace NamedIds
{
    public static class NamedIdsUtils
    {
        public static IEnumerable<AbstractNamedIdsConfig.Value> LoadEntries(Type forType)
        {
#if UNITY_EDITOR
            var container = LoadContainer(forType);

            return container != null
                ? container.Values
                : Enumerable.Empty<AbstractNamedIdsConfig.Value>();
#else
            return Enumerable.Empty<AbstractNamedIdsConfig.Value>();
#endif
        }

        public static AbstractNamedIdsConfig? LoadContainer(Type forType)
        {
#if UNITY_EDITOR

            var guids = AssetDatabase.FindAssets($"t:{forType.Name}");
            var assets = guids
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(entry => AssetDatabase.LoadAssetAtPath(entry, forType))
                .Where(c => c != null);

            var container = assets.FirstOrDefault() as AbstractNamedIdsConfig;
            return container;
#else
            return null;
#endif
        }

        public static void UpdateNamedIds<T>(
            IEnumerable<string> ids,
            bool setupIds = true,
            string defaultId = null!,
            bool trim = true)
            where T : AbstractNamedIdsConfig
        {
#if UNITY_EDITOR
            var entries = new List<AbstractNamedIdsConfig.Value>();

            entries.AddRange(ids.Select(id => new AbstractNamedIdsConfig.Value { Name = id }));

            if (!string.IsNullOrEmpty(defaultId) && entries.All(x => x.Name != defaultId))
            {
                entries.Insert(0, new AbstractNamedIdsConfig.Value { Name = defaultId });
            }

            UpdateNamedIds<T>(entries, setupIds);
#endif
        }

        public static void UpdateNamedIds<T>(
            IEnumerable<AbstractNamedIdsConfig.Value> entries,
            bool setupIds = true,
            bool trim = true)
            where T : AbstractNamedIdsConfig
        {
#if UNITY_EDITOR
            var container = LoadContainer(typeof(T));

            if (container == null)
            {
                return;
            }

            var list = entries.ToList();

            if (trim)
            {
                foreach (var entry in list)
                {
                    entry.Name = entry.Name.Trim();
                }
            }

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
