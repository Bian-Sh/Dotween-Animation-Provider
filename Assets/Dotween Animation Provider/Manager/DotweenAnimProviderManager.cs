using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace zFramework.Extension.Tweening
{
    public class DotweenAnimProviderManager : MonoBehaviour
    {
        public List<IDoTweenProviderBehaviours> Providers => ReloadSubProviders();
        internal List<IDoTweenProviderBehaviours> ReloadSubProviders()
        {
            var tweeners = new List<IDoTweenProviderBehaviours>();
            tweeners.Clear();
            tweeners.AddRange(GetComponentsInChildren<IDoTweenProviderBehaviours>());
            return tweeners;
        }
    }
}