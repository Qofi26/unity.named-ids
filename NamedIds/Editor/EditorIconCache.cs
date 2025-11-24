#nullable enable

using UnityEditor;
using UnityEngine;

namespace NamedIds.Editor
{
    internal class EditorIconCache
    {
        public readonly GUIContent? Icon;
        public readonly GUIStyle? Style;

        public EditorIconCache(string iconName, float? size = null)
        {
            size ??= EditorGUIUtility.singleLineHeight;
            (Icon, Style) = GetIcon(iconName, size.Value, 0);
        }

        private static (GUIContent, GUIStyle) GetIcon(string name, float iconSize, int offset = 1)
        {
            var icon = EditorGUIUtility.IconContent(name);
            var iconButtonStyle = new GUIStyle(GUI.skin.button)
            {
                imagePosition = ImagePosition.ImageOnly,
                fixedWidth = iconSize,
                fixedHeight = iconSize,
                padding = new RectOffset(offset, offset, offset, offset),
                margin = new RectOffset(0, 0, 0, 0)
            };

            return (icon, iconButtonStyle);
        }
    }
}
