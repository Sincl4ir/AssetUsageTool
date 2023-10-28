#if UNITY_EDITOR

using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
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
            Debug.Log($"Selected asset path is: {_assetPath}");
            // Get the GUID of the selected asset (original prefab).
            _assetGUID = AssetDatabase.AssetPathToGUID(_assetPath);
            Debug.Log($"Selected asset ID is: {_assetGUID}");
            _objectsUsingAssetList = new();
        }
        #endregion

        #region IAssetUsageChecker

        public abstract bool CheckAssetUsage(out List<Object> objectsUsingAsset);
        public virtual void CheckAssetUsageInScene(Object asset) { }
        public virtual void CheckAssetUsageInAssetDatabase(Object asset) { }

        #endregion

        #region Common Utilities
        protected bool AreGUIDsEqual(string a, string b) => a.Equals(b);

        protected bool TryGetGameObjectsFromScene(string scenePath, out GameObject[] sceneGameObjects)
        {
            sceneGameObjects = null;

            if (string.IsNullOrEmpty(scenePath) || !System.IO.File.Exists(scenePath))
            {
                Debug.LogWarning($"Invalid scene path: {scenePath}");
                return false;
            }
            
            var scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);

            if (scene == null)
            {
                Debug.LogWarning($"Invalid scene, scene path: {scenePath} could not be opened");
                return false;
            }
            // Find all GameObjects in the scene.
            sceneGameObjects = scene.GetRootGameObjects();
            return true;
        }

        protected bool TryGetAllScenePaths(out string[] scenePaths)
        {
            scenePaths = AssetDatabase.FindAssets("t:Scene")
                .Select(g => AssetDatabase.GUIDToAssetPath(g))
                .ToArray();

            return scenePaths.Length > 0;
        }

        //protected bool TryGetAllScenePaths(Object asset, out string[] scenePaths)
        //{
        //    scenePaths = null;
        //    string origGUID = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(asset));

        //    // Find all scenes in the project.
        //    string[] sceneGUIDs = AssetDatabase.FindAssets("t:Scene");

        //    // Early exit if no scenes are found.
        //    if (sceneGUIDs.Length <= 0) { return false; }

        //    // Create an array and populate it with scene paths.
        //    scenePaths = sceneGUIDs.Select(g => AssetDatabase.GUIDToAssetPath(g)).ToArray();

        //    return true;
        //}

        protected bool CheckAssetUsageInSceneGameObjects(Object asset, GameObject[] rootObjects)
        {
            foreach (GameObject go in rootObjects)
            {
                if (!IsAssetUsedAsComponent(asset, go, go.GetComponents<Component>())) { continue;}
                return true;
            }

            return false;
        }

        protected bool IsAssetUsedAsComponent(Object asset, GameObject rootObject, Component[] components)
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

                    Debug.Log($"Asset {asset.name} is used in {component.GetType()} on GameObject {rootObject.name}");
                    return true;
                }
            }

            return false;
        }
        //protected bool IsAssetUsedAsComponent(Object asset, GameObject rootObject, Component[] components)
        //{
        //    bool used = false;
        //    foreach (Component component in components)
        //    {
        //        Debug.Log($"Checking component: {component}");
        //        if (component == null) { continue; }
        //        SerializedObject so = new SerializedObject(component);
        //        SerializedProperty sp = so.GetIterator();
        //        while (sp.NextVisible(true))
        //        {
        //            if (sp.propertyType == SerializedPropertyType.ObjectReference)
        //            {
        //                // Check if the property references the asset.
        //                if (sp.objectReferenceValue == asset)
        //                {
        //                    Debug.Log($"Asset {asset.name} is used in {component.GetType()} on GameObject {rootObject.name}");
        //                    used = true;
        //                }
        //            }
        //        }
        //    }

        //    return used;
        //}
        #endregion
    }
}
//EOF.
#endif