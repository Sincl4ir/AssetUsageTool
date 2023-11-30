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
        protected override void PerformUsageCheckBasedOnCheckerType(Object asset, ObjectUsageChecker objectUsageChecker, out List<Object> objectsUsingAssets)
        {
            switch (objectUsageChecker.AssetType)
            {
                case AssetType.Monoscript:
                    CheckAssetUsageAsComponentInGameObjectsCollection(asset, FindAllGameObjectsInProject(), out objectsUsingAssets);
                    break;
                default:
                    objectsUsingAssets = new List<Object>();
                    Debug.LogWarning("There is no implementation for this ObjectUsageCheker yet");
                    break;
            }
        }
    }
}
//EOF.
#endif