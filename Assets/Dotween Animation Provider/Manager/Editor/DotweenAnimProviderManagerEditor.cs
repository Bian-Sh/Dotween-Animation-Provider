using DG.DOTweenEditor;
using UnityEditor;
using UnityEngine;
namespace zFramework.Extension.Tweening
{
    [CustomEditor(typeof(DotweenAnimProviderManager))]
    class DotweenAnimProviderManagerEditor : Editor
    {
        DotweenAnimProviderManager manager;
        private HideFlags cached;
        const string info = @"
用于驱动自身及其子节点的 Provider
";
        private void OnDisable()
        {
            EditorApplication.playModeStateChanged -= EditorApplication_playModeStateChanged;
            if (DOTweenEditorPreview.isPreviewing)
            {
                manager.StopPreview();
            }
        }
        private void OnEnable()
        {
            manager = target as DotweenAnimProviderManager;
            //manager.gameObject.hideFlags = HideFlags.None;
            EditorApplication.playModeStateChanged += EditorApplication_playModeStateChanged;
        }

        //在用户按下 Play 按钮时要还原 Dotween 动画预览
        private void EditorApplication_playModeStateChanged(PlayModeStateChange obj)
        {
            if (obj == PlayModeStateChange.ExitingEditMode && DOTweenEditorPreview.isPreviewing)
            {
                manager.StopPreview();
            }
        }

        public override void OnInspectorGUI()
        {
            GUI.enabled = !EditorApplication.isPlayingOrWillChangePlaymode && manager.gameObject.activeInHierarchy && (manager.hideFlags != HideFlags.NotEditable || manager.IsPreviewing());
            EditorStyles.helpBox.fontSize = 13;
            EditorGUILayout.HelpBox(info, MessageType.Info);
            bool isPreviewing = manager.IsPreviewing();
            if (GUILayout.Button(isPreviewing ? "停止预览" : "开始预览"))
            {
                if (isPreviewing)
                {
                    manager.StopPreview();
                }
                else
                {
                    manager.StartPreview();
                }
            }
        }

    }
}