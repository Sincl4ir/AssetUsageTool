#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEngine;

namespace Pampero.Editor
{
    /// <summary>
    /// An interface for handling asset usage searches.
    /// </summary>
    public interface IAssetUsageSearchHandler
    {
        /// <summary>
        /// Handles the search for objects using a specific asset based on the provided asset and ObjectUsageChecker.
        /// </summary>
        /// <param name="asset">The asset to check for usage.</param>
        /// <param name="objectUsageChecker">The checker for the asset usage.</param>
        /// <param name="objectsUsingAsset">A list of objects that are using the specified asset.</param>
        /// <returns>True if objects using the asset were found; otherwise, false.</returns>
        bool HandleAssetUsageSearch(Object asset, ObjectUsageChecker objectUsageChecker, out List<Object> objectsUsingAsset);
    }
}
//EOF.
#endif