#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Pampero.Editor
{
    /// <summary>
    /// Checks the usage of a GameObject Asset.
    /// </summary>
    public class GameObjectSearcher : BaseAssetSearcher
    {
        public override void CheckAssetUsageInAssetDatabase(Object asset, AssetDatabaseSearchHandler handler, out List<Object> objectsUsingAsset)
        {
            objectsUsingAsset = new List<Object>();

            // Example: Check usage in prefabs
            GameObject[] prefabInstances = handler.FindAllPrefabsFromAssetDataBase(asset);
            Debug.Log($"prefanInstances size: " + prefabInstances.Length);
            handler.CheckAssetUsageAsGameObjectInGameObjectsCollection(asset, prefabInstances, out objectsUsingAsset);
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
                if (handler.CheckAssetUsageAsGameObjectInGameObjectsCollection(asset, sceneGOs, out List<Object> objectsUsingAssetInScene))
                {
                    handler.AddSceneToObjectUsingAssetList(objectsUsingAsset, path);
                    // Handle gameObjects path if the scene is not active
                    handler.CanAddSceneGameObjectsToObjectsUsingAssetList(objectsUsingAsset, objectsUsingAssetInScene);
                }
                handler.HandleSceneClosure();
            }
        }
    }
}
//EOF.
#endif