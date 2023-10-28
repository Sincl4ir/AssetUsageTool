#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Pampero.Editor
{
    public class MonoScriptUsageChecker : ObjectUsageChecker
    {
        public MonoScriptUsageChecker(Object asset) : base(asset) { }

        public override bool CheckAssetUsage(out List<Object> objectsUsingAsset)
        {
            Debug.Log("Checking usage for Monoscript");
            CheckAssetUsageInScene(_myAsset);
            objectsUsingAsset = _objectsUsingAssetList;
            return _objectsUsingAssetList.Count > 0;
        }

        public override void CheckAssetUsageInScene(Object asset)
        {
            if (!TryGetAllScenePaths(out string[] scenesPath))
            {
                Debug.LogError("Could not get scenePaths");
                return;
            }

            foreach (var path in scenesPath)
            {
                if (!TryGetGameObjectsFromScene(path, out GameObject[] sceneGOs)) { continue; }
                if (!CheckAssetUsageInSceneGameObjects(_myAsset, sceneGOs)) { continue; }

                _objectsUsingAssetList.Add(AssetDatabase.LoadAssetAtPath<Object>(path));
            }
        }
    }
}
//EOF.
#endif