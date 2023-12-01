#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Pampero.Editor
{
    /// <summary>
    /// An abstract base class for handling asset usage searches. Derived classes implement specific usage check logic.
    /// </summary>
    public abstract class AssetUsageSearchHandler : IAssetUsageSearchHandler
    {
        #region Protected

        /// <summary>
        /// Performs the usage check based on the provided asset and ObjectUsageChecker.
        /// Derived classes should implement specific logic for usage checks.
        /// </summary>
        /// <param name="asset">The asset to check for usage.</param>
        /// <param name="objectUsageChecker">The checker for the asset usage.</param>
        /// <param name="objectsUsingAssetInScene">A list of objects that are using the specified asset.</param>
        protected virtual void PerformUsageCheckBasedOnAssetType(Object asset, ObjectUsageChecker objectUsageChecker, out List<Object> objectsUsingAsset)
        {
            objectsUsingAsset = new List<Object>();
            if (!TryGetAssetTypeHandler(asset, objectUsageChecker.AssetType, out var assetTypeHandler)) { return; }

            assetTypeHandler.GetObjectsUsingAsset(out objectsUsingAsset);
        }

        protected bool TryGetAssetTypeHandler(Object asset, AssetType assetType, out IAssetTypeHandler assetTypeHandler)
        {
            assetTypeHandler = null;

            switch (assetType)
            {
                case AssetType.Monoscript:
                    assetTypeHandler = new MonoscriptSearchHandler(asset, this);
                    return true;
                //monoscriptHandler.GetObjectsUsingAsset(out objectsUsingAssetInScene);
                //CheckMonoScriptUsageInScene(asset, out objectsUsingAssetInScene);
                //case AssetType.GameObject:
                //return new GameObjectSearchHandler(asset, this
                default:
                    return false;
            }
        }

        #endregion

        #region Public
        public virtual bool HandleAssetUsageSearch(Object asset, ObjectUsageChecker objectUsageChecker, out List<Object> objectsUsingAsset)
        {
            PerformUsageCheckBasedOnAssetType(asset, objectUsageChecker, out objectsUsingAsset);
            return objectsUsingAsset.Count > 0;
        }

        /// <summary>
        /// Finds all GameObjects in the project and returns an array of them.
        /// </summary>
        /// <returns>An array of all GameObjects in the project.</returns>
        public virtual GameObject[] FindAllGameObjectsInProject()
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

        /// <summary>
        /// Checks if the specified asset is used as a component in a collection of GameObjects.
        /// </summary>
        /// <param name="asset">The asset to check for usage as a component.</param>
        /// <param name="gameObjects">An array of GameObjects to check for asset usage.</param>
        /// <param name="gameObjectsUsingAsset">A list of GameObjects that are using the specified asset.</param>
        /// <returns>True if the asset is used in any of the provided GameObjects; otherwise, false.</returns>
        public virtual bool CheckAssetUsageAsComponentInGameObjectsCollection(Object asset, GameObject[] gameObjects, out List<Object> gameObjectsUsingAsset)
        {
            gameObjectsUsingAsset = new List<Object>();
            foreach (GameObject go in gameObjects)
            {
                if (!IsAssetUsedAsComponent(asset, go, go.GetComponents<Component>())) { continue; }
                gameObjectsUsingAsset.Add(go);
            }

            return gameObjectsUsingAsset.Count > 0;
        }

        /// <summary>
        /// Checks if the specified asset is used as a component in a GameObject.
        /// </summary>
        /// <param name="asset">The asset to check for usage as a component.</param>
        /// <param name="rootObject">The GameObject to check for asset usage.</param>
        /// <param name="components">An array of components in the GameObject to check.</param>
        /// <returns>True if the asset is used as a component in the GameObject; otherwise, false.</returns>
        public virtual bool IsAssetUsedAsComponent(Object asset, GameObject rootObject, Component[] components)
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