using DG.DOTweenEditor;
using DG.Tweening;
using System;
using UnityEditor;
using UnityEngine;
using zFramework;
using zFramework.Extension;
using Object = UnityEngine.Object;
namespace zFramework.Extension.Tweening
{
    [CustomEditor(typeof(TransformPosProvider))]
    class TransformPosProviderEditor : DoTweenBaseProviderEditor
    {
        const string info = @"
存在多个 Provider ，请挂载 Manager 预览全部动画！
";
        public override void DrawCustomHeaderInfo()
        {
            if (provider.GetComponents<IDoTweenProviderBehaviours>().Length > 1 && !provider.GetComponent<DotweenAnimProviderManager>())
            {
                EditorGUILayout.HelpBox(info, MessageType.Info);
            }
        }
        public override void DrawCustomGUI()
        {
            var itr = serializedObject.GetIterator();
            itr.NextVisible(true);
            while (itr.NextVisible(false))
            {
                EditorGUILayout.PropertyField(itr, true);
            }
        }
    }
}