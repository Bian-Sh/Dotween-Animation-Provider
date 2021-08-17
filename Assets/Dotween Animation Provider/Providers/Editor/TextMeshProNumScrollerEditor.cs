using TMPro;
using UnityEditor;
using UnityEngine;
using zFramework.Editor.Extension;

[CustomEditor(typeof(TextMeshProNumScroller))]
public class TextMeshProNumScrollerEditor : Editor
{
    IDoTweenAnimProviderBehaviours scroller;
    GameObject gameObject;
    TextMeshProUGUI text;
    private void OnEnable()
    {
        scroller = target as IDoTweenAnimProviderBehaviours;
        gameObject = ((Component)scroller).gameObject;
        text = gameObject.GetComponent<TextMeshProUGUI>();
    }
    public override void OnInspectorGUI()
    {
        GUI.enabled = EditorApplication.isPlaying||!scroller.IsPreviewing();
        base.OnInspectorGUI();
        GUI.enabled = !EditorApplication.isPlaying && gameObject.activeInHierarchy;
        bool isTweening = !EditorApplication.isPlaying&&scroller.IsPreviewing();
        if (GUILayout.Button(isTweening?"Í£Ö¹Ô¤ÀÀ":"¿ªÊ¼Ô¤ÀÀ"))
        {
            if (isTweening)
            {
                scroller.StopPreview();
            }
            else
            {
                scroller.StartPreview(OnUpdate);
            }
        }
    }

    private void OnUpdate()
    {

    }

    void OnDisable() 
    {
        scroller.StopPreview();
    }
}
