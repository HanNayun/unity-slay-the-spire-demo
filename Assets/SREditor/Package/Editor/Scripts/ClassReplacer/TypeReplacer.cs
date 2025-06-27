using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;

namespace SerializeReferenceEditor.Editor.ClassReplacer
{
    public static class TypeReplacer
    {
        public static bool ReplaceTypeInFile(string path, string oldTypePattern, string newTypePattern)
        {
            if (AssetDatabase.IsValidFolder(path))
            {
                return false;
            }

            string content = File.ReadAllText(path);
            bool wasModified = false;

            string newClassName;
            string newNamespace = string.Empty;
            string newAssembly = string.Empty;

            if (newTypePattern.Contains(","))
            {
                string[] parts = newTypePattern.Split(new[] { ',' }, 2);
                newAssembly = parts[0].Trim();
                string fullType = parts[1].Trim();
                string[] typeParts = fullType.Split('.');
                newClassName = typeParts[^1];
                if (typeParts.Length > 1)
                {
                    newNamespace = string.Join(".", typeParts, 0, typeParts.Length - 1);
                }
            }
            else
            {
                string[] typeParts = newTypePattern.Split('.');
                newClassName = typeParts[^1];
                if (typeParts.Length > 1)
                {
                    newNamespace = string.Join(".", typeParts, 0, typeParts.Length - 1);
                }
            }

            string oldClassName;
            string oldNamespace;
            string oldAssembly;

            if (oldTypePattern.Contains(","))
            {
                string[] parts = oldTypePattern.Split(new[] { ',' }, 2);
                oldAssembly = parts[0].Trim();
                string fullType = parts[1].Trim();
                string[] typeParts = fullType.Split('.');
                oldClassName = typeParts[^1];
                if (typeParts.Length > 1)
                {
                    oldNamespace = string.Join(".", typeParts, 0, typeParts.Length - 1);
                }
                else
                {
                    oldNamespace = newNamespace;
                }
            }
            else
            {
                string[] typeParts = oldTypePattern.Split('.');
                oldClassName = typeParts[^1];
                if (typeParts.Length > 1)
                {
                    oldNamespace = string.Join(".", typeParts, 0, typeParts.Length - 1);
                }
                else
                {
                    oldNamespace = newNamespace;
                }

                oldAssembly = newAssembly;
            }

            Match referencesSection = Regex.Match(content,
                @"references:\s*\n\s*version:\s*2\s*\n\s*RefIds:\s*(?:\n|.)*?(?=\n\s*\n|$)");
            if (referencesSection.Success)
            {
                string newReferencesSection = Regex.Replace(
                    referencesSection.Value,
                    @"type:\s*{\s*class:\s*(\w+),\s*ns:\s*([\w.]+)(?:,\s*asm:\s*(\w+))?}",
                    m =>
                    {
                        string className = m.Groups[1].Value;
                        string ns = m.Groups[2].Value;
                        string asm = m.Groups[3].Success ? m.Groups[3].Value : string.Empty;

                        if (className != oldClassName)
                        {
                            return m.Value;
                        }

                        if (oldNamespace != newNamespace && ns != oldNamespace)
                        {
                            return m.Value;
                        }

                        if (oldAssembly != newAssembly && asm != oldAssembly)
                        {
                            return m.Value;
                        }

                        string resultAssembly = string.IsNullOrEmpty(newAssembly) ? "Assembly-CSharp" : newAssembly;
                        return $"type: {{ class: {newClassName}, ns: {newNamespace}, asm: {resultAssembly} }}";
                    }
                );

                if (referencesSection.Value != newReferencesSection)
                {
                    content = content.Replace(referencesSection.Value, newReferencesSection);
                    wasModified = true;
                }
            }

            string typePattern =
                $@"type:\s*{{\s*class:\s*{oldClassName},\s*ns:\s*{oldNamespace}(?:,\s*asm:\s*{oldAssembly})?}}";
            if (Regex.IsMatch(content, typePattern))
            {
                string resultAssembly = string.IsNullOrEmpty(newAssembly) ? "Assembly-CSharp" : newAssembly;
                string replacement = $"type: {{ class: {newClassName}, ns: {newNamespace}, asm: {resultAssembly} }}";
                content = Regex.Replace(content, typePattern, replacement);
                wasModified = true;
            }

            string managedReferencesPattern = @"managedReferences\[\d+\]:\s*(\w+)\s+([\w.]+(?:\.[\w.]+)*)";
            MatchCollection managedReferencesMatches = Regex.Matches(content, managedReferencesPattern);

            foreach (Match match in managedReferencesMatches)
            {
                string assembly = match.Groups[1].Value;
                string fullType = match.Groups[2].Value;
                string[] typeParts = fullType.Split('.');
                string className = typeParts[^1];
                string ns = typeParts.Length > 1 ? string.Join(".", typeParts, 0, typeParts.Length - 1) : string.Empty;

                if (className != oldClassName)
                {
                    continue;
                }

                if (oldNamespace != newNamespace && ns != oldNamespace)
                {
                    continue;
                }

                if (oldAssembly != newAssembly && assembly != oldAssembly)
                {
                    continue;
                }

                string resultAssembly = string.IsNullOrEmpty(newAssembly) ? assembly : newAssembly;
                string newFullType = string.IsNullOrEmpty(newNamespace)
                    ? newClassName
                    : $"{newNamespace}.{newClassName}";
                string newReference =
                    $"managedReferences[{match.Groups[0].Value.Split('[')[1].Split(']')[0]}]: {resultAssembly} {newFullType}";
                content = content.Replace(match.Value, newReference);
                wasModified = true;
            }

            if (wasModified)
            {
                File.WriteAllText(path, content);
            }

            return wasModified;
        }
    }
}