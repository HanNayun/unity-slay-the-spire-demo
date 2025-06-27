using System.Collections.Generic;
using SerializeReferenceEditor.Editor.MissingTypesValidator.ReportFormats;
using UnityEditor;
using UnityEngine;

namespace SerializeReferenceEditor.Editor.MissingTypesValidator
{
    public class SRMissingTypesValidator
    {
        [MenuItem("Tools/SREditor/Check MissingTypes")]
        public static void Check()
        {
            SRMissingTypesValidatorConfig[] configs = Resources.FindObjectsOfTypeAll<SRMissingTypesValidatorConfig>();
            foreach (SRMissingTypesValidatorConfig config in configs)
            {
                foreach (AssetChecker checker in config.Checkers)
                {
                    var assets = new List<Object>();
                    checker.AssetsLoaders.TryLoadAssetsForCheck(assets);
                    foreach (Object asset in assets)
                    {
                        CheckAsset(asset, checker.ReportType);
                    }

                    checker.ReportType.Finished();
                }
            }
        }

        private static void CheckAsset(
            Object host,
            IAssetMissingTypeReport report)
        {
            if (!SerializationUtility.HasManagedReferencesWithMissingTypes(host))
            {
                return;
            }

            ManagedReferenceMissingType[]
                missingTypes = SerializationUtility.GetManagedReferencesWithMissingTypes(host);
            report.AttachMissingTypes(host, missingTypes);
        }
    }
}