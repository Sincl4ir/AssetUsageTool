#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace Pampero.Editor
{
    /// <summary>
    /// A class that handles asset usage searches specific to the Asset Database, extending the AssetUsageSearchHandler base class.
    /// </summary>
    public class AssetDatabaseSearchHandler : AssetUsageSearchHandler
    {
        public void CheckPrefabOriginalGUID(GameObject go)
        {
            PrefabInstanceStatus prefabStatus = PrefabUtility.GetPrefabInstanceStatus(go);

            if (prefabStatus == PrefabInstanceStatus.Connected || prefabStatus == PrefabInstanceStatus.Disconnected)
            {
                GameObject sourcePrefab = PrefabUtility.GetCorrespondingObjectFromSource(go);
                string assetPath = AssetDatabase.GetAssetPath(sourcePrefab);
                // Get the GUID of the selected asset (original prefab).
                string prefabGUID = AssetDatabase.AssetPathToGUID(assetPath);
                //Debug.Log($"{go.name}: Asset Path = {assetPath}, Asset GUID = {prefabGUID}");
            }
        }
    }
}
//EOF.
#endif