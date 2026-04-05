using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ByNorth.UI
{
    public class TextSlider: Slider
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private float duration = 0.5f;


        void Reset()
        {
            interactable = false;
            transition = Transition.None;
            text = GetComponentInChildren<TextMeshProUGUI>();
            fillRect = transform.Find("Fill Area/Fill").GetComponent<RectTransform>();
        }


        /// <summary>
        /// Text까지 같이 갱신 시킴
        /// </summary>
        /// <param name="value"></param>
        public void SetValue(float value)
        {
            text?.SetText($"{value:0} / {maxValue:0}");
            base.value = value;
        }
        /// <summary>
        /// SetValue에서 value를 DOTween을 사용해서 부드럽게 처리함
        /// </summary>
        /// <param name="value"></param>
        public void SetValueWithAnimation(float value)
        {
            text?.SetText($"{value:0} / {maxValue:0}");
            this.DOValue(value, duration);
        }
    }
}