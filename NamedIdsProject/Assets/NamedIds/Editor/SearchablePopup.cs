#nullable enable

using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Erem.NamedIds.Editor
{
    internal class SearchablePopup : EditorWindow
    {
        private string _searchText = string.Empty;
        private string[] _options = Array.Empty<string>();
        private Action<int>? _onSelect;
        private Vector2 _scrollPosition;
        private int _selectedIndex;

        private bool _focusSearch;
        private const string kSearchFieldName = "SearchablePopupSearchField";

        public static void Show(Rect rect, string[] options, Action<int> onSelect, int selectedIndex = -1)
        {
            var window = CreateInstance<SearchablePopup>();
            window._options = options;
            window._onSelect = onSelect;
            window._selectedIndex = selectedIndex;

            var windowSize = new Vector2(rect.width, 300);
            var screenPosition = GUIUtility.GUIToScreenRect(rect);

            if (selectedIndex >= 0)
            {
                var itemHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                window._scrollPosition = new Vector2(0, selectedIndex * itemHeight);
            }

            window._focusSearch = true;
            window.ShowAsDropDown(screenPosition, windowSize);
        }

        private void OnGUI()
        {
            GUI.SetNextControlName(kSearchFieldName);
            _searchText = EditorGUILayout.TextField(_searchText);

            if (_focusSearch)
            {
                EditorGUI.FocusTextInControl(kSearchFieldName);
                _focusSearch = false;
            }

            var filteredOptions = string.IsNullOrEmpty(_searchText)
                ? _options
                : _options.Where(x => x.IndexOf(_searchText, StringComparison.OrdinalIgnoreCase) >= 0);

            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            foreach (var option in filteredOptions)
            {
                var isSelected = Array.IndexOf(_options, option) == _selectedIndex;

                var style = new GUIStyle(GUI.skin.button);
                if (isSelected)
                {
                    style.fontStyle = FontStyle.Bold;
                    style.normal.textColor = EditorGUIUtility.isProSkin
                        ? Color.cyan
                        : Color.blue;
                }

                if (GUILayout.Button(option, style))
                {
                    _onSelect?.Invoke(Array.IndexOf(_options, option));
                    Close();
                }
            }

            EditorGUILayout.EndScrollView();
        }
    }
}
