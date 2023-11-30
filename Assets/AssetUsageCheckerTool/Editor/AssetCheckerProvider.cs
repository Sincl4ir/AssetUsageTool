#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace Pampero.Editor
{
    /// <summary>
    /// Provides methods for creating asset usage checkers and asset usage search handlers.
    /// </summary>
    public static class AssetCheckerProvider
    {
        #region Public
        /// <summary>
        /// Tries to create an asset usage checker for the specified asset.
        /// </summary>
        /// <param name="asset">The asset to check usage for.</param>
        /// <param name="iAssetUsageChecker">The created asset usage checker (if successful).</param>
        /// <returns>True if an asset usage checker is created successfully; otherwise, false.</returns>
        public static bool TryCreateChecker(Object asset, out IAssetUsageChecker iAssetUsageChecker)
        {
            iAssetUsageChecker = null;
            if (!GetAssetType(asset, out var assetType)) { return false; }

            iAssetUsageChecker = new ObjectUsageChecker(asset, assetType);

            return true;
        }

        /// <summary>
        /// Handles the creation of asset usage search handlers based on the specified search type.
        /// </summary>
        /// <param name="search">The type of asset usage search to perform.</param>
        /// <param name="iAssetUsageSearchHandler">The created asset usage search handler (if successful).</param>
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
        #endregion

        #region Private
        private static bool GetAssetType(Object asset, out AssetType assetType)
        {
            switch (asset)
            {
                case MonoScript:
                    assetType = AssetType.Monoscript;
                    break;
                //iAssetUsageChecker = new MonoScriptUsageChecker(asset);
                //return true;
                case GameObject:
                    assetType = AssetType.GameObject;
                    break;
                //iAssetUsageChecker = new GameObjectUsageChecker(asset);
                //return true;
                default:
                    CheckAssetType(asset);
                    Debug.LogWarning("Could not get a proper IAssetUsageChecker for the selected asset");
                    assetType = AssetType.Unknown;
                    return false;
            }

            return true;
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
        #endregion
    }
}
//EOF.
#endif