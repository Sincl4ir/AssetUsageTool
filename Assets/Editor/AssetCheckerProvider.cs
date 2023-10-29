#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace Pampero.Editor
{
    public static class AssetCheckerProvider
    {
        public static bool TryCreateChecker(Object asset, out IAssetUsageChecker iAssetUsageChecker)
        {
            iAssetUsageChecker = null;

            switch (asset)
            {
                case MonoScript:
                    iAssetUsageChecker = new MonoScriptUsageChecker(asset);
                    return true;
                case GameObject:
                    iAssetUsageChecker = new GameObjectUsageChecker(asset);
                    return true;
                default:
                    CheckAssetType(asset);
                    Debug.LogWarning("Could not get a proper IAssetUsageChecker for the selected asset");
                    return false; 
            }
        }

        public static void HandleUsageCheckSearchers(AssetCheckType search, out IAssetUsageSearchHandler iAssetUsageSearchHandler)
        {
            iAssetUsageSearchHandler = null;

            switch (search)
            {
                case AssetCheckType.SceneCheck:
                    iAssetUsageSearchHandler = new SceneSearchHandler();
                    break;
                case AssetCheckType.AssetDatabaseCheck:
                    iAssetUsageSearchHandler = new AssetDatabaseSearchHandler();
                    break;
            }
        }

        private static void CheckAssetType(Object asset)
        {
            string assetPath = AssetDatabase.GetAssetPath(asset);
            if (string.IsNullOrEmpty(assetPath)) 
            {
                Debug.Log("Not an Asset (e.g., Scene or GameObject in Hierarchy)");
                return; 
            }

            System.Type assetType = AssetDatabase.GetMainAssetTypeAtPath(assetPath);

            if (assetType == null)
            {
                Debug.LogWarning("Unknown Asset Type");
                return;
            }

            Debug.Log($"Asset Type: {assetType.Name}");
        }
    }
}
//EOF.
#endif