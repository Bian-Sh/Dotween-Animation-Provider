using DG.Tweening;

public interface IDoTweenAnimProviderBehaviours 
{
    Tweener Tweener { get; }
    bool IsPlaying { get; }

    void Play();
    void Stop();
}
