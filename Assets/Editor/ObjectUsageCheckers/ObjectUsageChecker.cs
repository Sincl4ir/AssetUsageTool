#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Pampero.Editor
{
    public abstract class ObjectUsageChecker :  IAssetUsageChecker
    {
        protected Object _myAsset;
        protected string _assetGUID;
        protected string _assetPath;
        protected List<Object> _objectsUsingAssetList;

        #region Constructor
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

        protected void HandleAssetUsageSearch(Object asset, AssetCheckType searchType)
        {
            var myHandler = GetAssetUsageSearchHandler(searchType);
            myHandler.HandleAssetUsageSearch(asset, this, out var objectsUsingAsset);
            _objectsUsingAssetList.AddRange(objectsUsingAsset);
        }

        protected IAssetUsageSearchHandler GetAssetUsageSearchHandler(AssetCheckType searchType)
        {
            AssetCheckerProvider.HandleUsageCheckSearchers(searchType, out var searchHandler);
            return searchHandler;
        }
    }
}
//EOF.
#endif