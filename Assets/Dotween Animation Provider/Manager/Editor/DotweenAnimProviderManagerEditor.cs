using DG.DOTweenEditor;
using DG.Tweening;
using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DotweenAnimProviderManager))]
class DotweenAnimProviderManagerEditor : Editor
{
    DotweenAnimProviderManager manager;
    private HideFlags cached;
    const string info = @"
1. 仅对子节点下的 Provider 有效。
2. 停止预览将恢复到预览前状态。
3. Manager 转变为 Disable 将停止所有预览。
4. Play 时将停止所有预览。
5. 点击他处导致的管理器复位，会定位到管理器一次，此为正常。
6. 开始/停止预览仅代表预览功能开启状态，不代表动画运行状态。
";
    private void OnDisable()
    {
        EditorApplication.playModeStateChanged -= EditorApplication_playModeStateChanged;
        if (DOTweenEditorPreview.isPreviewing)
        {
            StopPreview();
        }
        Undo.ClearUndo(manager);
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
            StopPreview();
            Undo.ClearUndo(manager);
        }
    }
    private void StopPreview()
    {
        manager.Stop();
        DOTweenEditorPreview.Stop(true);
        if (Undo.GetCurrentGroupName() == key)
        {
            Undo.PerformUndo();
        }
        manager.gameObject.hideFlags = cached;
    }
    public override void OnInspectorGUI()
    {
        GUI.enabled = !Application.isPlaying && manager.gameObject.activeInHierarchy && (manager.hideFlags != HideFlags.NotEditable || manager.IsTweening);
        EditorStyles.helpBox.fontSize = 13;
        EditorGUILayout.HelpBox(info, MessageType.Info);
        bool isPreviewing = DOTweenEditorPreview.isPreviewing;
        if (GUILayout.Button(isPreviewing ? "停止预览" : "开始预览"))
        {
            if (isPreviewing)
            {
                StopPreview();
            }
            else
            {
                StarPreview();
            }
        }
    }
    void StarPreview()
    {
        var ts = manager.Tweeners;
        if (ts.Count > 0)
        {
            cached = manager.gameObject.hideFlags;
            manager.gameObject.hideFlags = HideFlags.NotEditable;
            DOTweenEditorPreview.Start();
            manager.Play();
            key = $"CaptureObjectStateForTweening{DateTime.Now.Millisecond}";
            Undo.RegisterFullObjectHierarchyUndo(manager, key);
            foreach (var item in ts)
            {
                DOTweenEditorPreview.PrepareTweenForPreview(item.Tweener);
            }
        }
        else
        {
            Debug.LogError($"{nameof(DotweenAnimProviderManager)}: 动画预览失败，子节点下未发现 Provider！");
        }
    }
    string key = string.Empty;
}
