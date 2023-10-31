#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Pampero.Editor
{
    public abstract class AssetUsageSearchHandler : IAssetUsageSearchHandler
    {
        protected ICustomSearcher _customSearchHandler;

        public AssetUsageSearchHandler()
        {
            _customSearchHandler = AssetUsageSearchHandler.GetCustomSearchHandler(this);
        }

        public static ICustomSearcher GetCustomSearchHandler(AssetUsageSearchHandler assetUsageSearchHandler)
        {
            switch (assetUsageSearchHandler)
            {
                case SceneSearchHandler:
                    return new SceneSearcher();
                case AssetDatabaseSearchHandler:
                    return new AssetDatabaseSearcher();
                default:
                    return null;
            }
        }

        #region Public
        public virtual bool HandleAssetUsageSearch(Object asset, ObjectUsageChecker objectUsageChecker, out List<Object> objectsUsingAsset)
        {
            PerformUsageCheckBasedOnCheckerType(asset, objectUsageChecker, out objectsUsingAsset);
            return objectsUsingAsset.Count > 0;
        }
        #endregion

        #region Protected
        protected abstract void PerformUsageCheckBasedOnCheckerType(Object asset, ObjectUsageChecker objectUsageChecker, out List<Object> objectsUsingAssetInScene);

        protected virtual GameObject[] FindAllGameObjectsInProject()
        {
            string[] guids = AssetDatabase.FindAssets("t:GameObject");
            GameObject[] gameObjects = new GameObject[guids.Length];

            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                gameObjects[i] = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            }

            return gameObjects;
        }

        protected virtual bool CheckAssetUsageAsComponentInGameObjectsCollection(Object asset, GameObject[] gameObjects, out List<Object> gameObjectsUsingAsset)
        {
            gameObjectsUsingAsset = new List<Object>();
            foreach (GameObject go in gameObjects)
            {
                if (!IsAssetUsedAsComponent(asset, go, go.GetComponents<Component>())) { continue; }
                gameObjectsUsingAsset.Add(go);
            }

            return gameObjectsUsingAsset.Count > 0;
        }

        protected virtual bool IsAssetUsedAsComponent(Object asset, GameObject rootObject, Component[] components)
        {
            foreach (Component component in components)
            {
                if (component == null) { continue; }

                SerializedObject so = new SerializedObject(component);
                SerializedProperty sp = so.GetIterator();

                while (sp.NextVisible(true))
                {
                    if (sp.propertyType != SerializedPropertyType.ObjectReference) { continue; }
                    if (sp.objectReferenceValue != asset) { continue; }

                    //Debug.Log($"Asset {asset.name} is used in {component.GetType()} on GameObject {rootObject.name}");
                    return true;
                }
            }

            return false;
        }
        #endregion
    }

    public enum AssetCheckType 
    {
        SceneCheck,
        AssetDatabaseCheck
    }
}
//EOF.
#endif