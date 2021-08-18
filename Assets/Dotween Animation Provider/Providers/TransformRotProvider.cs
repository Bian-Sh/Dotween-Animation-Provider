using UnityEngine;
using DG.Tweening;
namespace zFramework.Extension.Tweening
{
    [DisallowMultipleComponent]
    public class TransformRotProvider:DoTweenBaseProvider
    {
        public bool isLocal = true;
        public RotateMode rotateMode = RotateMode.Fast;
        public Vector3 endValue = Vector3.zero;
        
        private void Reset() => endValue = isLocal ? transform.localEulerAngles : transform.eulerAngles;
       
        public override Tweener InitTween()
        {
            return isLocal ? transform.DOLocalRotate(endValue, duration, rotateMode) : transform.DORotate(endValue, duration, rotateMode);
        }
        public override void Stop()
        {
            base.Stop();
            tweener?.Rewind(); //复位 Dotween 所作的修改
            tweener = null;
        }
    }
}