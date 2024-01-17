using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
// TMPro 在居中对齐时，空格不会把字符串整体前移，所以表现效果是整个字符串都在前后不停的跳动，待Unity解决
//https://forum.unity.com/threads/why-there-is-no-setting-for-textmesh-pro-ugui-to-count-whitespace-at-the-end.676897/

namespace zFramework.Extension.Tweening
{
    [RequireComponent(typeof(Graphic))]
    public class TextLoadingAnimationProvider : DoTweenBaseProvider
    {
        [Header("For Text  and TextMeshPro")]
        public Graphic text;
        private string cached;
        public string Text
        {
            get => text is Text ? (text as Text).text : (text as TextMeshProUGUI).text;
            set
            {
                if (text is Text)
                {
                    (text as Text).text = value;
                }
                else
                {
                    (text as TextMeshProUGUI).text = value;
                }
            }
        }
        private void Awake() => text = GetComponent<Graphic>();
        private void Reset() => text = GetComponent<Graphic>();
        public override Tweener InitTween()
        {
            if (text)
            {
                string[] dots = { "   ", ".  ", ".. ", "...", };
                var msg = cached = Text;
                tweener = DOTween.To(() => 0, v => Text = $"{msg}{dots[v]}", 3, duration);
                target = text; //这个必须写！
            }
            else
            {
                Debug.LogError($"{nameof(TextLoadingAnimationProvider)}: 没有发现 Text 组件！");
            }
            return tweener;
        }
        public override void Stop()
        {
            base.Stop();
            tweener = null;
            Text = cached;
        }
        public override void OnValidate()
        {
            base.OnValidate();
            if (!(text is Text) && !(text is TMPro.TextMeshProUGUI))
            {
                Debug.LogError($"{nameof(TextLoadingAnimationProvider)}: Text Loading 组件需要 Text 或者 TMP 组件！");
                text = null;
            }
        }
    }
}
