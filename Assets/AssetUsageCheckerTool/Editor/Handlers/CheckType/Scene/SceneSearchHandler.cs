#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Pampero.Editor
{
    /// <summary>
    /// A class that handles asset usage searches specific to scenes, extending the AssetUsageSearchHandler base class.
    /// </summary>
    public class SceneSearchHandler : AssetUsageSearchHandler
    {
        protected OpenSceneMode _openSceneMode = OpenSceneMode.Additive; 
        protected Scene _originalActiveScene;
        protected Scene _lastOpenedScene;

        protected override void PerformUsageCheckBasedOnCheckerType(Object asset, ObjectUsageChecker objectUsageChecker, out List<Object> objectsUsingAssetInScene)
        {
            objectsUsingAssetInScene = new();
            if (!AssetCheckerProvider.TryGetAssetSearcher(objectUsageChecker.AssetType, out BaseAssetSearcher searcher)) { return; }

            searcher.CheckAssetUsageInScene(asset, this, out objectsUsingAssetInScene);
        }

        #region Public
        public bool TryGetAllScenePaths(out string[] scenePaths)
        {
            scenePaths = AssetDatabase.FindAssets("t:Scene")
                .Select(g => AssetDatabase.GUIDToAssetPath(g))
                .ToArray();

            return scenePaths.Length > 0;
        }

        public bool TryGetGameObjectsFromScene(string scenePath, out GameObject[] sceneGameObjects)
        {
            sceneGameObjects = null;

            if (string.IsNullOrEmpty(scenePath) || !System.IO.File.Exists(scenePath))
            {
                Debug.LogWarning($"Invalid scene path: {scenePath}");
                return false;
            }

            //Avoid loading scenes that belong to read-only packages
            if (scenePath.StartsWith("Packages/")) 
            {
                Debug.LogWarning($"Scene {scenePath} won't be loaded since it belongs to a read-only package");
                return false; 
            }

            try
            {
                _lastOpenedScene = EditorSceneManager.OpenScene(scenePath, _openSceneMode);
                // Find all GameObjects in the scene.
                sceneGameObjects = _lastOpenedScene.GetRootGameObjects();

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error opening scene: {e.Message}");
                return false;
            }
        }


        public void CanAddSceneGameObjectsToObjectsUsingAssetList(List<Object> objectsUsingAsset, List<Object> objectsUsingAssetInScene)
        {
            if (_originalActiveScene != _lastOpenedScene) { return; }
            objectsUsingAsset.AddRange(objectsUsingAssetInScene);
        }

        public void GetCurrentActiveScene()
        {
            _originalActiveScene = EditorSceneManager.GetActiveScene();
        }

        public void AddSceneToObjectUsingAssetList(List<Object> objectsUsingAssetInScene, string path)
        {
            objectsUsingAssetInScene.Add(AssetDatabase.LoadAssetAtPath<Object>(path));
        }

        public void HandleSceneClosure()
        {
            if (_originalActiveScene == _lastOpenedScene) { return; }
            EditorSceneManager.CloseScene(_lastOpenedScene, true);
        }
        #endregion
    }
}
//EOF.
#endif