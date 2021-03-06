using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace zFramework.Extension.Tweening
{
    [RequireComponent(typeof(Graphic))]
    public class GraphicColorChangeProvider : DoTweenBaseProvider
    {
        [Header("颜色变化、颜色混合、渐隐渐显，都请用该组件实现")]
        public Color endValue = default;
        [HideInInspector]
        public Graphic graphic;

        private void Awake() => graphic = GetComponent<Graphic>();

        #region 编辑器下初始化和空引用防治举措
        public override void OnValidate() => graphic = graphic ?? GetComponent<Graphic>();
        private void Reset()
        {
            graphic = GetComponent<Graphic>();
            endValue = graphic.color; //捕获初始值
        }
        #endregion

        public override Tweener InitTween()
        {
            var arr = GetComponents<GraphicColorChangeProvider>();
            var blendable = null != arr && arr.Length > 1;
            if (blendable)
            {
                Debug.Log($"发现多个 {nameof(GraphicColorChangeProvider)} 进入颜色混合模式！");
            }
            return blendable ? graphic.DOBlendableColor(endValue, duration) : graphic.DOColor(endValue, duration);
        }
        public override void Stop()
        {
            base.Stop();
            tweener?.Rewind(); //复位 Dotween 所作的修改
            tweener = null;
        }
    }
}