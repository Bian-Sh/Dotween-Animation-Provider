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
            target = text;// ��һ�����Բ�����
            return DOTween.To(() => 0, y => text.text = y.ToString(), value, duration); //tostring ��GC
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