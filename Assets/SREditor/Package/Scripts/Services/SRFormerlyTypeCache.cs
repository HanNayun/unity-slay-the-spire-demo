#if UNITY_EDITOR
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    namespace SerializeReferenceEditor.Services
    {
        public static class SRFormerlyTypeCache
        {
            private static readonly Dictionary<FormerlySerializedTypeAttribute, Type> _attributeTypes = new();

            static SRFormerlyTypeCache()
            {
                CollectTypeReplacements();
            }

            private static void CollectTypeReplacements()
            {
                Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
                foreach (Assembly assembly in assemblies)
                {
                    try
                    {
                        if (assembly.IsDynamic)
                        {
                            continue;
                        }

                        CollectTypeReplacementsForAssembly(assembly);
                    }
                    catch
                    {
                        continue;
                    }
                }
            }

            private static void CollectTypeReplacementsForAssembly(Assembly assembly)
            {
                string assemblyName = assembly.GetName().Name;

                Dictionary<FormerlySerializedTypeAttribute, Type> newAttributeTypes = _attributeTypes
                    .Where(kvp => kvp.Value.Assembly.GetName().Name != assemblyName)
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

                _attributeTypes.Clear();
                foreach (KeyValuePair<FormerlySerializedTypeAttribute, Type> kvp in newAttributeTypes)
                {
                    _attributeTypes[kvp.Key] = kvp.Value;
                }

                try
                {
                    Type[] types = assembly.GetTypes();
                    foreach (Type type in types)
                    {
                        IEnumerable<FormerlySerializedTypeAttribute> attributes =
                            type.GetCustomAttributes<FormerlySerializedTypeAttribute>();
                        foreach (FormerlySerializedTypeAttribute attr in attributes)
                        {
                            _attributeTypes[attr] = type;
                        }
                    }
                }
                catch (ReflectionTypeLoadException)
                {
                }
            }

            public static void CollectTypeReplacementsForAssembly(string assemblyPath)
            {
                try
                {
                    Assembly assembly = Assembly.LoadFrom(assemblyPath);
                    CollectTypeReplacementsForAssembly(assembly);
                }
                catch
                {
                }
            }

            public static Type GetTypeForAttribute(FormerlySerializedTypeAttribute attribute)
            {
                return _attributeTypes.GetValueOrDefault(attribute);
            }

            public static Type GetReplacementType(string assemblyName, string typeName)
            {
                if (string.IsNullOrEmpty(typeName))
                {
                    return null;
                }

                return _attributeTypes
                    .Where(kvp =>
                        (string.IsNullOrEmpty(assemblyName) || kvp.Key.OldAssemblyName == assemblyName) &&
                        $"{kvp.Key.OldNamespace}.{kvp.Key.OldTypeName}".TrimStart('.') == typeName)
                    .Select(kvp => kvp.Value)
                    .FirstOrDefault();
            }

            public static IEnumerable<(string oldAssembly, string oldType, Type newType)> GetAllReplacements()
            {
                return _attributeTypes.Select(kvp =>
                    (kvp.Key.OldAssemblyName,
                        $"{kvp.Key.OldNamespace}.{kvp.Key.OldTypeName}".TrimStart('.'),
                        kvp.Value));
            }
        }
    }
#endif