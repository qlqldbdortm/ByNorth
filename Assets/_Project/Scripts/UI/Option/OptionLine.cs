using System;
using UnityEngine;
using UnityEngine.UI;

namespace ByNorth.UI.Option
{
    public class OptionLine: SelectableLine<OptionLine>
    {
        [SerializeField] private GameObject focusedObject;
        public Selectable uiElement;
        

        public Action OnClickEvent { get; set; } = null;
        
        
        public override void OnClick()
        {
            OnClickEvent?.Invoke();
        }
        
        public override void OnFocus()
        {
            focusedObject?.SetActive(true);
        }
        public override void OnUnfocus()
        {
            focusedObject?.SetActive(false);
        }

        public override void Focus()
        {
            OptionUIManager.Instance.FocusedOption?.Unfocus();
            OptionUIManager.Instance.FocusedOption = this;
            OnFocus();
        }
        public override void Unfocus()
        {
            OptionUIManager.Instance.FocusedOption = null;
            OnUnfocus();
        }
    }
}