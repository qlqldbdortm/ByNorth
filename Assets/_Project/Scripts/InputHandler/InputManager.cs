using System;
using ByNorth.UI.Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace ByNorth.InputHandler
{
    public class InputManager : MonoBehaviour
    {
        public static Action OnInputModeChanged { get; set; } = null;
        
        public static PlayerInput PlayerInput { get; private set; }
        public static Action OnChangedScene { get; set; }
        public static InputMode CurrentMode { get; private set; } = InputMode.None;

        public static ActionMapType CurrentActionMap { get; private set; } = ActionMapType.UI;

        
        void Awake()
        {
            if (PlayerInput is null)
            {
                DontDestroyOnLoad(gameObject);
                PlayerInput = GetComponent<PlayerInput>();
                SceneManager.sceneUnloaded += OnUnloadScene;
                SceneManager.sceneLoaded += OnChangeScene;
            }
            else
            {
                DestroyImmediate(gameObject);
            }
        }

        void Update()
        {
            CheckInputMode();
        }

        
        private void CheckInputMode()
        {
            // 현재 InputMode 판단
            InputMode currentMode = InputMode.None;
            switch (PlayerInput.currentControlScheme)
            {
                case "Gamepad":
                    currentMode = InputMode.Gamepad;
                    break;
                case "KeyboardMouse":
                default:
                    currentMode = InputMode.KeyBoardAndMouse;
                    break;
            }

            // InputMode가 변경된 거라면, 변경된 대로 설정을 변경하는 작업
            if (currentMode == CurrentMode) return;
            
            switch (currentMode)
            {
                case InputMode.Gamepad:
                    Cursor.visible = false;
                    break;
                case InputMode.KeyBoardAndMouse:
                default:
                    Cursor.visible = true;
                    break;
            }
            CurrentMode = currentMode;
            OnInputModeChanged?.Invoke();
        }

        /// <summary>
        /// 현재 PlayerInput의 ActionMap을 변경
        /// </summary>
        /// <param name="actionMapType">변경할 ActionMapType</param>
        public static void ChangeActionMap(ActionMapType actionMapType)
        {
            PlayerInput.currentActionMap = GetActionMap(actionMapType);
            CurrentActionMap = actionMapType;
        }
        /// <summary>
        /// ActionMap 반환
        /// </summary>
        /// <param name="actionMapName">반환할 ActionMap의 명칭</param>
        /// <returns></returns>
        public static InputActionMap GetActionMap(ActionMapType actionMapType) => PlayerInput.actions.FindActionMap(actionMapType.ToString(), true);

        /// <summary>
        /// 기존에 ActionMap에 구독된 event를 Clear
        /// </summary>
        /// <param name="oldScene"></param>
        private void OnUnloadScene(Scene oldScene)
        {
            OnChangedScene?.Invoke();
            OnChangedScene = null;
            OnInputModeChanged = null;
        }
        /// <summary>
        /// 새로운 Scene에 맞춰 기본 ActionMap을 변경
        /// </summary>
        /// <param name="newScene"></param>
        /// <param name="mode"></param>
        private void OnChangeScene(Scene newScene, LoadSceneMode mode)
        {
            ChangeActionMap(newScene.name == "InGame" ? ActionMapType.Player : ActionMapType.UI);
        }
    }
}
