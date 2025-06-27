using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using SerializeReferenceEditor.Editor.Settings;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SerializeReferenceEditor.Editor.DoubleCleaner
{
    [InitializeOnLoad]
    public static class SRDuplicateCleaner
    {
        static SRDuplicateCleaner()
        {
            AssetChangeDetector.ChangeEvent -= OnAssetChanged;
            AssetChangeDetector.ChangeEvent += OnAssetChanged;
        }

        private static void OnAssetChanged(Object asset)
        {
            if (asset == null)
            {
                return;
            }

            SRDuplicateMode duplicateMode =
                SREditorSettings.GetOrCreateSettings()?.DuplicateMode ?? SRDuplicateMode.Null;

            var seenObjects = new HashSet<object>();

            if (asset is GameObject gameObject)
            {
                foreach (Component component in gameObject.GetComponents<Component>())
                {
                    if (component == null)
                    {
                        continue;
                    }

                    var serializedObject = new SerializedObject(component);
                    SerializedProperty iterator = serializedObject.GetIterator();
                    ProcessSerializedProperty(iterator, duplicateMode, seenObjects);
                }
            }
            else
            {
                try
                {
                    var serializedObject = new SerializedObject(asset);
                    SerializedProperty iterator = serializedObject.GetIterator();
                    ProcessSerializedProperty(iterator, duplicateMode, seenObjects);
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex);
                }
            }
        }

        private static void ProcessSerializedProperty(SerializedProperty property, SRDuplicateMode duplicateMode,
                                                      HashSet<object> seenObjects)
        {
            while (property.Next(true))
            {
                if (property.propertyType == SerializedPropertyType.ManagedReference)
                {
                    object managedReferenceValue = property.managedReferenceValue;

                    if (managedReferenceValue != null && !seenObjects.Add(managedReferenceValue))
                    {
                        bool refChanged = false;
                        switch (duplicateMode)
                        {
                            case SRDuplicateMode.Null:
                                property.managedReferenceValue = null;
                                refChanged = true;
                                break;

                            case SRDuplicateMode.Default:
                                object currentValue = property.managedReferenceValue;
                                if (currentValue == null)
                                {
                                    property.managedReferenceValue = null;
                                    refChanged = true;
                                    break;
                                }

                                string propertyPath = property.propertyPath;
                                bool isArrayElement = propertyPath.EndsWith("]");

                                if (!isArrayElement)
                                {
                                    SerializedProperty parentProperty =
                                        property.serializedObject.FindProperty(
                                            propertyPath.Substring(0, propertyPath.LastIndexOf('.')));

                                    if (parentProperty != null)
                                    {
                                        object parentObject = null;

                                        if (parentProperty.propertyType == SerializedPropertyType.ManagedReference)
                                        {
                                            parentObject = parentProperty.managedReferenceValue;
                                        }
                                        else if (parentProperty.propertyType == SerializedPropertyType.ObjectReference)
                                        {
                                            parentObject = parentProperty.objectReferenceValue;
                                        }
                                        else if (parentProperty.propertyType == SerializedPropertyType.Generic)
                                        {
                                            Object targetObject = parentProperty.serializedObject.targetObject;
                                            string parentPath =
                                                propertyPath.Substring(0, propertyPath.LastIndexOf('.'));
                                            parentObject = GetObjectFromPath(targetObject, parentPath);
                                        }

                                        if (parentObject != null)
                                        {
                                            Type parentType = parentObject.GetType();
                                            string fieldName =
                                                propertyPath.Substring(propertyPath.LastIndexOf('.') + 1);
                                            FieldInfo field = parentType.GetField(fieldName,
                                                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                                            if (field != null)
                                            {
                                                property.managedReferenceValue = GetDefaultValueFromField(field);
                                                refChanged = true;
                                            }
                                            else
                                            {
                                                property.managedReferenceValue = null;
                                                refChanged = true;
                                            }
                                        }
                                        else
                                        {
                                            property.managedReferenceValue = null;
                                            refChanged = true;
                                        }
                                    }
                                    else
                                    {
                                        property.managedReferenceValue = null;
                                        refChanged = true;
                                    }
                                }
                                else
                                {
                                    Type currentType = currentValue.GetType();
                                    object newInstance = Activator.CreateInstance(currentType);

                                    foreach (FieldInfo field in currentType.GetFields(BindingFlags.Public |
                                                 BindingFlags.NonPublic | BindingFlags.Instance))
                                    {
                                        field.SetValue(newInstance, GetDefaultValueFromField(field));
                                    }

                                    property.managedReferenceValue = newInstance;
                                    refChanged = true;
                                }

                                break;

                            case SRDuplicateMode.Copy:
                                Type sourceType = managedReferenceValue.GetType();
                                try
                                {
                                    object newInstance = CreateDeepCopy(managedReferenceValue);
                                    property.managedReferenceValue = newInstance;
                                    seenObjects.Add(newInstance);
                                    refChanged = true;
                                }
                                catch (Exception e)
                                {
                                    Debug.LogError($"Failed to create instance of type {sourceType.Name}: {e.Message}");
                                    property.managedReferenceValue = null;
                                    refChanged = true;
                                }

                                break;
                        }

                        if (refChanged)
                        {
                            property.serializedObject.ApplyModifiedProperties();
                        }
                    }
                }
            }
        }

        private static object GetDefaultValueFromField(FieldInfo field)
        {
            try
            {
                if (field.GetCustomAttribute<SerializeReference>() != null)
                {
                    if (field.DeclaringType != null)
                    {
                        object tempInstance = Activator.CreateInstance(field.DeclaringType);
                        object defaultValue = field.GetValue(tempInstance);

                        if (defaultValue != null)
                        {
                            Type defaultType = defaultValue.GetType();
                            object newInstance = Activator.CreateInstance(defaultType);

                            foreach (FieldInfo f in defaultType.GetFields(BindingFlags.Public | BindingFlags.NonPublic |
                                         BindingFlags.Instance))
                            {
                                object value = f.GetValue(defaultValue);
                                if (value != null)
                                {
                                    f.SetValue(newInstance, value);
                                }
                            }

                            return newInstance;
                        }
                    }
                }

                Type fieldType = field.FieldType;
                if (fieldType.IsArray)
                {
                    Type elementType = fieldType.GetElementType();
                    if (elementType != null)
                    {
                        return Array.CreateInstance(elementType, 0);
                    }

                    return null;
                }

                if (typeof(IList).IsAssignableFrom(fieldType) && fieldType.IsGenericType)
                {
                    return Activator.CreateInstance(fieldType);
                }

                return null;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to get default value for field {field.Name}: {e.Message}");
                return null;
            }
        }

        private static object CreateDeepCopy(object source)
        {
            if (source == null) return null;

            Type sourceType = source.GetType();
            object newInstance = Activator.CreateInstance(sourceType);

            foreach (FieldInfo field in sourceType.GetFields(BindingFlags.Public | BindingFlags.NonPublic |
                         BindingFlags.Instance))
            {
                object value = field.GetValue(source);
                if (value == null)
                {
                    field.SetValue(newInstance, null);
                    continue;
                }

                bool isSerializeReference = field.GetCustomAttribute<SerializeReference>() != null;

                if (isSerializeReference)
                {
                    object copiedValue = CreateDeepCopy(value);
                    field.SetValue(newInstance, copiedValue);
                }
                else
                {
                    field.SetValue(newInstance, value);
                }
            }

            return newInstance;
        }

        private static object GetObjectFromPath(object root, string path)
        {
            if (root == null || string.IsNullOrEmpty(path))
            {
                return null;
            }

            string[] parts = path.Split('.');
            object current = root;

            for (int i = 0; i < parts.Length; i++)
            {
                if (current == null)
                {
                    return null;
                }

                string part = parts[i];

                if (part == "Array" && i + 1 < parts.Length && parts[i + 1].StartsWith("data["))
                {
                    string arrayIndexPart = parts[i + 1];
                    string indexStr = arrayIndexPart.Substring(5, arrayIndexPart.Length - 6);
                    if (int.TryParse(indexStr, out int index))
                    {
                        var array = current as Array;
                        if (array != null && index >= 0 && index < array.Length)
                        {
                            current = array.GetValue(index);
                        }
                        else
                        {
                            return null;
                        }
                    }

                    i++;
                    continue;
                }

                FieldInfo field = current.GetType().GetField(part,
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (field == null)
                {
                    return null;
                }

                current = field.GetValue(current);
            }

            return current;
        }
    }
}