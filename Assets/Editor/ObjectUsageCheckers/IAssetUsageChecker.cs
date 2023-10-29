#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEngine;

namespace Pampero.Editor
{
    public interface IAssetUsageChecker
    {
        bool CheckAssetUsage(out List<Object> objectUsingAsset);
    }
}
//EOF.
#endif