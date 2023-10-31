#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEngine;

namespace Pampero.Editor
{
    /// <summary>
    /// Interface for checking asset usage.
    /// </summary>
    public interface IAssetUsageChecker
    {
        /// <summary>
        /// Checks the usage of the asset.
        /// </summary>
        /// <param name="objectUsingAsset">List of objects using the asset.</param>
        /// <returns>True if the asset is in use; false otherwise.</returns>
        bool CheckAssetUsage(out List<Object> objectUsingAsset);
    }
}
//EOF.
#endif