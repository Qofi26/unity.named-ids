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

        private const string kDefaultButtonIcon = "*";

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

            var labelRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, position.height);
            EditorGUI.LabelField(labelRect, label);

            var buttonSize = EditorGUIUtility.singleLineHeight;
            const float spacing = 2f;
            var fieldWidth = position.width - EditorGUIUtility.labelWidth - buttonSize - spacing;

            var fieldRect = new Rect(position.x + EditorGUIUtility.labelWidth, position.y, fieldWidth, position.height);
            var modeButtonRect = new Rect(fieldRect.xMax + spacing, position.y, buttonSize, buttonSize);

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

            if (GUI.Button(modeButtonRect, GetButtonIcon()))
            {
                _viewState = (AbstractNamedIdsConfig.ViewState) (((int) _viewState + 1) % 3);
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

        private string GetButtonIcon()
        {
            return kDefaultButtonIcon;
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
