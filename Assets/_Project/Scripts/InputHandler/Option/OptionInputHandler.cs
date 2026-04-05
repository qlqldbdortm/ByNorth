using UnityEngine;
using UnityEngine.InputSystem;
using ByNorth.Extensions;
using ByNorth.UI.Option;

namespace ByNorth.InputHandler.Option
{
    public class OptionInputHandler: InputHandlerBase
    {
        private bool lockMove = false;
        
        
        protected override void Init()
        {
            InputActionMap action = InputManager.GetActionMap(ActionMapType.Option);
            action.AddAction("Move", OnMove);
            action.AddAction("Select", OnSelect);
            action.AddAction("Unselect", OnUnselect);
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                lockMove = false;
            }
            else
            {
                if (lockMove) return;
                
                Vector2 input = context.ReadValue<Vector2>();

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
                
                OptionUIManager.Instance.MoveCursor(keyCode);
                lockMove = true;
            }
        }
        private void OnSelect(InputAction.CallbackContext context)
        {
            OptionUIManager.Instance.Select();
        }
        private void OnUnselect(InputAction.CallbackContext context)
        {
            OptionUIManager.Instance.Unselect();
        }
    }
}