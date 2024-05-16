#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;

namespace Pampero.Editor
{
    public class MonoscriptSearchHandler : ObjectSearchHandler
    {
        public MonoscriptSearchHandler(Object asset, AssetUsageSearchHandler assetUsageSearchHandler) : base(asset, assetUsageSearchHandler) { }

        public override void GetObjectsUsingAsset(out List<Object> objectsUsingAsset)
        {
            switch (_assetUsageSearchHandler)
            {
                case SceneSearchHandler:
                    CheckMonoScriptUsageInScene(out objectsUsingAsset);
                    break;
                case AssetUsageSearchHandler:
                    CheckMonoScriptUsageInAssetDatabase(out objectsUsingAsset);
                    break;
                default:
                    objectsUsingAsset = new List<Object>();
                    break;
            }
        }

        private void CheckMonoScriptUsageInAssetDatabase(out List<Object> objectsUsingAsset)
        {
            _assetUsageSearchHandler.CheckAssetUsageAsComponentInGameObjectsCollection(_asset,
                                            _assetUsageSearchHandler.FindAllGameObjectsInProject(),
                                            out objectsUsingAsset);
        }

        private void CheckMonoScriptUsageInScene(out List<Object> objectsUsingAsset)
        {
            objectsUsingAsset = new List<Object>();

            // Cast the AssetUsageSearchHandler to SceneSearchHandler
            var sceneSearchHandler = _assetUsageSearchHandler as SceneSearchHandler;

            sceneSearchHandler.GetCurrentActiveScene();

            if (!sceneSearchHandler.TryGetAllScenePaths(out string[] scenesPath))
            {
                Debug.LogError("Could not get scenePaths");
                return;
            }

            foreach (var path in scenesPath)
            {
                if (!sceneSearchHandler.TryGetGameObjectsFromScene(path, out GameObject[] sceneGOs)) { continue; }
                if (sceneSearchHandler.CheckAssetUsageAsComponentInGameObjectsCollection(_asset, sceneGOs, out List<Object> objectsUsingAssetInScene))
                {
                    sceneSearchHandler.AddSceneToObjectUsingAssetList(objectsUsingAsset, path);
                    //Handle gameObjects path if scene is not active
                    sceneSearchHandler.CanAddSceneGameObjectsToObjectsUsingAssetList(objectsUsingAsset, objectsUsingAssetInScene);
                }
                sceneSearchHandler.HandleSceneClosure();
            }
        }
    }
}
//EOF.
#endif