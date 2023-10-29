#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine;

namespace Pampero.Editor
{
    public class AssetUsageEditorTool : EditorWindow
    {
        private const string WINDOW_TITTLE = "Asset Usage Checker";
        private const string MAIN_BUTTON_TITTLE = "Check Usage";
        private const string REORDABLE_LIST_TITTLE = "Objects Using Asset in Scene";
        private const string EDITOR_WINDOW_PATH = "Custom/Asset Usage Checker";
        private const string SEARCH_BOX_TITTLE = "Select an asset to check usage:";
        private const string INACTIVE_SCENES_WARNING_MESSAGE = "Some objects are not loaded because the scene where they belong is not currently active.";

        private Object _selectedAsset;
        private Object _newSelectedAsset;
        private IAssetUsageChecker _assetUsageChecker;
        private List<Object> _objectsUsingAsset = new List<Object>(); 
        private ReorderableList _reorderableList; 
        private bool _showReorderableList = false;


        [MenuItem(EDITOR_WINDOW_PATH)]
        public static void ShowWindow()
        {
            //GetWindow<AssetUsageEditorTool>("Asset Usage Checker");
            var window = GetWindow<AssetUsageEditorTool>(WINDOW_TITTLE);
            window.Show();
        }

        public static void HandleCheckAssetUsageContextRequest(Object asset)
        {
            if (asset == null) { return; }

            var window = GetWindow<AssetUsageEditorTool>(WINDOW_TITTLE);
            window.Show();
            window._selectedAsset = asset;
            window.HandleCheckAssetUsageRequest(asset);
        }

        private void OnEnable()
        {
            InitializeReordableList();
            EditorSceneManager.sceneOpened -= OnSceneOpened;
            EditorSceneManager.sceneOpened += OnSceneOpened;
        }

        private void OnDisable()
        {
            EditorSceneManager.sceneOpened -= OnSceneOpened;
        }

        private void OnGUI()
        {
            HandleAssetSelection();
            HandleCheckUsageButtonPress();
            HandleReordableListRefresh();
            CheckInactiveScenesWarning();
        }

        private void OnSceneOpened(UnityEngine.SceneManagement.Scene scene, OpenSceneMode mode)
        {
            Debug.Log($"{scene.name} was opened with mode: {mode}");
            ReordableListClearUp();
        }

        private void ReordableListClearUp()
        {
            _showReorderableList = false; // Hide the list
            _objectsUsingAsset.Clear(); // Clear the list data
        }

        private void HandleAssetSelection()
        {
            GUILayout.Label(SEARCH_BOX_TITTLE);
            _newSelectedAsset = EditorGUILayout.ObjectField(_selectedAsset, typeof(Object), false);
            HandleAssetSelectionChecks();
        }

        private void HandleAssetSelectionChecks()
        {
            if (_newSelectedAsset == _selectedAsset) { return; }
            // Asset selection has changed, reset the list
            _selectedAsset = _newSelectedAsset;
            ReordableListClearUp();
        }

        private void HandleCheckUsageButtonPress()
        {
            if (!GUILayout.Button(MAIN_BUTTON_TITTLE)) { return; }
            if (_selectedAsset == null) { return; }

            HandleCheckAssetUsageRequest(_selectedAsset);
        }

        private void HandleCheckAssetUsageRequest(Object asset)
        {
            if (!AssetCheckerProvider.TryCreateChecker(asset, out _assetUsageChecker)) { return; }

            _showReorderableList = _assetUsageChecker.CheckAssetUsage(out _objectsUsingAsset);
            _reorderableList.list = _objectsUsingAsset;
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

        private void CheckInactiveScenesWarning()
        {
            foreach (var obj in _objectsUsingAsset)
            {
                if (obj is SceneAsset sceneAsset)
                {
                    string scenePath = AssetDatabase.GetAssetPath(sceneAsset);
                    var scene = EditorSceneManager.GetSceneByPath(scenePath);

                    if (!scene.IsValid() || !scene.isLoaded)
                    {
                        // Display a warning message
                        EditorGUILayout.HelpBox(INACTIVE_SCENES_WARNING_MESSAGE, MessageType.Warning);
                        break; // Display the warning only once
                    }
                }
            }
        }
    }
}
//EOF.
#endif