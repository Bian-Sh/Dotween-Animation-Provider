using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public static class DoTweenModuleUI
{
    /// <summary>Tweens a RectTransform's anchoredPosition to the given value.
    /// Also stores the RectTransform as the tween's target so it can be used for filtered operations</summary>
    /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
    /// <param name="snapping">If TRUE the tween will smoothly snap all values to integers</param>
    public static TweenerCore<Vector3, Vector3, VectorOptions> DOAnchorPos(this RectTransform target, Vector3 endValue, float duration, bool snapping = false)
    {
        TweenerCore<Vector3, Vector3, VectorOptions> t = DOTween.To(() => (Vector3)target.anchoredPosition, x => target.anchoredPosition = x, endValue, duration);
        t.SetOptions(snapping).SetTarget(target);
        return t;
    }
}
