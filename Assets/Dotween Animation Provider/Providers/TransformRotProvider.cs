using UnityEngine;
using DG.Tweening;
using RoboRyanTron.SearchableEnum;
[DisallowMultipleComponent]
public class TransformRotProvider : MonoBehaviour,IDoTweenAnimProviderBehaviours
{
    public bool playOnAwake = true;
    public bool isLocal = true;
    public float delay = 0f;
    public float duration = 1f;
    public int loopcount = 0;
    public LoopType loopType = LoopType.Restart;
    [SearchableEnum]
    public Ease ease = Ease.Linear;
    public RotateMode rotateMode = RotateMode.Fast;
    public Vector3 endValue = Vector3.zero;
    public Tweener Tweener=>tweener;
    public Tweener tweener;
    public bool IsPlaying { get; private set; }

    #region Engine Func
    void OnEnable()
    {
        if (playOnAwake)
        {
            Play();
        }
    }
    void OnDisable() => Stop();
    private void OnValidate() => loopcount = loopcount < -1 ? -1 : loopcount;
    private void Reset() => endValue = isLocal ? transform.localEulerAngles : transform.eulerAngles;
    #endregion
    public void Play()
    {
        tweener = (isLocal ? transform.DOLocalRotate(endValue, duration, rotateMode) : transform.DORotate(endValue, duration, rotateMode))
                        .SetDelay(delay)
                        .SetEase(ease)
                        .SetLoops(loopcount, loopType);
        IsPlaying = true;
    }
    public void Stop()
    {
        IsPlaying = false;
        //this.transform.DOKill(); //此 api 必须找到tween所影响的object 且不会将 tweener 置空
        tweener?.Rewind(); //复位 Dotween 所作的修改
        tweener?.Kill();
        tweener = null;
    }
}
