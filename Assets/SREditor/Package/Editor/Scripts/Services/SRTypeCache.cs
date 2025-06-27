using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SerializeReferenceEditor.Editor.Services
{
    public static class SRTypeCache
    {
        private static readonly Dictionary<string, Type> _typeCache = new();
        private static readonly Dictionary<Type, Type[]> _baseTypeCache = new();
        private static readonly Dictionary<Type, TypeInfo> _typeInfoCache = new();

        public static TypeInfo[] GetTypeInfos(Type baseType)
        {
            if (baseType == null)
            {
                return null;
            }

            Type[] types = GetSupportTypes(baseType);
            return GetTypeInfos(types);
        }

        public static TypeInfo[] GetTypeInfos(Type[] types)
        {
            if (types == null)
            {
                return null;
            }

            var results = new List<TypeInfo>();
            foreach (Type type in types)
            {
                if (type == null || type.GetCustomAttributes(typeof(SRHiddenAttribute), false).Length > 0)
                {
                    continue;
                }

                if (_typeInfoCache.TryGetValue(type, out TypeInfo typeInfo))
                {
                    results.Add(typeInfo);
                    continue;
                }

                string typeName = type.FullName;
                SRNameAttribute nameAttribute = type.GetCustomAttributes(typeof(SRNameAttribute), false)
                    .Select(attr => attr as SRNameAttribute)
                    .FirstOrDefault();

                if (nameAttribute != null)
                {
                    typeName = nameAttribute.FullName;
                }

                typeInfo = new TypeInfo
                {
                    Type = type,
                    Path = typeName,
                };

                _typeInfoCache[type] = typeInfo;
                results.Add(typeInfo);
            }

            return results.ToArray();
        }

        public static TypeInfo[] GetTypeInfos(string typeName)
        {
            if (string.IsNullOrEmpty(typeName))
            {
                Debug.LogError("[SR] Incorrect type name.");
            }

            Type type = GetTypeByName(typeName);
            if (type == null)
            {
                Debug.LogError("[SR] Incorrect type.");
            }

            return GetTypeInfos(type);
        }

        public static Type GetTypeByName(string typeName)
        {
            if (string.IsNullOrEmpty(typeName))
            {
                return null;
            }

            if (_typeCache.TryGetValue(typeName, out Type result))
            {
                return result;
            }

            string[] typeSplit = typeName.Split(char.Parse(" "));
            string typeAssembly = typeSplit[0];
            string typeClass = typeSplit[1];

            result = Type.GetType(typeClass + ", " + typeAssembly);

            if (result != null && result.GetCustomAttributes(typeof(SRHiddenAttribute), false).Length > 0)
            {
                return null;
            }

            if (result != null)
            {
                _typeCache[typeName] = result;
            }

            return result;
        }

        public static Type[] GetSupportTypes(Type baseType)
        {
            if (_baseTypeCache.TryGetValue(baseType, out Type[] result))
            {
                return result;
            }

            result = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(childType => IsCorrectChildTypeForSearchTree(baseType, childType))
                .ToArray();

            _baseTypeCache[baseType] = result;
            return result;
        }

        private static bool IsCorrectChildTypeForSearchTree(Type baseType, Type childType)
        {
            return !childType.IsAbstract
                && !childType.IsInterface
                && (childType == baseType ||
                    baseType.IsInterface
                        ? baseType.IsAssignableFrom(childType)
                        : childType.IsSubclassOf(baseType))
                && childType.GetCustomAttributes(typeof(SRHiddenAttribute), false).Length <= 0;
        }
    }
}