using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ByNorth.UI.InGame
{
    public class MenuButton: Selectable, IPointerEnterHandler, IPointerExitHandler
    {
        public static MenuButton FocusedButton { get; private set; } = null;


        public GameObject focusedObject;
        
        
        private Button button;


        void Awake()
        {
            button = GetComponentInChildren<Button>();
        }


        public void OnPointerEnter(PointerEventData eventData)
        {
            Focus();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Unfocus();
        }


        public void OnClick()
        {
            FocusedButton = null;
            button.onClick?.Invoke();
        }

        public void Focus()
        {
            FocusedButton?.Unfocus();
            FocusedButton = this;
            OnFocus();;
        }
        public void Unfocus()
        {
            OnUnfocus();
            FocusedButton = null;
        }

        private void OnFocus()
        {
            focusedObject.SetActive(true);
        }
        private void OnUnfocus()
        {
            focusedObject.SetActive(false);
        }

        public MenuButton GetUp() => FindSelectableOnUp()?.GetComponent<MenuButton>();
        public MenuButton GetDown() => FindSelectableOnDown()?.GetComponent<MenuButton>();
    }
}