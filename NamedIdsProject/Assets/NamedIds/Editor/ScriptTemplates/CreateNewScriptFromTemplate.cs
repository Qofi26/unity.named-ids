#if UNITY_2019_1_OR_NEWER
using System;
using System.IO;
using UnityEditor;

namespace Erem.NamedIds.Editor.ScriptTemplates
{
    public static class CreateNewPanelFromTemplate
    {
        private const string kMenuItem = "Assets/Create/NamedIds/";

        private const string kTemplate = "NewNamedIdsTemplate.cs.txt";

        [MenuItem(kMenuItem + "New Named Ids script", false, -1000)]
        public static void CreateNewNamedIds()
        {
            CreateFromTemplate(kTemplate);
        }

        #region Private API

        private static void CreateFromTemplate(string template, string name = null)
        {
            if (name == null)
            {
                name = Path.ChangeExtension(template, null);
                name = name.Replace("Template", string.Empty);
            }

            var path = FindTemplate(template);
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(path, name);
        }

        private static string FindTemplate(string name)
        {
            var path = UnityEngine.Application.dataPath;
            var files = Directory.GetFiles(path,
                name,
                SearchOption.AllDirectories);

            if (files.Length == 0)
            {
                throw new ArgumentException($"Script template {name} not found!");
            }

            return files[0];
        }

        #endregion
    }
}
#endif
