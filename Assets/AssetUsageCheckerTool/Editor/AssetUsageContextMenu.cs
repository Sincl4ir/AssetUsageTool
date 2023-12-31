#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Pampero.Editor
{
    /// <summary>
    /// Provides a context menu item to check the usage of a selected asset within the project.
    /// </summary>
    public static class AssetUsageContextMenu
    {
        private const string CONTEXT_DISPLAY_TITTLE = "Assets/Check Asset Usage in the Project";

        [MenuItem(CONTEXT_DISPLAY_TITTLE)]
        private static void CheckAssetUsage()
        {
            Object selectedAsset = Selection.activeObject;
            if (selectedAsset == null)
            {
                Debug.LogWarning("No asset selected for usage check.");
                return;
            }
            AssetUsageEditorTool.HandleCheckAssetUsageContextRequest(selectedAsset);
        }
    }
}
//EOF.
#endif