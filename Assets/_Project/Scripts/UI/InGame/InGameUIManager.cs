using System;
using ByNorth.Core;
using ByNorth.InputHandler;
using ByNorth.SlotSystem;
using ByNorth.SlotSystem.Slot;
using ByNorth.Unit.Behaviour.Structure;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ByNorth.UI.InGame
{
    public class InGameUIManager: Singleton<InGameUIManager>
    {
        public static PopupUIType PopupType { get; private set; } = PopupUIType.None;

        public TextMeshProUGUI timerText;

        public GameObject menuPanel = null;
        public MenuButton defaultMenu = null;
        
        public void MoveCursor(KeyCode keyCode)
        {
            switch (PopupType)
            {
                case PopupUIType.Inventory:
                case PopupUIType.Stats:
                    SlotManager.Instance.MoveSlotFocus(keyCode);
                    break;
                case PopupUIType.Menu:
                    if (MenuButton.FocusedButton == null)
                    {
                        defaultMenu.Focus();
                        return;
                    }
                    MenuButton next = null;
                    switch (keyCode)
                    {
                        case KeyCode.UpArrow:
                        case KeyCode.LeftArrow:
                            next = MenuButton.FocusedButton.GetUp();
                            break;
                        case KeyCode.DownArrow:
                        case KeyCode.RightArrow:
                            next = MenuButton.FocusedButton.GetDown();
                            break;
                    }
                    if (next != null)
                    {
                        next.Focus();
                    }
                    break;
            }
        }

        public void Select()
        {
            switch (PopupType)
            {
                case PopupUIType.Menu:
                    MenuButton.FocusedButton?.OnClick();
                    break;
                case PopupUIType.Stats:
                case PopupUIType.Inventory:
                default:
                    if (Slot.SelectedSlot != null && Slot.SelectedSlot != Slot.FocusedSlot)
                    {
                        Slot.FocusedSlot.OnSwap(Slot.SelectedSlot);
                        Slot.SelectedSlot.Unselect();
                        return;
                    }
                    Slot.FocusedSlot.Select();
                    break;
            }
        }

        public void OpenOption()
        {
            // TODO: Option 열기
        }
        public void ReturnLobby()
        {
            SaveManager.Save();
            SceneManager.LoadScene("Lobby");
        }
        public void QuitGame()
        {
            Application.Quit();
        }
        
        public void ToggleQuickSlot()
        {
            if (PopupType != PopupUIType.None) return;
            
            SlotManager.Instance.ToggleQuickSlotGroup();
        }
        
        public void OpenStats()
        {
            OpenInit(PopupUIType.Stats);
            SlotManager.Instance.skillSlotGroup.slotsList[0].OnFocus();
            
            SlotManager.Instance.OpenStats();
        }

        public void OpenInventory(bool isWithStorage = false)
        {
            OpenInit(PopupUIType.Inventory);
            SlotManager.Instance.inventorySlotGroup.slotsList[0].OnFocus();
            
            SlotManager.Instance.OpenInventory();
            if (isWithStorage)
            {
                StoragableManager.Instance.OpenStorage();
            }
        }

        public void OpenMenu()
        {
            OpenInit(PopupUIType.Menu);

            menuPanel.SetActive(true);
        }
        
        public void Close()
        {
            switch (PopupType)
            {
                case PopupUIType.Stats:
                    SlotManager.Instance.CloseStats();
                    Slot.SelectedSlot?.Unselect();
                    break;
                case PopupUIType.Inventory:
                    SlotManager.Instance.CloseInventory();
                    StoragableManager.Instance.CloseStorage();
                    Slot.SelectedSlot?.Unselect();
                    break;
                case PopupUIType.Menu:
                    menuPanel.SetActive(false);
                    break;
            }
            SlotManager.Instance.tooltipUI.HideTooltip();
            CloseInit();
        }


        private void OpenInit(PopupUIType popupType)
        {
            PopupType = popupType;
            InputManager.ChangeActionMap(ActionMapType.UI);
        }
        private void CloseInit()
        {
            PopupType = PopupUIType.None;
            InputManager.ChangeActionMap(ActionMapType.Player);
        }
    }
}