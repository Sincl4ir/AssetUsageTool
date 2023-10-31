#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Pampero.Editor
{
    /// <summary>
    /// The controller responsible for mediating interactions between the model (AssetUsageChecker) and view (AssetUsageEditorTool) components.
    /// </summary>
    public class AssetUsageController
    {
        private AssetUsageEditorTool _assetUsageEditorTool; 
        private IAssetUsageChecker _assetUsageChecker;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetUsageController"/> class.
        /// </summary>
        /// <param name="view">The associated view component.</param>
        public AssetUsageController(AssetUsageEditorTool view)
        {
            _assetUsageEditorTool = view;
        }

        /// <summary>
        /// Handles a request to check asset usage for the specified asset.
        /// </summary>
        /// <param name="asset">The asset to check for usage.</param>
        /// <param name="objectsUsingAsset">A list of objects that use the asset.</param>
        /// <returns><c>true</c> if the check was successful; otherwise, <c>false</c>.</returns>
        public bool HandleCheckAssetUsageRequest(Object asset, out List<Object> objectsUsingAsset)
        {
            objectsUsingAsset = null;
            if (!AssetCheckerProvider.TryCreateChecker(asset, out _assetUsageChecker)) { return false; }
            return _assetUsageChecker.CheckAssetUsage(out objectsUsingAsset);
        }

        /// <summary>
        /// Handles warnings about inactive scenes related to the asset usage.
        /// </summary>
        /// <param name="objectsUsingAsset">A list of objects that use the asset.</param>
        public void HandleInactiveScenesWarning(List<Object> objectsUsingAsset)
        {
            foreach (var obj in objectsUsingAsset)
            {
                if (obj is not SceneAsset sceneAsset) { continue; }

                string scenePath = AssetDatabase.GetAssetPath(sceneAsset);
                var scene = EditorSceneManager.GetSceneByPath(scenePath);

                if (scene.isLoaded) { continue; }
                // Display a warning message
                _assetUsageEditorTool.DisplayInactiveScenesWarning();
                break; // Display the warning only once
            }
        }
    }
}
//EOF.
#endif