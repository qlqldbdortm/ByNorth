using ByNorth.Extensions;
using ByNorth.SlotSystem;
using ByNorth.UI.InGame;
using ByNorth.UI.Player;
using ByNorth.Unit.Behaviour.Movable;
using ByNorth.Unit.Behaviour.Structure;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ByNorth.InputHandler.InGame
{
    public class PlayerInputHandler : InputHandlerBase
    {
        [Header("바닥 레이어")]
        public LayerMask groundLayer;

        
        public static bool CanControl
        {
            get
            {
                if (!PlayerHandler.Player.Unit.IsAlive) return false;
                
                if (InputManager.CurrentActionMap != ActionMapType.Player) return false;
                
                if (InputManager.CurrentMode == InputMode.KeyBoardAndMouse)
                {
                    Mouse mouse = Mouse.current;
                    if (mouse is null) return false;
                    
                    // 마우스가 화면 안에 있는지 체크
                    Vector2 mousePos = mouse.position.ReadValue();
                    bool hasInsideX = mousePos.x >= 0 && mousePos.x < Screen.width;
                    bool hasInsideY = mousePos.y >= 0 && mousePos.y < Screen.height;
                    if (!hasInsideX || !hasInsideY) return false;

                    // 마우스가 하단 UI 그룹 위에 있는지 확인
                    if (BottomEnterChecker.Instance?.HasEntered ?? true) return false;
                }
                return true;
            }
        }

        
        private PlayerHandler handler = null;
        private Camera mainCamera = null;

        
        void Start()
        {
            handler = PlayerHandler.Player;
        }
        
        protected override void Init()
        {
            mainCamera = Camera.main;
            
            InputActionMap actionMap = InputManager.GetActionMap(ActionMapType.Player);
            actionMap.AddAction("Move", OnMove);
            actionMap.AddAction("Look", OnLook);
            actionMap.AddAction("QuickSlot", OnQuickSlot, null, null);
            actionMap.AddAction("Interaction", OnInteraction, null, null);
            actionMap.AddAction("OpenStat", OnOpenStat, null, null);
            actionMap.AddAction("ToggleSlot", OnToggleSlot, null, null);
            actionMap.AddAction("OpenInventory", OnOpenInventory, null, null);
            actionMap.AddAction("OpenMenu", OnOpenMenu, null, null);
            actionMap.AddAction("Sprint", OnSprint, null, OnSprint);
            actionMap.AddAction("SoftAttack", null, null, OnSoftAttack);
            actionMap.AddAction("UtilSlot", OnUtilSlot, null, null);
            actionMap.AddAction("HardAttack", null, null, OnHardAttack);

            InputManager.OnInputModeChanged += OnInputModeChanged;
        }

        void Update()
        {
            if (!CanControl) return;
            
            UpdateLookRotation();
        }

        private void UpdateLookRotation()
        {
            if (InputManager.CurrentMode == InputMode.KeyBoardAndMouse)
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayer))
                {
                    Vector3 direction = hit.point - handler.transform.position;
                    Vector2 dir = new Vector2(direction.x, direction.z).normalized;
                    handler?.SetLook(dir);
                }
            }
        }

        private void OnInputModeChanged()
        {
            handler.SetLook(Vector2.zero);
        }
        
        private void OnMove(InputAction.CallbackContext context)
        {
            if (handler == null) return;

            Vector2 dir = context.ReadValue<Vector2>();
            handler.SetMove(dir);
        }

        private void OnLook(InputAction.CallbackContext context)
        {
            if (handler == null) return;

            Vector2 dir = context.ReadValue<Vector2>();
            handler.SetLook(dir);
        }

        private void OnQuickSlot(InputAction.CallbackContext context)
        {
            Vector2 input = context.ReadValue<Vector2>();
            int selectSlotIdx = -1;
            if (input.y > 0.5f)
            {
                selectSlotIdx = 0;
            }
            else if (input.y < 0.5f && !Mathf.Approximately(input.y, 0))
            {
                selectSlotIdx = 2;
            }
            else if (input.x < 0.5f)
            {
                selectSlotIdx = 3;
            }
            else if (input.x > 0.5f && !Mathf.Approximately(input.x, 0))
            {
                selectSlotIdx = 1;
            }
            
            if (selectSlotIdx != -1)
            {
                SlotManager.Instance.UseQuickSlot(selectSlotIdx);
            }
        }

        private void OnInteraction(InputAction.CallbackContext context)
        {
            if (!StoragableManager.Instance.HasSelected) return;
            
            InGameUIManager.Instance.OpenInventory(true);
        }

        private void OnOpenStat(InputAction.CallbackContext context)
        {
            InGameUIManager.Instance.OpenStats();
        }
        private void OnToggleSlot(InputAction.CallbackContext context)
        {
            InGameUIManager.Instance.ToggleQuickSlot();
        }
        private void OnOpenInventory(InputAction.CallbackContext context)
        {
            InGameUIManager.Instance.OpenInventory();
        }
        private void OnOpenMenu(InputAction.CallbackContext context)
        {
            InGameUIManager.Instance.OpenMenu();
        }

        private void OnSprint(InputAction.CallbackContext context)
        {
            float value = context.ReadValue<float>();
            handler.SetSprint(value);
        }

        private void OnUtilSlot(InputAction.CallbackContext context)
        {
            Debug.Log("UtilSlot키 입니다.");
        }

        private void OnSoftAttack(InputAction.CallbackContext context)
        {
            if (!CanControl) return;
            
            handler.ControlBehaviour(ControlType.NormalAttack);
        }

        private void OnHardAttack(InputAction.CallbackContext context)
        {
            if (!CanControl) return;

            handler.ControlBehaviour(ControlType.HardAttack);
        }
    }
}
