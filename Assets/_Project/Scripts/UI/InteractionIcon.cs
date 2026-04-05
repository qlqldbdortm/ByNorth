using System;
using ByNorth.InputHandler;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace ByNorth.UI
{
    public class InteractionIcon: MonoBehaviour
    {
        public Sprite keyboardAndMouseIcon;
        public Sprite gamepadIcon;
        
        
        private Image image;


        void Awake()
        {
            image = GetComponent<Image>();
        }

        void OnEnable()
        {
            OnChangedController();
            InputManager.OnInputModeChanged += OnChangedController;
        }
        void OnDisable()
        {
            InputManager.OnInputModeChanged -= OnChangedController;
        }


        private void OnChangedController()
        {
            image.sprite = InputManager.CurrentMode switch
            {
                InputMode.KeyBoardAndMouse => keyboardAndMouseIcon,
                InputMode.Gamepad => gamepadIcon,
                _ => null
            };
        }
    }
}