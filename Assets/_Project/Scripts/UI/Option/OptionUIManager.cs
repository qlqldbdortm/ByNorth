using System.Reflection;
using ByNorth.Core;
using ByNorth.InputHandler;
using UnityEngine;
using UnityEngine.UI;

namespace ByNorth.UI.Option
{
    public class OptionUIManager: Singleton<OptionUIManager>
    {
        // TODO: Option은 Gamepad로 조작하는 거 고려해야 할 것 같음
        public GameObject optionRoot;
        public GameObject[] tabPages;


        public OptionLine FocusedOption { get; set; } = null;
        private int currentTab = -1;


        void Start()
        {
            ChangeTab(0);
        }


        public void OpenOption()
        {
            InputManager.ChangeActionMap(ActionMapType.Option);
            optionRoot.SetActive(true);
        }
        public void CloseOption()
        {
            InputManager.ChangeActionMap(ActionMapType.UI);
            optionRoot.SetActive(false);
        }

        public void Select()
        {
            
        }

        public void Unselect()
        {
            // TODO: 선택된 게 있는지 확인

            CloseOption();
        }
        
        public void ChangeTab(int tab)
        {
            if (tab == currentTab) return;
            
            currentTab = tab;
            for (int i = 0; i < tabPages.Length; i++)
            {
                tabPages[i].SetActive(i == currentTab);
            }
            tabPages[currentTab].GetComponentInChildren<OptionLine>()?.Focus();
        }

        private void ChangeTab(bool isLeft)
        {
            if (isLeft && currentTab <= 0) return;
            if (!isLeft && currentTab >= tabPages.Length - 1) return;
            
            ChangeTab(currentTab + (isLeft ? -1 : 1));
        }

        public void MoveCursor(KeyCode keyCode)
        {
            if (FocusedOption is null)
            {
                tabPages[currentTab].GetComponentInChildren<OptionLine>()?.Focus();
                return;
            }
            
            OptionLine next = null;
            switch (keyCode)
            {
                case KeyCode.UpArrow:
                    next = FocusedOption?.GetUp();
                    break;
                case KeyCode.DownArrow:
                    next = FocusedOption?.GetDown();
                    break;
                case KeyCode.LeftArrow:
                case KeyCode.RightArrow:
                    ChangeTab(keyCode == KeyCode.LeftArrow);
                    break;
            }

            next?.Focus();
        }

        private void ChangeOption(bool isLeft)
        {
            if(FocusedOption is null) return;

            if (FocusedOption.uiElement is Dropdown dropdown)
            {
                if (isLeft && dropdown.value <= 0) return;
                if (!isLeft && dropdown.value >= dropdown.options.Count - 1) return;
                
                dropdown.value += isLeft ? -1 : 1;
            }
            else if (FocusedOption.uiElement is Toggle toggle)
            {
                toggle.isOn = !toggle.isOn;
            }
        }
    }
}