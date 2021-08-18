using UnityEngine;
using DG.Tweening;
using zFramework.Editors.Extension;

namespace zFramework.Extension.Tweening
{
    public abstract class DoTweenBaseProvider : MonoBehaviour, IDoTweenProviderBehaviours
    {
        [HideInInspector] public Object target;
        public bool playOnAwake = true;
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
        public virtual void OnValidate() => loopcount = loopcount < -1 ? -1 : loopcount;
        public virtual void Play()
        {
            if (tweener != null)
            {
                tweener.Kill();
                tweener = null;
            }
            tweener = InitTween()
                    .SetDelay(delay)
                     .SetEase(ease)
                    .SetLoops(loopcount, loopType);
            if (!target) target = (Object)tweener.target;
            tweener.SetTarget(target);
        }

        public abstract Tweener InitTween();

        public virtual void Stop()
        {
            tweener?.Kill();
        }
    }
}