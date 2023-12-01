#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Pampero.Editor
{
    /// <summary>
    /// Checks the usage of a GameObject Asset.
    /// </summary>
    public class GameObjectUsageChecker : ObjectUsageChecker
    {
        public GameObjectUsageChecker(Object asset) : base(asset, AssetType.GameObject) {}

        public override bool CheckAssetUsage(out List<Object> objectsUsingAsset)
        {
            //Debug.Log("Checking usage for GameObject");
            HandleAssetUsageSearch(_myAsset, AssetCheckType.SceneCheck);
            HandleAssetUsageSearch(_myAsset, AssetCheckType.AssetDatabaseCheck);

            objectsUsingAsset = _objectsUsingAssetList;
            return _objectsUsingAssetList.Count > 0;
        }

        
    }
}
//EOF.
#endif