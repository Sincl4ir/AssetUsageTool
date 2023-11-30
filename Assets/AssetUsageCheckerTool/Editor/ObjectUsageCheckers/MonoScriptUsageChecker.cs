#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;

namespace Pampero.Editor
{
    /// <summary>
    /// Checks the usage of a Monoscript asset.
    /// </summary>
    public class MonoScriptUsageChecker : ObjectUsageChecker
    {
        public MonoScriptUsageChecker(Object asset) : base(asset, AssetType.Monoscript) { }

        public override bool CheckAssetUsage(out List<Object> objectsUsingAsset)
        {
            //Debug.Log("Checking usage for Monoscript");
            HandleAssetUsageSearch(_myAsset, AssetCheckType.SceneCheck);
            HandleAssetUsageSearch(_myAsset, AssetCheckType.AssetDatabaseCheck);

            objectsUsingAsset = _objectsUsingAssetList;
            return _objectsUsingAssetList.Count > 0;
        }
    }
}
//EOF.
#endif