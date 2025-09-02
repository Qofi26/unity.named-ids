#nullable enable

using System.Linq;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Erem.NamedIds.Editor
{
    internal class NamedIdsDropdown : AdvancedDropdown
    {
        private readonly string[] _options;
        private readonly System.Action<int> _onSelect;
        private readonly string? _groupSeparator;
        private readonly float _maxHeight;

        public NamedIdsDropdown(
            Rect rect,
            string[] options,
            System.Action<int> onSelect,
            string? groupSeparator = null,
            float maxHeight = 400)
            : base(new AdvancedDropdownState())
        {
            _options = options;
            _onSelect = onSelect;
            _groupSeparator = groupSeparator;
            _maxHeight = maxHeight;
            minimumSize = new Vector2(rect.width, 200);
        }

        public void ShowAdvanced(Rect rect)
        {
            this.Show(rect, _maxHeight);
        }

        protected override AdvancedDropdownItem BuildRoot()
        {
            var root = new AdvancedDropdownItem("Search");

            for (var i = 0; i < _options.Length; i++)
            {
                var path = _options[i].Split(new[] { _groupSeparator }, System.StringSplitOptions.RemoveEmptyEntries);
                var parent = root;

                for (var j = 0; j < path.Length; j++)
                {
                    var part = path[j];
                    if (j == path.Length - 1)
                    {
                        parent.AddChild(new AdvancedDropdownItem(part) { id = i });
                    }
                    else
                    {
                        var existing = parent.children?.FirstOrDefault(child => child.name == part);
                        if (existing == null)
                        {
                            existing = new AdvancedDropdownItem(part);
                            parent.AddChild(existing);
                        }

                        parent = existing;
                    }
                }
            }

            return root;
        }

        protected override void ItemSelected(AdvancedDropdownItem item)
        {
            _onSelect?.Invoke(item.id);
        }
    }
}
