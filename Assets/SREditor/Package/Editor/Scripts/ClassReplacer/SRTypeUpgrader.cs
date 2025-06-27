using System;
using System.Collections.Generic;
using SerializeReferenceEditor.Editor.Settings;
using SerializeReferenceEditor.Services;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace SerializeReferenceEditor.Editor.ClassReplacer
{
    [InitializeOnLoad]
    public class SRTypeUpgrader : AssetPostprocessor
    {
        private static HashSet<string> _processedAssets = new();
        private static HashSet<int> _processingObjects = new();

        static SRTypeUpgrader()
        {
            _processedAssets.Clear();
            _processingObjects.Clear();

            EditorSceneManager.sceneSaving -= OnSceneSaving;
            EditorSceneManager.sceneSaving += OnSceneSaving;

            Selection.selectionChanged -= OnSelectionChanged;
            Selection.selectionChanged += OnSelectionChanged;

            OnSelectionChanged();
        }

        private static void OnPostprocessAllAssets(
            string[] importedAssets,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            if (!SREditorSettings.GetOrCreateSettings()?.FormerlySerializedTypeOnAssetImport ?? false)
            {
                return;
            }

            foreach (string assetPath in importedAssets)
            {
                var asset = AssetDatabase.LoadAssetAtPath<Object>(assetPath);
                if (asset != null)
                {
                    ProcessObject(asset);
                }
            }
        }

        private static void OnSceneSaving(Scene scene, string path)
        {
            if (!SREditorSettings.GetOrCreateSettings()?.FormerlySerializedTypeOnSceneSave ?? false)
            {
                return;
            }

            foreach (GameObject rootObj in scene.GetRootGameObjects())
            {
                foreach (Component component in rootObj.GetComponentsInChildren<Component>(true))
                {
                    if (component != null)
                    {
                        ProcessObject(component);
                    }
                }
            }
        }

        private static void OnSelectionChanged()
        {
            if (!SREditorSettings.GetOrCreateSettings()?.FormerlySerializedTypeOnAssetSelect ?? false)
            {
                return;
            }

            if (Selection.objects != null && Selection.objects.Length > 0)
            {
                foreach (Object obj in Selection.objects)
                {
                    if (_processingObjects.Contains(obj.GetInstanceID()))
                    {
                        continue;
                    }

                    ProcessObject(obj);
                }
            }
        }

        private static void ProcessObject(Object obj)
        {
            if (obj == null)
            {
                return;
            }

            int instanceId = obj.GetInstanceID();
            if (!_processingObjects.Add(instanceId))
            {
                return;
            }

            try
            {
                string assetPath = AssetDatabase.GetAssetPath(obj);
                if (string.IsNullOrEmpty(assetPath))
                {
                    return;
                }

                if (PrefabUtility.IsPartOfPrefabAsset(obj))
                {
                    if (!_processedAssets.Add(assetPath))
                    {
                        return;
                    }
                }

                bool modified = false;
                foreach ((string oldAssembly, string oldType, Type newType) in SRFormerlyTypeCache.GetAllReplacements())
                {
                    string oldTypePattern = string.IsNullOrEmpty(oldAssembly) ? oldType : $"{oldAssembly}, {oldType}";
                    string newAssembly = newType.Assembly.GetName().Name;
                    string newTypePattern = string.IsNullOrEmpty(newAssembly)
                        ? newType.FullName
                        : $"{newAssembly}, {newType.FullName}";

                    if (TypeReplacer.ReplaceTypeInFile(assetPath, oldTypePattern, newTypePattern))
                    {
                        modified = true;
                    }
                }

                if (modified)
                {
                    AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
                    EditorUtility.SetDirty(obj);

                    EditorApplication.delayCall += () =>
                    {
                        if (Selection.activeObject == obj)
                        {
                            Selection.activeObject = null;
                            EditorApplication.delayCall += () =>
                            {
                                Selection.activeObject = obj;
                                _processingObjects.Remove(instanceId);
                                _processedAssets.Clear();
                            };
                        }
                        else
                        {
                            _processingObjects.Remove(instanceId);
                            _processedAssets.Clear();
                        }
                    };
                }
                else
                {
                    _processingObjects.Remove(instanceId);
                }
            }
            catch
            {
                _processingObjects.Remove(instanceId);
                throw;
            }
        }
    }
}