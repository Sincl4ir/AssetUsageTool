#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Pampero.Editor
{
    public class GameObjectUsageChecker : ObjectUsageChecker
    {
        public GameObjectUsageChecker(Object asset) : base(asset) {}

        public override bool CheckAssetUsage(out List<Object> objectsUsingAsset)
        {
            Debug.Log("Checking usage for GameObject");
            objectsUsingAsset = _objectsUsingAssetList;
            return false;
        }

        private void CheckPrefabOriginalGUID(GameObject go)
        {
            PrefabInstanceStatus prefabStatus = PrefabUtility.GetPrefabInstanceStatus(go);

            if (prefabStatus == PrefabInstanceStatus.Connected || prefabStatus == PrefabInstanceStatus.Disconnected)
            {
                GameObject sourcePrefab = PrefabUtility.GetCorrespondingObjectFromSource(go);
                string assetPath = AssetDatabase.GetAssetPath(sourcePrefab);
                // Get the GUID of the selected asset (original prefab).
                string prefabGUID = AssetDatabase.AssetPathToGUID(assetPath);
                Debug.Log($"{go.name}: Asset Path = {assetPath}, Asset GUID = {prefabGUID}");
            }
        }
    }
}
//EOF.
#endif