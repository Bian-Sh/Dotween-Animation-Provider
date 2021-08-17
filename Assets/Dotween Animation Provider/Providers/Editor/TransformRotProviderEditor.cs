using DG.DOTweenEditor;
using DG.Tweening;
using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[CustomEditor(typeof(TransformRotProvider))]
class TransformRotProviderEditor : Editor
{
    TransformRotProvider provider;
    private void OnDisable()
    {
        EditorApplication.playModeStateChanged -= EditorApplication_playModeStateChanged;
        if (DOTweenEditorPreview.isPreviewing)
        {
            StopPreview();
        }
    }
    private void OnEnable()
    {
        EditorApplication.playModeStateChanged += EditorApplication_playModeStateChanged;
        provider = target as TransformRotProvider;
    }

    //在用户按下 Play 按钮时要还原 Dotween 动画预览
    private void EditorApplication_playModeStateChanged(PlayModeStateChange obj)
    {
        if (obj == PlayModeStateChange.ExitingEditMode && DOTweenEditorPreview.isPreviewing)
        {
            StopPreview();
        }
    }
    private void StopPreview()
    {
        provider.Stop();
        DOTweenEditorPreview.Stop(true);
        Undo.PerformUndo();
        Undo.ClearUndo(provider.transform);
    }
    public override void OnInspectorGUI()
    {
        bool isPlaying = provider.tweener?.IsPlaying() ?? false;
        bool isTweening = DOTweenEditorPreview.isPreviewing && isPlaying; serializedObject.Update();
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
        Undo.RegisterCompleteObjectUndo(provider, $"CaptureObjectStateForTweening{DateTime.Now.Millisecond}");
        DOTweenEditorPreview.PrepareTweenForPreview(provider.tweener);
        provider.tweener.OnComplete(StopPreview);
    }
}
