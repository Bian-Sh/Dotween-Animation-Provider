using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
namespace zFramework.Extension.Tweening
{
    [CustomEditor(typeof(DoTweenBaseProvider), true)]
    public class DoTweenBaseProviderEditor : Editor
    {
        public DoTweenBaseProvider provider;
        bool fd0 = true;
        bool fd1 = true;
        GUIStyle bt;
        GUIStyle fds;
        SerializedProperty loopcount;
        public override void OnInspectorGUI()
        {
            #region Init GUIStyle
            if (null == bt) bt = new GUIStyle(EditorStyles.miniButtonRight) { fixedWidth = 100, richText = true };
            if (null == fds) fds = new GUIStyle(EditorStyles.foldoutHeader) { fontStyle = FontStyle.Normal, fontSize = 14 };
            fds.onNormal.textColor = Color.white;
            #endregion

            GUI.enabled = EditorApplication.isPlaying || !provider.IsPreviewing();
            serializedObject.Update();
            #region 数据校验
            loopcount = loopcount ?? serializedObject.FindProperty("loopcount");
            loopcount.intValue = loopcount.intValue < -1 ? -1 : loopcount.intValue;
            #endregion

            var itr = serializedObject.GetIterator();
            itr.NextVisible(true);
            fd0 = EditorGUILayout.Foldout(fd0, " 通用参数 ", fds);
            if (fd0)
            {
                HorizontalLine();
                while (itr.NextVisible(false))
                {
                    if (itr.name != "loopType" || provider.loopcount != 0 && provider.loopcount != 1)
                    {
                        EditorGUILayout.PropertyField(itr, true);
                    }
                    if (itr.name == "ease") break;
                }
            }
            fd1 = EditorGUILayout.Foldout(fd1, " Provider 参数 ", fds);
            if (fd1)
            {
                HorizontalLine();
                while (itr.NextVisible(false))
                {
                    if (fd0 || IsPropertyDrawRequired(itr))
                    {
                        EditorGUILayout.PropertyField(itr, true);
                    }
                }
            }
            if (serializedObject.hasModifiedProperties)
            {
                serializedObject.ApplyModifiedProperties();
            }
            GUI.enabled = !EditorApplication.isPlayingOrWillChangePlaymode && provider.gameObject.activeInHierarchy;
            bool isTweening = !EditorApplication.isPlayingOrWillChangePlaymode && provider.IsPreviewing();
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(isTweening ? "停止预览" : "开始预览"))
            {
                if (isTweening)
                {
                    provider.StopPreview();
                }
                else
                {
                    provider.StartPreview(OnTweenerStart,OnTweenerUpdating);
                }
            }

            var providers = provider.GetComponents<DoTweenBaseProvider>();
            if (null != providers && providers.Length > 1)
            {
                var anyispreviewing = providers.Any(v => v.IsPreviewing());
                var label = anyispreviewing ? "<color=red>全部停止</color>" : "全部预览";
                if (GUILayout.Button(label, bt))
                {
                    if (anyispreviewing)
                    {
                        foreach (var item in providers)
                        {
                            item.StopPreview();
                        }
                    }
                    else
                    {
                        foreach (var item in providers)
                        {
                            item.StartPreview(()=>OnTweenerStart(providers),() => OnTweenerUpdating(item));
                        }
                    }
                }
            }
            GUILayout.EndHorizontal();
        }

        bool isTweenerTargetNeedSetDirty = false;
        public virtual void OnTweenerStart(DoTweenBaseProvider[] providers)
        {
            isTweenerTargetNeedSetDirty = providers.Any(v => DotweenProviderSettings.VerifyTargetNeedSetDirty(v.target));
        }

        public virtual void OnTweenerStart()
        {
            isTweenerTargetNeedSetDirty = DotweenProviderSettings.VerifyTargetNeedSetDirty(provider.target);
        }

        public virtual void OnTweenerUpdating()
        {
            //想要预览 TextMeshPro UGUI 、Text 就必须刷新
            // 其他类型，请在 DotweenProviderSettings 中设置
            if (isTweenerTargetNeedSetDirty)
            {
                EditorUtility.SetDirty(provider.target);
            }
        }
        public virtual void OnTweenerUpdating(DoTweenBaseProvider provider)
        {
            if (isTweenerTargetNeedSetDirty)
            {
                EditorUtility.SetDirty(provider.target);
            }
        }
        public virtual void OnDisable() => provider.StopPreview();
        public virtual void OnEnable()=>provider = target as DoTweenBaseProvider;

        #region 辅助绘制方法
        static List<string> fields;
        [InitializeOnLoadMethod]
        static void LoadFieldsNameByRef()
        {
            fields = typeof(DoTweenBaseProvider)
                .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly)
                .Select(v => v.Name)
                .ToList();
        }
        internal bool IsPropertyDrawRequired(SerializedProperty property)
        {
            return !fields.Contains(property.name);
        }

        internal static void HorizontalLine(float height = 1)
        {
            float c = EditorGUIUtility.isProSkin ? 0.5f : 0.4f;
            HorizontalLine(new Color(c, c, c), height);
        }

        internal static void HorizontalLine(Color color, float height = 1)
        {
            Rect rect = EditorGUILayout.GetControlRect(false, height);
            EditorGUI.DrawRect(rect, color);
            EditorGUILayout.GetControlRect(false, height);
        }
        #endregion
    }
}