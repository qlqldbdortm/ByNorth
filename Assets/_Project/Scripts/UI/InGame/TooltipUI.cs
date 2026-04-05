using System.Collections;
using System.Collections.Generic;
using ByNorth.Extensions;
using ByNorth.SlotSystem.Data;
using TMPro;
using UnityEngine;

namespace ByNorth
{
    public class TooltipUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI tooltipText;
        [SerializeField] private RectTransform tooltipBox;

        void Awake()
        {
            if (tooltipText == null)
            {
                tooltipText = GetComponentInChildren<TextMeshProUGUI>();
            }
            if (tooltipBox == null)
            {
                tooltipBox = transform as RectTransform;
            }
            gameObject.SetActive(false);
        }

        public void ShowTooltip(SlotData slotData)
        {
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }
            tooltipText.text = slotData.ItemToolTip();
        }

        public void ShowTooltip(Vector2 pos)
        {
            tooltipBox.anchoredPosition = pos;
        }
        
        public void HideTooltip()
        {
            gameObject.SetActive(false);
        }
    }
}
