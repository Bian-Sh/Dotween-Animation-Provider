using DG.DOTweenEditor;
using DG.Tweening;
using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
namespace zFramework.Extension.Tweening
{

    [CustomEditor(typeof(RectTransformDeltaSizeProvider))]
    class RectTransformDeltaSizeProviderEditor : DoTweenBaseProviderEditor
    {
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