using DG.DOTweenEditor;
using DG.Tweening;
using System;
using UnityEditor;
using UnityEngine;
using zFramework.Extension;
using Object = UnityEngine.Object;

[CustomEditor(typeof(TransformPosProvider))]
class TransformPosProviderEditor : Editor
{
    TransformPosProvider provider;
    const string info = @"
存在多个 Provider ，请挂载 Manager 预览全部动画！
";
    private void OnDisable()
    {
        EditorApplication.playModeStateChanged -= EditorApplication_playModeStateChanged;
        if (DOTweenEditorPreview.isPreviewing)
        {
            StopPreview();
        }
        if (provider)
        {
            provider.gameObject.hideFlags = cached;
        }
    }
    private void OnEnable()
    {
        provider = target as TransformPosProvider;
        cached = provider.gameObject.hideFlags;
        EditorApplication.playModeStateChanged += EditorApplication_playModeStateChanged;
    }

    //在用户按下 Play 按钮时要还原 Dotween 动画预览
    private void EditorApplication_playModeStateChanged(PlayModeStateChange obj)
    {
        if (obj == PlayModeStateChange.ExitingEditMode && DOTweenEditorPreview.isPreviewing)
        {
            StopPreview();
        }
    }

    public override void OnInspectorGUI()
    {
        if (provider.GetComponents<IDoTweenAnimProviderBehaviours>().Length > 1 && !provider.GetComponent<DotweenAnimProviderManager>())
        {
            EditorGUILayout.HelpBox(info, MessageType.Info);
        }

        bool isPlaying = provider.tweener?.IsPlaying() ?? false;
        bool isTweening = DOTweenEditorPreview.isPreviewing && isPlaying;

        serializedObject.Update();
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
        provider.gameObject.hideFlags = HideFlags.NotEditable;
        DOTweenEditorPreview.Start();
        provider.Play();
        Undo.RegisterCompleteObjectUndo(provider, $"CaptureObjectStateForTweening{DateTime.Now.Millisecond}");
        DOTweenEditorPreview.PrepareTweenForPreview(provider.tweener);
        provider.tweener.OnComplete(StopPreview);
    }
    private void StopPreview()
    {
        provider.Stop();
        DOTweenEditorPreview.Stop(true);
        Undo.PerformUndo(); //问题还很多，目前一停具停，且会同时重置，此为异常
        Undo.ClearUndo(provider.transform);
        provider.gameObject.hideFlags = cached;
    }
    HideFlags cached;
}
