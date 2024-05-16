#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pampero.Editor
{
    public interface IAssetTypeHandler
    {
        void GetObjectsUsingAsset(out List<Object> objectsUsingAsset);
    }

    public abstract class ObjectSearchHandler : IAssetTypeHandler
    {
        protected Object _asset;
        protected AssetUsageSearchHandler _assetUsageSearchHandler;

        protected ObjectSearchHandler(Object asset, AssetUsageSearchHandler assetUsageSearchHandler)
        {
            _asset = asset;
            _assetUsageSearchHandler = assetUsageSearchHandler;
        }

        public virtual void GetObjectsUsingAsset(out List<Object> objectsUsingAsset)
        {
            throw new System.NotImplementedException();
        }
    }
}
//EOF.
#endif