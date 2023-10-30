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
        private const string UNUSED_ASSET_MESSAGE = "Asset is not being used";

        private const int TOP_PADDING = 15;
        private const int LEFT_PADDING = 15;
        private const int RIGHT_PADDING = 15;
        private const int BOTTOM_PADDING = 15;
        private const int UI_ELEMENTS_SPACE = 10;
        private const float MAX_BUTTON_WIDTH = 120;

        private AssetUsageController _assetUsageController;
        private Object _selectedAsset;
        private Object _newSelectedAsset;
        private List<Object> _objectsUsingAsset = new List<Object>(); 
        private ReorderableList _reorderableList; 
        private bool _showReorderableList = false;
        private bool _activeUsageCheck = false;
        private bool _assetUsed = true;

        [MenuItem(EDITOR_WINDOW_PATH)]
        public static void ShowWindow()
        {
            var window = GetWindow<AssetUsageEditorTool>(WINDOW_TITTLE);
            window.Show();
        }

        public static void HandleCheckAssetUsageContextRequest(Object asset)
        {
            if (asset == null) { return; }

            var window = GetWindow<AssetUsageEditorTool>(WINDOW_TITTLE);
            window.Show();
            window._selectedAsset = asset;
            window.HandleAssetUsageRequest(asset);
        }

        private void OnEnable()
        {
            InitializeController();
            InitializeReordableList();
            EditorSceneManager.sceneOpened -= OnSceneOpened;
            EditorSceneManager.sceneOpened += OnSceneOpened;
        }

        private void OnDisable()
        {
            EditorSceneManager.sceneOpened -= OnSceneOpened;
        }

        #region OnGUI

        private void OnGUI()
        {
            HandleTopUIWindowConfiguration();
            HandleSearchUISettings();
            HandleSearchResultsUISettings();
            HandleBottomUIWindowConfiguration();
        }

        private static void HandleTopUIWindowConfiguration()
        {
            GUILayout.Space(TOP_PADDING);
            GUILayout.BeginHorizontal();
            GUILayout.Space(LEFT_PADDING);
            GUILayout.BeginVertical();
        }

        private void HandleSearchUISettings()
        {
            SetUpSearchTittle();
            GUILayout.BeginHorizontal();
            HandleAssetSelection();
            GUILayout.Space(UI_ELEMENTS_SPACE);
            HandleCheckUsageButtonPress();
            GUILayout.EndHorizontal();
        }

        private void HandleSearchResultsUISettings()
        {
            GUILayout.Space(UI_ELEMENTS_SPACE);
            HandleReordableListRefresh();
            GUILayout.Space(UI_ELEMENTS_SPACE);
            DisplayUnusedAssetMessageBox(_assetUsed);
            HandleInactiveScenesWarningChecks();
        }

        private static void HandleBottomUIWindowConfiguration()
        {
            GUILayout.EndVertical(); 
            GUILayout.Space(RIGHT_PADDING); 
            GUILayout.EndHorizontal();
            GUILayout.Space(BOTTOM_PADDING);
        }

        private static void SetUpSearchTittle()
        {
            GUILayout.Label(SEARCH_BOX_TITTLE);
            GUILayout.Space(UI_ELEMENTS_SPACE);
        }
        #endregion

        private void InitializeController()
        {
            _assetUsageController = new AssetUsageController(this);
        }

        private void InitializeReordableList()
        {
            _reorderableList = new ReorderableList(_objectsUsingAsset, typeof(Object), true, true, false, false);
            _reorderableList.drawHeaderCallback = rect => EditorGUI.LabelField(rect, REORDABLE_LIST_TITTLE);
            _reorderableList.drawElementCallback = (rect, index, active, focused) =>
            {
                _objectsUsingAsset[index] = (Object)EditorGUI.ObjectField(rect, _objectsUsingAsset[index], typeof(Object), false);
            };
        }

        private void OnSceneOpened(UnityEngine.SceneManagement.Scene scene, OpenSceneMode mode)
        {
            if (_activeUsageCheck) { return; }
            //Debug.Log($"{scene.name} was opened with mode: {mode}");
            ReordableListClearUp();
        }

        public void ReordableListClearUp()
        {
            _showReorderableList = false; 
            _objectsUsingAsset.Clear(); 
        }

        private void HandleAssetSelection()
        {
            _newSelectedAsset = EditorGUILayout.ObjectField(_selectedAsset, typeof(Object), false);
            HandleAssetSelectionChecks();
        }

        private void HandleAssetSelectionChecks()
        {
            if (_newSelectedAsset == _selectedAsset) { return; }

            _selectedAsset = _newSelectedAsset;
            ReordableListClearUp();
        }

        private void HandleCheckUsageButtonPress()
        {
            if (!GUILayout.Button(MAIN_BUTTON_TITTLE, GUILayout.MaxWidth(MAX_BUTTON_WIDTH), GUILayout.ExpandWidth(false))) { return; }
            if (_selectedAsset == null) { return; }

            _activeUsageCheck = true;
            HandleAssetUsageRequest(_selectedAsset);
            _activeUsageCheck = false;
        }

        private void HandleAssetUsageRequest(Object asset)
        {
            _assetUsed = _assetUsageController.HandleCheckAssetUsageRequest(asset, out _objectsUsingAsset);
            _showReorderableList = _assetUsed;
            _reorderableList.list = _objectsUsingAsset;
        }
        
        private void HandleReordableListRefresh()
        {
            if (!_showReorderableList) { return; }
            _reorderableList.DoLayoutList();
        }

        public void DisplayInactiveScenesWarning()
        {
            EditorGUILayout.HelpBox(INACTIVE_SCENES_WARNING_MESSAGE, MessageType.Warning);
        }

        public void DisplayUnusedAssetMessageBox(bool used)
        {
            if (used) { return; }
            EditorGUILayout.HelpBox(UNUSED_ASSET_MESSAGE, MessageType.Info);
        }

        private void HandleInactiveScenesWarningChecks()
        {
            _assetUsageController.HandleInactiveScenesWarning(_objectsUsingAsset);
        }
    }
}
//EOF.
#endif