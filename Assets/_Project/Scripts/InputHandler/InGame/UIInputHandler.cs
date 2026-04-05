using ByNorth.Extensions;
using ByNorth.SlotSystem;
using ByNorth.SlotSystem.Slot;
using ByNorth.UI.InGame;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ByNorth.InputHandler.InGame {
    public class UIInputHandler : InputHandlerBase {
        private bool lockMove = false;


        protected override void Init()
        {
            InputActionMap actionMap = InputManager.GetActionMap(ActionMapType.UI);
            actionMap.AddAction("Move", OnMove);
            actionMap.AddAction("CloseTab",OnCloseTab, null, null);
            actionMap.AddAction("Use", OnUse, null, null);
            actionMap.AddAction("Select", OnSelect, null, null);
        }


        public void OnMove(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                lockMove = false;
            }
            else
            {
                Vector2 input = context.ReadValue<Vector2>();

                if (input.magnitude < 0.25f) lockMove = false;

                if (lockMove) return;

                KeyCode keyCode = KeyCode.None;
                if (Mathf.Abs(input.y) > 0.5f)
                {
                    if (input.y > 0.5f)
                    {
                        keyCode = KeyCode.UpArrow;
                    }
                    else if (input.y < 0.5f)
                    {
                        keyCode = KeyCode.DownArrow;
                    }
                }
                else if (Mathf.Abs(input.x) > 0.5f)
                {
                    if (input.x < 0.5f)
                    {
                        keyCode = KeyCode.LeftArrow;
                    }
                    else if (input.x > 0.5f)
                    {
                        keyCode = KeyCode.RightArrow;
                    }
                }

                if (keyCode == KeyCode.None) return;

                lockMove = true;
                InGameUIManager.Instance.MoveCursor(keyCode);
            }
        }
        public void OnCloseTab(InputAction.CallbackContext context)
        {
            if (Slot.SelectedSlot != null)
            {
                Slot.SelectedSlot.Unselect();
                return;
            }
            
            InGameUIManager.Instance.Close();
        }
        public void OnUse(InputAction.CallbackContext context)
        {
            Slot.FocusedSlot?.Select();
            SlotManager.Instance.UseSelectedSlot();
            Slot.SelectedSlot?.Unselect();
        }
        public void OnSelect(InputAction.CallbackContext context)
        {
            InGameUIManager.Instance.Select();
        }
    }
}
