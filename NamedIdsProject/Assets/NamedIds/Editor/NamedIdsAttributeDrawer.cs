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
        private AbstractNamedIdsConfig? _config;
        private AbstractNamedIdsConfig.Entry[]? _entries;
        private string[]? _names;

        private AbstractNamedIdsConfig.ViewState _viewState;

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

            var names = _names ?? Array.Empty<string>();
            var selectedIndex = GetSelectedIndex(property, _entries);

            position = EditorGUI.PrefixLabel(position, label);

            var buttonSize = EditorGUIUtility.singleLineHeight;
            var modeButtonRect = new Rect(position.x, position.y, buttonSize, buttonSize);
            var fieldRect = new Rect(position.x + buttonSize + 2,
                position.y,
                position.width - buttonSize - 2,
                position.height);

            const string icon = "*";
            if (GUI.Button(modeButtonRect, icon))
            {
                _viewState = (AbstractNamedIdsConfig.ViewState) (((int) _viewState + 1) % 3);
            }

            switch (_viewState)
            {
                case AbstractNamedIdsConfig.ViewState.Default:
                    EditorGUI.PropertyField(fieldRect, property, GUIContent.none, true);
                    break;
                case AbstractNamedIdsConfig.ViewState.Button:
                    var buttonText = selectedIndex >= 0 && names.Length > selectedIndex
                        ? names[selectedIndex]
                        : "Select...";
                    if (GUI.Button(fieldRect, buttonText))
                    {
                        SearchablePopup.Show(fieldRect, names, Select, selectedIndex);
                    }

                    break;
                case AbstractNamedIdsConfig.ViewState.Popup:
                    var newIndex = EditorGUI.Popup(fieldRect, selectedIndex, names);
                    if (newIndex != selectedIndex && newIndex >= 0 && newIndex < _entries.Length)
                    {
                        SetPropertyValue(property, _entries[newIndex]);
                        property.serializedObject.ApplyModifiedProperties();
                    }

                    break;
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

            _config = NamedIdsUtils.LoadContainer(forType);
            if (!_config)
            {
                return;
            }

            _viewState = _config!.GetIdViewState();
            _entries = _config.Entries.ToArray();
            if (_entries == null)
            {
                return;
            }

            _names = _entries
                .OrderBy(entry => entry.Id)
                .Select(entry => _config.GetEntryAsString(entry))
                .ToArray();
        }
    }
}
