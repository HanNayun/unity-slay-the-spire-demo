using System;
using System.Linq;
using SerializeReferenceEditor.Editor.Settings;

namespace SerializeReferenceEditor.Editor
{
    public class NameService
    {
        private readonly SREditorSettings _settings = SREditorSettings.GetOrCreateSettings();

        public string GetTypeName(string typeName)
        {
            return GetTypeName(typeName, _settings.ShowNameType);
        }

        public static string GetTypeName(string typeName, ShowNameType showType)
        {
            if (string.IsNullOrEmpty(typeName))
            {
                return "(empty)";
            }

            if (TypeByName(typeName)?
                    .GetCustomAttributes(typeof(SRNameAttribute), false)
                    .FirstOrDefault()
                is SRNameAttribute nameAttr)
            {
                return nameAttr.Name;
            }

            return showType switch
            {
                ShowNameType.FullName => GetFullName(typeName),
                ShowNameType.OnlyNameSpace => GetOnlyNameSpace(typeName),
                ShowNameType.OnlyCurrentType => GetOnlyCurrentType(typeName),
                _ => throw new ArgumentOutOfRangeException(),
            };
        }

        private static string GetOnlyCurrentType(string typeName)
        {
            string nameSpace = GetOnlyNameSpace(typeName);
            int index = nameSpace.LastIndexOf('.');
            if (index >= 0)
            {
                return nameSpace.Substring(index + 1);
            }

            return typeName;
        }

        private static string GetOnlyNameSpace(string typeName)
        {
            string fullName = GetFullName(typeName);
            int index = fullName.LastIndexOf(' ');
            if (index >= 0)
            {
                return fullName.Substring(index + 1);
            }

            return typeName;
        }

        private static string GetFullName(string typeName)
        {
            return typeName;
        }

        private static Type TypeByName(string className)
        {
            string[] splitClassName = className.Split(' ');
            return Type.GetType(
                string.Format(
                    "{0}, {1}",
                    splitClassName[1],
                    splitClassName[0]));
        }

        public string[] GetSplitPathType(string pathType)
        {
            return pathType.Split(_settings.NameSeparators);
        }
    }
}