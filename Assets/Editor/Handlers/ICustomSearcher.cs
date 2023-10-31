#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEngine;

namespace Pampero.Editor
{
    public interface ICustomSearcher
    {
        void PerformUsageCheckBasedOnCheckerType(Object asset, out List<Object> objectsUsingAssetInScene);
    }
}
//EOF.
#endif