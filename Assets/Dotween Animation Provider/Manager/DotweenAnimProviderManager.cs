using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class DotweenAnimProviderManager : MonoBehaviour
{
    public List<IDoTweenAnimProviderBehaviours> Tweeners => ReloadSubProviders();
   internal List<IDoTweenAnimProviderBehaviours> ReloadSubProviders() 
    {
        var tweeners = new List<IDoTweenAnimProviderBehaviours>();
        tweeners.Clear();
        tweeners.AddRange(GetComponentsInChildren<IDoTweenAnimProviderBehaviours>());
        return tweeners;
    }

    public void Stop()
    {
        isPerformPreviewing = false;
        foreach (var item in  Tweeners)
        {
            item.Stop();
        }
    }
    public void Play()
    {
        isPerformPreviewing = true;
        foreach (var item in Tweeners)
        {
            item.Play();
        }
    }
    private bool isPerformPreviewing = false;
    public bool IsTweening=>isPerformPreviewing&&(Tweeners.Select(v => v.IsPlaying).Any(x=>x));
}
