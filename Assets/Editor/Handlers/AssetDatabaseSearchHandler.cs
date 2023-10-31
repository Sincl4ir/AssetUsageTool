#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;

namespace Pampero.Editor
{
    public class AssetDatabaseSearchHandler : AssetUsageSearchHandler
    {
        public AssetDatabaseSearchHandler() : base() { }

        protected override void PerformUsageCheckBasedOnCheckerType(Object asset, ObjectUsageChecker objectUsageChecker, out List<Object> objectsUsingAssets)
        {
            switch (objectUsageChecker)
            {
                case MonoScriptUsageChecker:
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