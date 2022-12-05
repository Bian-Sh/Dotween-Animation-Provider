using UnityEngine;
using DG.Tweening;

namespace zFramework.Extension.Tweening
{
    [DisallowMultipleComponent]
    public class TransformPosProvider : DoTweenBaseProvider
    {
        public bool isLocal = true;
        public bool snapping = false;
        public Vector3 endValue = Vector3.zero;
        private void Reset() => endValue = isLocal ? transform.localPosition : transform.position;
        public override Tweener InitTween()
        {
            tweener = isLocal ? transform.DOLocalMove(endValue, duration, snapping) : transform.DOMove(endValue, duration, snapping);
            return tweener;
        }
        public override void Stop()
        {
            base.Stop();
            tweener?.Rewind(); //复位 Dotween 所作的修改
            tweener = null;
        }
    }
}