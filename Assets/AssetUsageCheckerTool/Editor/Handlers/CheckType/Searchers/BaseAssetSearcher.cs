using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pampero.Editor
{
    public abstract class BaseAssetSearcher : ISceneSearchable, IAssetDatabaseSearchable
    {
        public virtual void CheckAssetUsageInAssetDatabase(Object asset, AssetDatabaseSearchHandler handler, out List<Object> objectsUsingAsset)
        {
            throw new System.NotImplementedException();
        }

        public virtual void CheckAssetUsageInScene(Object asset, SceneSearchHandler handler, out List<Object> objectsUsingAsset)
        {
            throw new System.NotImplementedException();
        }
    }

    public interface ISceneSearchable
    {
        public void CheckAssetUsageInScene(Object asset, SceneSearchHandler handler, out List<Object> objectsUsingAsset);
    }

    public interface IAssetDatabaseSearchable
    {
        public void CheckAssetUsageInAssetDatabase(Object asset, AssetDatabaseSearchHandler handler, out List<Object> objectsUsingAsset);
    }
}
//EOF.