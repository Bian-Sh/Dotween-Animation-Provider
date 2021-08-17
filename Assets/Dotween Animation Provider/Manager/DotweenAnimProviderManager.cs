using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace zFramework.Extension
{
    public class DotweenAnimProviderManager : MonoBehaviour
    {
        public List<IDoTweenAnimProviderBehaviours> Providers => ReloadSubProviders();
        internal List<IDoTweenAnimProviderBehaviours> ReloadSubProviders()
        {
            var tweeners = new List<IDoTweenAnimProviderBehaviours>();
            tweeners.Clear();
            tweeners.AddRange(GetComponentsInChildren<IDoTweenAnimProviderBehaviours>());
            return tweeners;
        }
    }
}