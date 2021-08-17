using UnityEngine;
using DG.Tweening;
using TMPro;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

public class TextMeshProNumScroller : MonoBehaviour, IDoTweenAnimProviderBehaviours
{
    public TextMeshProUGUI text;
    public int value;
    public float duration = 2f;
    public int loopcount = 0;
    public LoopType loopType = LoopType.Restart;
    public Ease ease = Ease.Linear;
    public TweenerCore<int,int,NoOptions> tweener;

    public Tweener Tweener => tweener;
    public bool IsPlaying => null != tweener && tweener.IsPlaying();

    private void OnEnable()
    {
        Play();
    }
    private void OnDisable()
    {
        Stop();
    }

    public Tweener Play()
    {
        Stop();
        tweener = DOTween.To(() => 0, y => text.text=y.ToString(), value, duration) //tostring ¸ßGC
                .SetEase(ease)
                .SetLoops(loopcount, loopType)
                .SetTarget(text);
        return tweener;
    }
    public void Stop()
    {
        if (tweener != null)
        {
            tweener.Kill();
            tweener = null;
        }
    }
}
