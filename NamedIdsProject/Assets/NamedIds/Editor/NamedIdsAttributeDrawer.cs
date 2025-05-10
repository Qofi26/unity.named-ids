#nullable enable

using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Erem.NamedIds.Editor
{
    [CustomPropertyDrawer(typeof(NamedIdsAttribute), true)]
    internal class NamedIdsAttributeDrawer : PropertyDrawer
    {
        private AbstractNamedIdsConfig.Entry[]? _entries;
        private string[]? _names;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var namedIdsAttribute = (NamedIdsAttribute) attribute;
            LoadEntries(namedIdsAttribute.ContainedType);

            if (_entries == null)
            {
                EditorGUI.LabelField(position, $"Please create asset for '{namedIdsAttribute.ContainedType.Name}'");
                EditorGUI.EndProperty();
                return;
            }

            position = EditorGUI.PrefixLabel(position, label);

            var names = _names ?? Array.Empty<string>();

            var selectedIndex = GetSelectedIndex(property, _entries);
            var buttonText = selectedIndex >= 0 && names.Length > selectedIndex
                ? names[selectedIndex]
                : "Select...";

            if (GUI.Button(position, buttonText))
            {
                SearchablePopup.Show(position, names, Select, selectedIndex);
            }

            EditorGUI.EndProperty();

            return;

            void Select(int newIndex)
            {
                if (selectedIndex == newIndex)
                {
                    return;
                }

                var entry = _entries[newIndex];
                SetPropertyValue(property, entry);
                property.serializedObject.ApplyModifiedProperties();
            }
        }

        private static int GetSelectedIndex(SerializedProperty property, AbstractNamedIdsConfig.Entry[] entries)
        {
            int selectedIndex;
            switch (property.propertyType)
            {
                case SerializedPropertyType.Integer:
                    var currentId = property.intValue;
                    selectedIndex = Array.FindIndex(entries, entry => entry.Id == currentId);
                    break;

                case SerializedPropertyType.String:
                    var currentName = property.stringValue;
                    selectedIndex = Array.FindIndex(entries, entry => entry.Name == currentName);
                    break;
                default:
                    selectedIndex = 0;
                    break;
            }

            return selectedIndex;
        }

        private static void SetPropertyValue(SerializedProperty property, AbstractNamedIdsConfig.Entry entry)
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.Integer:
                    property.intValue = entry.Id;
                    break;
                case SerializedPropertyType.String:
                    property.stringValue = entry.Name;
                    break;
            }
        }

        private void LoadEntries(Type forType)
        {
            if (_entries != null)
            {
                return;
            }

            var container = NamedIdsUtils.LoadContainer(forType);
            if (container == null)
            {
                return;
            }

            _entries = container.Entries.ToArray();
            if (_entries == null)
            {
                return;
            }

            _names = _entries
                .OrderBy(entry => entry.Id)
                .Select(entry => container.GetEntryAsString(entry))
                .ToArray();
        }
    }
}
