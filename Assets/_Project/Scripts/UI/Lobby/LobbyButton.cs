using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ByNorth.UI.Lobby
{
    public class LobbyButton: SelectableLine<LobbyButton>, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private GameObject focusedObject;

            
        private TweenButton button;

        
        protected override void Awake()
        {
            button = GetComponentInChildren<TweenButton>();
        }
        
        
        public override void OnClick()
        {
            button.onClick?.Invoke();
        }

        public override void OnFocus()
        {
            focusedObject?.SetActive(true);
            button?.OnPointerEnter(null);
        }
        public override void OnUnfocus()
        {
            focusedObject?.SetActive(false);
        }
        public override void Focus()
        {
            LobbyUIManager.Instance.FocusedButton = this;
            OnFocus();
        }
        public override void Unfocus()
        {
            if (LobbyUIManager.Instance.FocusedButton != this) return;
            
            LobbyUIManager.Instance.FocusedButton = null;
            OnUnfocus();
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            Focus();
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            Unfocus();
        }
    }
}