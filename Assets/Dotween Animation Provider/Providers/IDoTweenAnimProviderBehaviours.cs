using DG.Tweening;

public interface IDoTweenAnimProviderBehaviours 
{
    Tweener Tweener { get; }
    bool IsPlaying { get; }
    Tweener Play();
    void Stop();
}
