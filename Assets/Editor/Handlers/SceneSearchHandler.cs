#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Pampero.Editor
{
    public class SceneSearchHandler : AssetUsageSearchHandler
    {
        protected OpenSceneMode _openSceneMode = OpenSceneMode.Additive; 
        protected Scene _originalActiveScene;
        protected Scene _lastOpenedScene;

        #region Public
        public override bool HandleAssetUsageSearch(Object asset, ObjectUsageChecker objectUsageChecker, out List<Object> objectsUsingAssetInScene)
        {
            //CheckMonoScriptUsageInScene(asset, out objectsUsingAssetInScene);
            PerformUsageCheckBasedOnCheckerType(asset, objectUsageChecker, out objectsUsingAssetInScene);
            return objectsUsingAssetInScene.Count > 0;
        }

        protected override void PerformUsageCheckBasedOnCheckerType(Object asset, ObjectUsageChecker objectUsageChecker, out List<Object> objectsUsingAssetInScene)
        {
            switch (objectUsageChecker)
            {
                case MonoScriptUsageChecker:
                    CheckMonoScriptUsageInScene(asset, out objectsUsingAssetInScene);
                    break;
                default:
                    objectsUsingAssetInScene = new List<Object>();
                    Debug.LogWarning("There is no implementation for this ObjectUsageCheker yet");
                    break;
            }
        }

        public void CheckMonoScriptUsageInScene(Object asset, out List<Object> objectsUsingAsset)
        {
            objectsUsingAsset = new List<Object>();
            GetCurrentActiveScene();

            if (!TryGetAllScenePaths(out string[] scenesPath))
            {
                Debug.LogError("Could not get scenePaths");
                return;
            }

            foreach (var path in scenesPath)
            {
                if (!TryGetGameObjectsFromScene(path, out GameObject[] sceneGOs)) { continue; }
                if (CheckAssetUsageAsComponentInGameObjectsCollection(asset, sceneGOs, out List<Object> objectsUsingAssetInScene))
                {
                    AddSceneToObjectUsingAssetList(objectsUsingAsset, path);
                    //Handle gameObjects path if scene is not active
                    CanAddSceneGameObjectsToObjectsUsingAssetList(objectsUsingAsset, objectsUsingAssetInScene);
                }
                HandleSceneClosure();
            }
        }
        #endregion

        #region Private 
        private bool TryGetAllScenePaths(out string[] scenePaths)
        {
            scenePaths = AssetDatabase.FindAssets("t:Scene")
                .Select(g => AssetDatabase.GUIDToAssetPath(g))
                .ToArray();

            return scenePaths.Length > 0;
        }

        private bool TryGetGameObjectsFromScene(string scenePath, out GameObject[] sceneGameObjects)
        {
            sceneGameObjects = null;

            if (string.IsNullOrEmpty(scenePath) || !System.IO.File.Exists(scenePath))
            {
                Debug.LogWarning($"Invalid scene path: {scenePath}");
                return false;
            }

            _lastOpenedScene = EditorSceneManager.OpenScene(scenePath, _openSceneMode);

            // Find all GameObjects in the scene.
            sceneGameObjects = _lastOpenedScene.GetRootGameObjects();

            return true;
        }

        private void CanAddSceneGameObjectsToObjectsUsingAssetList(List<Object> objectsUsingAsset, List<Object> objectsUsingAssetInScene)
        {
            if (_originalActiveScene != _lastOpenedScene) { return; }
            objectsUsingAsset.AddRange(objectsUsingAssetInScene);
        }


        private void GetCurrentActiveScene()
        {
            _originalActiveScene = EditorSceneManager.GetActiveScene();
        }

        private void AddSceneToObjectUsingAssetList(List<Object> objectsUsingAssetInScene, string path)
        {
            objectsUsingAssetInScene.Add(AssetDatabase.LoadAssetAtPath<Object>(path));
        }

        private void HandleSceneClosure()
        {
            if (_originalActiveScene == _lastOpenedScene) { return; }
            EditorSceneManager.CloseScene(_lastOpenedScene, true);
        }
        #endregion
    }
}
//EOF.
#endif