#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Pampero.Editor
{
    /// <summary>
    /// Base class for checking the usage of an asset in the project.
    /// </summary>
    public abstract class ObjectUsageChecker :  IAssetUsageChecker
    {
        protected Object _myAsset;
        protected string _assetGUID;
        protected string _assetPath;
        protected List<Object> _objectsUsingAssetList;

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the ObjectUsageChecker.
        /// </summary>
        /// <param name="myAsset">The asset to check usage for.</param>
        protected ObjectUsageChecker(Object myAsset)
        {
            _myAsset = myAsset;
            _assetPath = AssetDatabase.GetAssetPath(myAsset);
            //Debug.Log($"Selected asset path is: {_assetPath}");
            // Get the GUID of the selected asset (original prefab).
            _assetGUID = AssetDatabase.AssetPathToGUID(_assetPath);
            //Debug.Log($"Selected asset ID is: {_assetGUID}");
            _objectsUsingAssetList = new();
        }
        #endregion

        public abstract bool CheckAssetUsage(out List<Object> objectsUsingAsset);

        /// <summary>
        /// Handles asset usage search based on the specified search type and adds the results to the list of objects using the asset.
        /// </summary>
        /// <param name="asset">The asset to check usage for.</param>
        /// <param name="searchType">The type of asset usage search to perform.</param>
        protected void HandleAssetUsageSearch(Object asset, AssetCheckType searchType)
        {
            var myHandler = GetAssetUsageSearchHandler(searchType);
            myHandler.HandleAssetUsageSearch(asset, this, out var objectsUsingAsset);
            _objectsUsingAssetList.AddRange(objectsUsingAsset);
        }

        /// <summary>
        /// Gets the appropriate asset usage search handler based on the specified search type.
        /// </summary>
        /// <param name="searchType">The type of asset usage search to perform.</param>
        /// <returns>The asset usage search handler for the specified search type.</returns>
        protected IAssetUsageSearchHandler GetAssetUsageSearchHandler(AssetCheckType searchType)
        {
            AssetCheckerProvider.HandleUsageCheckSearchers(searchType, out var searchHandler);
            return searchHandler;
        }
    }
}
//EOF.
#endif