#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Erem.NamedIds.Editor
{
    [CustomEditor(typeof(AbstractNamedIdsConfig), true)]
    public class NamedIdsConfigEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            GUILayout.BeginVertical();

            if (GUILayout.Button("Validate"))
            {
                Validate(false);
            }

            if (GUILayout.Button("Fix duplicates"))
            {
                Validate(true);
            }

            if (GUILayout.Button("Save"))
            {
                Save();
            }

            GUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }

        private void FixDuplicates()
        {
            var entries = ((AbstractNamedIdsConfig) target).Entries;
            var maxId = entries.Max(x => x.Id);

            foreach (var entry in entries)
            {
                if (entry.Id == -1)
                {
                    entry.Id = ++maxId;
                }
            }
        }

        private void Validate(bool fixDuplicates)
        {
            Undo.RecordObject(target, $"Validate {target.name}");
            var uniqueIds = new List<int>();
            var uniqueNames = new List<string>();
            var entries = ((AbstractNamedIdsConfig) target).Entries;
            var maxId = entries.Max(x => x.Id);
            var success = true;

            var validateStringBuilder = new StringBuilder();
            var fixStringBuilder = new StringBuilder();

            var nextId = ++maxId;

            for (var index = 0; index < entries.Count; index++)
            {
                var entry = entries[index];
                if (uniqueIds.Contains(entry.Id))
                {
                    if (fixDuplicates)
                    {
                        validateStringBuilder.Append(
                            $"Fix duplicate (index: {index}) [{entry.Id} : {entry.Name}] ----> [{nextId} : {entry.Name}]{Environment.NewLine}");

                        entry.Id = nextId++;
                    }
                    else
                    {
                        success = false;
                        fixStringBuilder.Append($"Find duplicate ID: {entry.Id} | {entry.Name}{Environment.NewLine}");
                    }
                }
                else
                {
                    uniqueIds.Add(entry.Id);
                }

                if (uniqueNames.Contains(entry.Name))
                {
                    success = false;

                    fixStringBuilder.Append($"Find duplicate Name: {entry.Id} | {entry.Name}{Environment.NewLine}");
                }
                else
                {
                    uniqueNames.Add(entry.Name);
                }
            }

            Debug.Log($"Validate status: {success}");

            if (validateStringBuilder.Length != 0)
            {
                Debug.Log(validateStringBuilder.ToString());
            }

            if (fixStringBuilder.Length != 0)
            {
                Debug.LogError(fixStringBuilder.ToString());
            }
        }

        private void Save()
        {
            EditorUtility.SetDirty(target);
            AssetDatabase.SaveAssets();
            Debug.Log($"{target.name} saved");
        }
    }
}
