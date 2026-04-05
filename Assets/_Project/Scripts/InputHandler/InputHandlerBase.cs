using UnityEngine;
using UnityEngine.InputSystem;

namespace ByNorth.InputHandler
{
    public abstract class InputHandlerBase: MonoBehaviour
    {
        protected PlayerInput PlayerInput { get; private set; } = null;
        protected InputMode CurrentMode => InputManager.CurrentMode;


        protected virtual void Awake()
        {
            PlayerInput = InputManager.PlayerInput;
            
            Init();
        }

        protected abstract void Init();
    }
}