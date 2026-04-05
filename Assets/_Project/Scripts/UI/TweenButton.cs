using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ByNorth.UI
{
    public class TweenButton: Button, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler
    {
        public override void OnPointerDown(PointerEventData eventData)
        {
            transform.DOScale(0.9f, 0.1f).SetEase(Ease.InOutCirc);
        }
        public override void OnPointerUp(PointerEventData eventData)
        {
            transform.DOScale(1f, 0.1f).SetEase(Ease.InOutCirc);
        }
        
        public override void OnPointerEnter(PointerEventData eventData)
        {
            transform.DOComplete();
            transform.DOScale(1.05f, 0.1f).SetEase(Ease.InOutCirc).SetLoops(2, LoopType.Yoyo);
        }
    }
}