#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Pampero.Editor
{
    public enum AssetType
    {
        Unknown,
        Monoscript,
        GameObject
    }
    /// <summary>
    /// Base class for checking the usage of an asset in the project.
    /// </summary>
    public class ObjectUsageChecker : IAssetUsageChecker
    {
        protected Object _myAsset;
        protected List<Object> _objectsUsingAssetList;

        public AssetType AssetType { get; private set; }

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the ObjectUsageChecker.
        /// </summary>
        /// <param name="myAsset">The asset to check usage for.</param>
        public ObjectUsageChecker(Object myAsset, AssetType assetType)
        {
            _myAsset = myAsset;
            AssetType = assetType;
            //_assetPath = AssetDatabase.GetAssetPath(myAsset);
            //_assetGUID = AssetDatabase.AssetPathToGUID(_assetPath);
            _objectsUsingAssetList = new();
        }
        #endregion

        public virtual bool CheckAssetUsage(out List<Object> objectsUsingAsset)
        {
            HandleAssetUsageSearch(_myAsset, AssetCheckType.SceneCheck);
            HandleAssetUsageSearch(_myAsset, AssetCheckType.AssetDatabaseCheck);

            objectsUsingAsset = _objectsUsingAssetList;
            return _objectsUsingAssetList.Count > 0;
        }

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
            AssetCheckerProvider.GetProperAssetSearchHandler(searchType, out var searchHandler);
            return searchHandler;
        }
    }
}
//EOF.
#endif