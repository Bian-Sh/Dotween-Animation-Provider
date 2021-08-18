using DG.Tweening;
using TMPro;

namespace zFramework.Extension.Tweening
{
    public class TextMeshProNumScroller : DoTweenBaseProvider
    {
        public TextMeshProUGUI text;
        public int value;

        public override Tweener InitTween()
        {
            target = text;// 这一步绝对不能少
            return DOTween.To(() => 0, y => text.text = y.ToString(), value, duration); //tostring 高GC
        }

        public override void Play()
        {
            base.Play();
        }
        public override void Stop()
        {
            base.Stop();
        }
    }
}