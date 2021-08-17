using DG.DOTweenEditor;
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using zFramework.Extension;
using UnityEditor;
using System;

namespace zFramework.Editor.Extension
{
    public static class DotweenPreviewManager
    {
        static List<Tweener> tweeners = new List<Tweener>();
        [InitializeOnLoadMethod]
        static void ModelImporterAvatarSetup()
        {
            EditorApplication.playModeStateChanged -= EditorApplication_playModeStateChanged;
            EditorApplication.playModeStateChanged += EditorApplication_playModeStateChanged;
        }

        private static void EditorApplication_playModeStateChanged(PlayModeStateChange obj)
        {
            if (obj == PlayModeStateChange.ExitingEditMode && DOTweenEditorPreview.isPreviewing)
            {
                tweeners.Clear();
                DOTweenEditorPreview.Stop(true);
            }
        }
        #region 扩展方法
        public static void StopPreview(this IDoTweenAnimProviderBehaviours provider)
        {
            if (null == provider) return;
            TweenerPostProcess(provider);
        }
        public static void StartPreview(this IDoTweenAnimProviderBehaviours provider,TweenCallback OnUpdate=null)
        {
            if (null == provider) return;
            var tweener = provider.Play();

            if (!tweeners.Contains(tweener))
            {
                tweeners.Add(tweener);
            }
            if (!DOTweenEditorPreview.isPreviewing)
            {
                DOTweenEditorPreview.Start();
            }
            DOTweenEditorPreview.PrepareTweenForPreview(tweener);
            // 注册回调，务必在最后加监听
            tweener.OnComplete(() => TweenerPostProcess(provider));
            tweener.OnUpdate(OnUpdate);
        }

        public static void StopPreview(this DotweenAnimProviderManager manager)
        {
            if (null == manager) return;
            foreach (var provider in manager.Providers)
            {
                provider.StopPreview();
            }
        }
        public static void StartPreview(this DotweenAnimProviderManager manager)
        {
            if (null == manager) return;
            foreach (var provider in manager.Providers)
            {
                provider.StartPreview();
            }
        }

        public static bool IsPreviewing(this IDoTweenAnimProviderBehaviours provider)
        {
            return null != provider && null != provider.Tweener && provider.Tweener.IsPlaying() && tweeners.Contains(provider.Tweener);
        }
        public static bool IsPreviewing(this DotweenAnimProviderManager manager)
        {
            return manager.Providers.Any(v => v.IsPreviewing());
        }
        #endregion

        #region Assistance Funtion
        static void SetHideFlags(this IDoTweenAnimProviderBehaviours provider, HideFlags hideFlags)
        {
            ((Component)provider).gameObject.hideFlags = hideFlags;
        }
        static void Reset(this Tweener tween)
        {
            if (null == tween) return;
            try
            {
                if (IsFrom(tween))
                {
                    tween.Complete();
                }
                else
                {
                    tween.Rewind();
                }
            }
            catch { }
        }

        private static bool IsFrom(Tweener tween)
        {
            var info = tween.GetType()
                .GetField("isFrom", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(tween);
            return (bool)info;
        }

        private static void UpdateManagerState()
        {
            ValidateTween();
            if (tweeners.Count == 0)
            {
                DOTweenEditorPreview.Stop();
            }
        }

        private static void ValidateTween()
        {
            for (int num = tweeners.Count - 1; num > -1; num--)
            {
                if (tweeners[num] == null || !tweeners[num].active)
                {
                    tweeners.RemoveAt(num);
                }
            }
        }

        private static void TweenerPostProcess(IDoTweenAnimProviderBehaviours provider)
        {
            var tweener = provider.Tweener;
            if (tweeners.Contains(tweener))
            {
                provider.Stop();
                tweener.Reset(); //复位 Dotween 的修改

                tweeners.Remove(tweener);
                UpdateManagerState();
            }
        }

        #endregion
    }
}