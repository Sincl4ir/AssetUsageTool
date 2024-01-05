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
            throw new System.NotImplementedException();
        }

        public override void CheckAssetUsageInScene(Object asset, SceneSearchHandler handler, out List<Object> objectsUsingAsset)
        {
            throw new System.NotImplementedException();
        }


        //private void CheckPrefabOriginalGUID(GameObject go)
        //{
        //    PrefabInstanceStatus prefabStatus = PrefabUtility.GetPrefabInstanceStatus(go);

        //    if (prefabStatus == PrefabInstanceStatus.Connected || prefabStatus == PrefabInstanceStatus.Disconnected)
        //    {
        //        GameObject sourcePrefab = PrefabUtility.GetCorrespondingObjectFromSource(go);
        //        string assetPath = AssetDatabase.GetAssetPath(sourcePrefab);
        //        // Get the GUID of the selected asset (original prefab).
        //        string prefabGUID = AssetDatabase.AssetPathToGUID(assetPath);
        //        //Debug.Log($"{go.name}: Asset Path = {assetPath}, Asset GUID = {prefabGUID}");
        //    }

    }
}
//EOF.
#endif