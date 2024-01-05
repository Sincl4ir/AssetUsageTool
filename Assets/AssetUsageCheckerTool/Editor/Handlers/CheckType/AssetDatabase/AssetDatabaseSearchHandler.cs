#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;

namespace Pampero.Editor
{
    /// <summary>
    /// A class that handles asset usage searches specific to the Asset Database, extending the AssetUsageSearchHandler base class.
    /// </summary>
    public class AssetDatabaseSearchHandler : AssetUsageSearchHandler
    {
        protected override void PerformUsageCheckBasedOnCheckerType(Object asset, ObjectUsageChecker objectUsageChecker, out List<Object> objectsUsingAsset)
        {
            objectsUsingAsset = new();
            if (!AssetCheckerProvider.TryGetAssetSearcher(objectUsageChecker.AssetType, out BaseAssetSearcher searcher)) { return; }

            searcher.CheckAssetUsageInAssetDatabase(asset, this, out objectsUsingAsset);
        }
    }
}
//EOF.
#endif