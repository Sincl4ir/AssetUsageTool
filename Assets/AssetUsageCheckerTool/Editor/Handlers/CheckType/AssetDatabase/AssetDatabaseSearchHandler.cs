#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Pampero.Editor
{
    /// <summary>
    /// A class that handles asset usage searches specific to the Asset Database, extending the AssetUsageSearchHandler base class.
    /// </summary>
    public class AssetDatabaseSearchHandler : AssetUsageSearchHandler
    {
        protected override void PerformUsageCheckBasedOnCheckerType(Object asset, ObjectUsageChecker objectUsageChecker, out List<Object> objectsUsingAsset)
        {
            objectsUsingAsset = new();
            if (!AssetCheckerProvider.TryGetAssetSearcher(objectUsageChecker.AssetType, out BaseAssetSearcher searcher)) { return; }

            searcher.CheckAssetUsageInAssetDatabase(asset, this, out objectsUsingAsset);
        }

        public GameObject[] FindAllPrefabsFromAssetDataBase(Object prefab)
        {
            string[] guids = AssetDatabase.FindAssets($"t:Prefab");
            List<GameObject> prefabInstances = new List<GameObject>();

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);

                GameObject prefabInstance = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                //Do not load to list if the asset is the same one we are looking for.
                if (prefab == prefabInstance || prefabInstance == null) { continue; }
                prefabInstances.Add(prefabInstance);
            }

            return prefabInstances.ToArray();
        }

        public bool CheckAssetUsageAsGameObjectInPrefabInstance(Object asset, GameObject prefabInstance, out List<Object> prefabObjectsUsingAsset)
        {
            prefabObjectsUsingAsset = new List<Object>();


            if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(prefabInstance, out string prefabRootGUID, out long _))
            {
                AssetDatabase.TryGetGUIDAndLocalFileIdentifier(asset, out string assetGUID, out long _);

                if (prefabRootGUID == assetGUID)
                {
                    prefabObjectsUsingAsset.Add(prefabInstance);
                    return true;
                }
            }

            return false;
        }

        public Material[] FindAllMaterialsUsingAsset(Object asset)
        {
            // Example: Find all materials in the project
            string[] guids = AssetDatabase.FindAssets($"t:Material");
            List<Material> materials = new List<Material>();

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                Material material = AssetDatabase.LoadAssetAtPath<Material>(path);

                // Example: Check if the asset is used as a shader in the material
                if (IsAssetUsedAsShader(asset, material))
                {
                    materials.Add(material);
                }
            }

            return materials.ToArray();
        }

        // Add more helper methods for other asset types as needed...

        private bool IsAssetUsedAsShader(Object asset, Material material)
        {
            // Example: Check if the asset is used as a shader in the material
            Shader shader = material.shader;
            return shader == asset;
        }

    }
}
//EOF.
#endif