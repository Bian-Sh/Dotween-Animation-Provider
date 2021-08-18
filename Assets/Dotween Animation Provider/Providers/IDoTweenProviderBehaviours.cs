using DG.Tweening;

namespace zFramework.Extension.Tweening
{
    public interface IDoTweenProviderBehaviours
    {
        Tweener Tweener { get; }
        bool IsPlaying { get; }
        Tweener InitTween();
        void Play();
        void Stop();
    }
}