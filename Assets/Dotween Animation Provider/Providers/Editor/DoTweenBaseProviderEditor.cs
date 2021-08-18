using UnityEditor;
using UnityEngine;
namespace zFramework.Extension.Tweening
{
    [CustomEditor(typeof(DoTweenBaseProvider))]
    public class DoTweenBaseProviderEditor : Editor
    {
        public DoTweenBaseProvider provider;

        public override void OnInspectorGUI()
        {
            DrawCustomHeaderInfo();
            GUI.enabled = EditorApplication.isPlaying || !provider.IsPreviewing();
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
            DrawCustomGUI();
            if (serializedObject.hasModifiedProperties)
            {
                serializedObject.ApplyModifiedProperties();
            }
            GUI.enabled = !EditorApplication.isPlaying && ((Component)provider).gameObject.activeInHierarchy;
            bool isTweening = !EditorApplication.isPlaying && provider.IsPreviewing();
            if (GUILayout.Button(isTweening ? "停止预览" : "开始预览"))
            {
                if (isTweening)
                {
                    provider.StopPreview();
                }
                else
                {
                    provider.StartPreview(OnTweenerUpdating);
                }
            }
        }

        public virtual void DrawCustomHeaderInfo() { }
        public virtual void OnTweenerUpdating() { }
        public virtual void DrawCustomGUI() { }
        public virtual void OnDisable()
        {
            provider.StopPreview();
        }
        public virtual void OnEnable()
        {
            provider = target as DoTweenBaseProvider;
        }
    }
}