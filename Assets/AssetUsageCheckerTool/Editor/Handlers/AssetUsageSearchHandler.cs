#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Pampero.Editor
{
    /// <summary>
    /// An abstract base class for handling asset usage searches. Derived classes implement specific usage check logic.
    /// </summary>
    public abstract class AssetUsageSearchHandler : IAssetUsageSearchHandler
    {
        #region Public
        public virtual bool HandleAssetUsageSearch(Object asset, ObjectUsageChecker objectUsageChecker, out List<Object> objectsUsingAsset)
        {
            PerformUsageCheckBasedOnCheckerType(asset, objectUsageChecker, out objectsUsingAsset);
            return objectsUsingAsset.Count > 0;
        }
        #endregion

        #region Protected

        /// <summary>
        /// Performs the usage check based on the provided asset and ObjectUsageChecker.
        /// Derived classes should implement specific logic for usage checks.
        /// </summary>
        /// <param name="asset">The asset to check for usage.</param>
        /// <param name="objectUsageChecker">The checker for the asset usage.</param>
        /// <param name="objectsUsingAssetInScene">A list of objects that are using the specified asset.</param>
        protected abstract void PerformUsageCheckBasedOnCheckerType(Object asset, ObjectUsageChecker objectUsageChecker, out List<Object> objectsUsingAssetInScene);

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

        public bool CheckAssetUsageAsComponentInGameObjectsCollection(Object asset, GameObject[] gameObjects, out List<Object> gameObjectsUsingAsset)
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

        public bool CheckAssetUsageAsGameObjectInGameObjectsCollection(Object asset, GameObject[] gameObjects, out List<Object> gameObjectsUsingAsset)
        {
            gameObjectsUsingAsset = new List<Object>();
            foreach (GameObject go in gameObjects)
            {
                if (!IsAssetUsedAsGameObject(asset, go) && !IsAssetUsedAsNestedPrefab(asset, go)) { continue; }
                gameObjectsUsingAsset.Add(go);
            }

            return gameObjectsUsingAsset.Count > 0;
        }

        protected virtual bool IsAssetUsedAsGameObject(Object asset, GameObject gameObject)
        {
            if (gameObject == asset) { return true; }
            if (!PrefabUtility.IsPartOfPrefabInstance(gameObject)) { return false; }

            GameObject prefabRoot = PrefabUtility.GetCorrespondingObjectFromSource(gameObject) as GameObject;
            if (prefabRoot == null) { return false; }

            if (!AssetDatabase.TryGetGUIDAndLocalFileIdentifier(prefabRoot, out string prefabGUID, out long _)) { return false; }

            AssetDatabase.TryGetGUIDAndLocalFileIdentifier(asset, out string assetGUID, out long _);
            return prefabGUID == assetGUID;
        }

        protected virtual bool IsAssetUsedAsNestedPrefab(Object asset, GameObject gameObject)
        {
            foreach (Transform childTransform in gameObject.transform)
            {
                if (!PrefabUtility.IsPartOfPrefabInstance(childTransform.gameObject)) { continue; }

                GameObject prefabRoot = PrefabUtility.GetCorrespondingObjectFromSource(childTransform.gameObject) as GameObject;
                if (prefabRoot == null) { return false; }
                
                if (IsAssetUsedAsGameObject(asset, prefabRoot)) { return true; }
                if (IsAssetUsedAsNestedPrefab(asset, childTransform.gameObject)) { return true; }
            }

            return false;
        }
        //private void CheckPrefabOriginalGUID(GameObject go)
        //{
        //    PrefabInstanceStatus prefabStatus = PrefabUtility.GetPrefabInstanceStatus(go);

        //    if (prefabStatus == PrefabInstanceStatus.Connected || prefabStatus == PrefabInstanceStatus.Disconnected)
        //    {
        //        GameObject sourcePrefab = PrefabUtility.GetCorrespondingObjectFromSource(go);
        //        string assetPath = AssetDatabase.GetAssetPath(sourcePrefab);
        //        // Get the GUID of the selected asset (original prefab).
        //        string prefabGUID = AssetDatabase.AssetPathToGUID(assetPath);
        //        //Debug.Log($"{go.name}: Asset Path = {assetPath}, Asset GUID = {prefabGUID}");
        //    }
        //}

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