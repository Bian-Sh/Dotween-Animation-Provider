using System.Collections.Generic;
using UnityEngine;

namespace zFramework.Extension.Tweening
{
    public class DotweenProviderManager : MonoBehaviour
    {
        public List<IDoTweenProviderBehaviours> Providers => new List<IDoTweenProviderBehaviours>(GetComponentsInChildren<IDoTweenProviderBehaviours>());
    }
}