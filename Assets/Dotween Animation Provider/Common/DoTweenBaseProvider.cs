using UnityEngine;
using DG.Tweening;
using RoboRyanTron.SearchableEnum;

namespace zFramework.Extension.Tweening
{
    public abstract class DoTweenBaseProvider : MonoBehaviour, IDoTweenProviderBehaviours
    {
        [HideInInspector] public Object target;
        [Tooltip("勾上后，每次当游戏对象激活时开始播放动画")]
        public bool playOnAwake = true;
        [Tooltip("勾上后，动画完成既销毁，需要回放(Rewind)请设为 false")]
        public bool setAutoKill =  true;
        public float delay = 0f;
        public float duration = 2f;
        public int loopcount = 0;
        public LoopType loopType = LoopType.Restart;
        [SearchableEnum]
        public Ease ease = Ease.Linear;
        public Tweener tweener;
        public Tweener Tweener => tweener;
        public bool IsPlaying => null != tweener && tweener.IsPlaying();
        public virtual void OnEnable()
        {
            if (playOnAwake) Play();
        }
        public virtual void OnDisable() => Stop();
        public virtual void OnValidate() { }
        public virtual void Play()
        {
            tweener?.Kill();
            tweener = null;
            tweener = InitTween();
            if (!target) target = (Object)tweener.target;
            tweener.SetDelay(delay)
                         .SetAutoKill(setAutoKill)
                         .SetEase(ease)
                         .SetLoops(loopcount, loopType)
                         .SetTarget(target);
      
        }
        public abstract Tweener InitTween();
        public virtual void Stop()=>tweener?.Kill();
        public virtual void Rewind()=>tweener?.Rewind();
    }
}