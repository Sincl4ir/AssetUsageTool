#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEngine;

namespace Pampero.Editor
{
    public interface IAssetUsageSearchHandler
    {
        bool HandleAssetUsageSearch(Object asset, ObjectUsageChecker objectUsageChecker, out List<Object> objectsUsingAsset);
    }
}
//EOF.
#endif