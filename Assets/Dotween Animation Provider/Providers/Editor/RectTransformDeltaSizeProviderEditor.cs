using DG.DOTweenEditor;
using DG.Tweening;
using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[CustomEditor(typeof(RectTransformDeltaSizeProvider))]
class RectTransformDeltaSizeProviderEditor : Editor
{
    RectTransformDeltaSizeProvider provider;
    private void OnDisable()
    {
        EditorApplication.playModeStateChanged -= EditorApplication_playModeStateChanged;
        if (DOTweenEditorPreview.isPreviewing)
        {
            StopPreview();
        }
        Undo.ClearUndo(provider);
    }

    private void OnEnable()
    {
        EditorApplication.playModeStateChanged += EditorApplication_playModeStateChanged;
        provider = target as RectTransformDeltaSizeProvider;
    }

    //在用户按下 Play 按钮时要还原 Dotween 动画预览
    private void EditorApplication_playModeStateChanged(PlayModeStateChange obj)
    {
        if (obj == PlayModeStateChange.ExitingEditMode && DOTweenEditorPreview.isPreviewing)
        {
            StopPreview();
            Undo.ClearUndo(provider);
        }
    }


    public override void OnInspectorGUI()
    {
        bool isPlaying = provider.tweener?.IsPlaying() ?? false;
        bool isTweening = DOTweenEditorPreview.isPreviewing && isPlaying;
        serializedObject.Update();
        GUI.enabled = !isTweening;
        var itr = serializedObject.GetIterator();
        itr.NextVisible(true);
        while (itr.NextVisible(false))
        {
            if (itr.name != "loopType" || provider.loopcount != 0 && provider.loopcount != 1)
            {
                EditorGUILayout.PropertyField(itr, true);
            }
        }
        if (serializedObject.hasModifiedProperties)
        {
            serializedObject.ApplyModifiedProperties();
        }
        GUI.enabled = !Application.isPlaying && provider.gameObject.activeInHierarchy && (provider.hideFlags != HideFlags.NotEditable || isPlaying);
        if (GUILayout.Button(isTweening ? "停止预览" : "开始预览"))
        {
            if (isTweening)
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
        DOTweenEditorPreview.Start();
        provider.Play();
        Undo.GetCurrentGroupName();
        key = $"CaptureObjectStateForTweening{DateTime.Now.Millisecond}";
        Undo.RegisterCompleteObjectUndo(provider, key);
        DOTweenEditorPreview.PrepareTweenForPreview(provider.tweener);
        provider.tweener.OnComplete(StopPreview);
    }
    private void StopPreview()
    {
        provider.Stop();
        DOTweenEditorPreview.Stop(true);
        if (key == Undo.GetCurrentGroupName())
        {
            Undo.PerformUndo();
        }
    }
    string key = string.Empty;
}
