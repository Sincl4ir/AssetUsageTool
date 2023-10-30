#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Pampero.Editor
{
    public class AssetUsageController
    {
        private AssetUsageEditorTool _assetUsageEditorTool; // Reference to the view (AssetUsageEditorTool)
        private IAssetUsageChecker _assetUsageChecker;

        public AssetUsageController(AssetUsageEditorTool view)
        {
            _assetUsageEditorTool = view;
        }

        public bool HandleCheckAssetUsageRequest(Object asset, out List<Object> objectsUsingAsset)
        {
            objectsUsingAsset = null;
            if (!AssetCheckerProvider.TryCreateChecker(asset, out _assetUsageChecker)) { return false; }
            return _assetUsageChecker.CheckAssetUsage(out objectsUsingAsset);
        }

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