#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;

namespace Pampero.Editor
{
    /// <summary>
    /// Checks the usage of a Monoscript asset.
    /// </summary>
    public class MonoScriptSearcher : BaseAssetSearcher
    {
        public override void CheckAssetUsageInAssetDatabase(Object asset, AssetDatabaseSearchHandler handler, out List<Object> objectsUsingAsset)
        {
            handler.CheckAssetUsageAsComponentInGameObjectsCollection(asset, handler.FindAllGameObjectsInProject(), out objectsUsingAsset);
        }

        public override void CheckAssetUsageInScene(Object asset, SceneSearchHandler handler, out List<Object> objectsUsingAsset)
        {
            objectsUsingAsset = new List<Object>();
            handler.GetCurrentActiveScene();

            if (!handler.TryGetAllScenePaths(out string[] scenesPath))
            {
                Debug.LogError("Could not get scenePaths");
                return;
            }

            foreach (var path in scenesPath)
            {
                if (!handler.TryGetGameObjectsFromScene(path, out GameObject[] sceneGOs)) { continue; }
                if (handler.CheckAssetUsageAsComponentInGameObjectsCollection(asset, sceneGOs, out List<Object> objectsUsingAssetInScene))
                {
                    handler.AddSceneToObjectUsingAssetList(objectsUsingAsset, path);
                    //Handle gameObjects path if scene is not active
                    handler.CanAddSceneGameObjectsToObjectsUsingAssetList(objectsUsingAsset, objectsUsingAssetInScene);
                }
                handler.HandleSceneClosure();
            }
        }
    }
}
//EOF.
#endif