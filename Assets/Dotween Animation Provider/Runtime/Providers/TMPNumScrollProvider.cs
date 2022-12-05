using DG.Tweening;
using TMPro;
using UnityEngine;

namespace zFramework.Extension.Tweening
{
    [DisallowMultipleComponent, RequireComponent(typeof(TextMeshProUGUI))]
    public class TMPNumScrollProvider : DoTweenBaseProvider
    {
        public TextMeshProUGUI text;
        public int value;
        public override Tweener InitTween()
        {
            target = text;// 这一步绝对不能少
            return DOTween.To(() => 0, y => text.text = y.ToString(), value, duration); //tostring 高GC
        }
        private void Reset() => text = GetComponent<TextMeshProUGUI>();
    }
}