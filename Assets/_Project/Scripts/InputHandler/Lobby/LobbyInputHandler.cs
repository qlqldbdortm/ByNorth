using ByNorth.Extensions;
using ByNorth.UI.Lobby;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ByNorth.InputHandler.Lobby
{
    public class LobbyInputHandler : InputHandlerBase
    {
        private bool lockMove = false;


        protected override void Init()
        {
            InputActionMap action = InputManager.GetActionMap(ActionMapType.UI);
            action.AddAction("Move", OnMove);
            action.AddAction("Select", OnSelect, null, null);
        }

        private void OnMove(InputAction.CallbackContext context)
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
                LobbyUIManager.Instance.MoveCursor(keyCode);
            }
        }
        private void OnSelect(InputAction.CallbackContext context)
        {
            LobbyUIManager.Instance.OnSelect();
        }
    }
}
