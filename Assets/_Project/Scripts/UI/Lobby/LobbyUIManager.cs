using ByNorth.Core;
using ByNorth.Core.GameFlow;
using ByNorth.InputHandler;
using ByNorth.UI.Option;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ByNorth.UI.Lobby
{
    public class LobbyUIManager: Singleton<LobbyUIManager>
    {
        public LobbyButton defaultButton = null;
        public GameObject optionObject = null;
        
        
        public LobbyButton FocusedButton { get; set; } = null;
        
        
        public void MoveCursor(KeyCode keyCode)
        {
            if (FocusedButton is null)
            {
                defaultButton.Focus();
                return;
            }

            LobbyButton next = null;
            switch (keyCode)
            {
                case KeyCode.UpArrow:
                case KeyCode.LeftArrow:
                    next = FocusedButton.GetUp();
                    break;
                case KeyCode.DownArrow:
                case KeyCode.RightArrow:
                    next = FocusedButton.GetDown();
                    break;
            }

            if (next is null) return;

            FocusedButton?.Unfocus();
            next.Focus();
        }

        public void OnSelect()
        {
            FocusedButton?.OnClick();
        }

        public void NewGame()
        {
            PlayerInfo.Instance.HasSaveData = false;
            PlayerInfo.Instance.RandomNextStage();
            
            SceneManager.LoadScene("InGame");
        }
        
        public void LoadGame()
        {
            PlayerInfo.Instance.HasSaveData = true;
            SaveManager.LoadPlayerInfo();
            
            SceneManager.LoadScene("InGame");
        }

        public void OpenOption()
        {
            OptionUIManager.Instance.OpenOption();
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}