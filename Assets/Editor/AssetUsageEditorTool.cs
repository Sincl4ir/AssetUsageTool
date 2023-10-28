#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Pampero.Editor
{
    public class AssetUsageEditorTool : EditorWindow
    {
        private const string WINDOW_TITTLE = "Asset Usage Checker";
        private const string MAIN_BUTTON_TITTLE = "Check Usage";
        private const string REORDABLE_LIST_TITTLE = "Objects Using Asset";

        private Object _selectedAsset;
        private Object _newSelectedAsset;
        private IAssetUsageChecker _assetUsageChecker;
        private List<Object> _objectsUsingAsset = new List<Object>(); // List to store objects using the asset
        private ReorderableList _reorderableList; // ReorderableList for displaying the objects
        private bool _showReorderableList = false;


        [MenuItem("Custom/Asset Usage Checker")]
        public static void ShowWindow()
        {
            //GetWindow<AssetUsageEditorTool>("Asset Usage Checker");
            var window = GetWindow<AssetUsageEditorTool>(WINDOW_TITTLE);
            window.Show();
        }

        private void OnEnable()
        {
            InitializeReordableList();
        }

        private void OnGUI()
        {
            HandleAssetSelection();
            HandleCheckUsageButtonPress();
            HandleReordableListRefresh();
        }

        private void HandleAssetSelection()
        {
            GUILayout.Label("Select an asset to check usage:");
            _newSelectedAsset = EditorGUILayout.ObjectField(_selectedAsset, typeof(Object), false);
            HandleAssetSelectionChecks();
        }

        private void HandleAssetSelectionChecks()
        {
            if (_newSelectedAsset == _selectedAsset) { return; }
            // Asset selection has changed, reset the list
            _selectedAsset = _newSelectedAsset;
            _showReorderableList = false;
            _objectsUsingAsset.Clear();
        }

        private void HandleCheckUsageButtonPress()
        {
            if (!GUILayout.Button(MAIN_BUTTON_TITTLE)) { return; }
            if (_selectedAsset == null) { return; }
            if (!AssetCheckerProvider.TryCreateChecker(_selectedAsset, out _assetUsageChecker)) { return; }

            _showReorderableList = _assetUsageChecker.CheckAssetUsage(out _objectsUsingAsset);
            _reorderableList.list = _objectsUsingAsset;

            foreach (var obj in _objectsUsingAsset)
            {
                Debug.Log($"Selected asset path is: {AssetDatabase.GetAssetPath(obj)}");
                // Get the GUID of the selected asset (original prefab).
                Debug.Log($"Selected asset ID is: {AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(obj))}");
            }
        }
        
        private void HandleReordableListRefresh()
        {
            if (!_showReorderableList) { return; }
            _reorderableList.DoLayoutList();
        }

        private void InitializeReordableList()
        {
            // Initialize the ReorderableList for displaying objects
            _reorderableList = new ReorderableList(_objectsUsingAsset, typeof(Object), true, true, false, false);
            _reorderableList.drawHeaderCallback = rect => EditorGUI.LabelField(rect, REORDABLE_LIST_TITTLE);
            _reorderableList.drawElementCallback = (rect, index, active, focused) =>
            {
                _objectsUsingAsset[index] = (Object)EditorGUI.ObjectField(rect, _objectsUsingAsset[index], typeof(Object), false);
            };
        }
    }
}
//EOF.
#endif