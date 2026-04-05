using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

namespace ByNorth.UI.End
{
    public class EndSceneManager : MonoBehaviour
    {
        public TMP_Text textDisplay;
        public Image backgroundImage;

        public int skipSentence = -1;
        public string[] sentences;
        public Sprite[] images;
        public float typingDelay = 0.05f;
        

        private int currentIndex = 0;
        private Coroutine typingCoroutine;

        void Start()
        {
            textDisplay.text = "";
            currentIndex = 0;
            ShowNext();
        }

        void Update()
        {
            if (Input.anyKeyDown)
            {
                if (typingCoroutine != null)
                {
                    StopCoroutine(typingCoroutine);
                    textDisplay.text = sentences[currentIndex];
                    typingCoroutine = null;
                }
                else
                {
                    currentIndex++;
                    if (skipSentence == currentIndex)
                    {
                        currentIndex++;
                    }
                    
                    if (currentIndex < sentences.Length)
                        ShowNext();
                    else
                    {
                        SceneManager.LoadScene("Lobby");
                    }
                }
            }
        }
        void ShowNext()
        {
            if (images != null && currentIndex < images.Length)
                backgroundImage.sprite = images[currentIndex];

            typingCoroutine = StartCoroutine(TypeSentence(sentences[currentIndex]));
        }
        IEnumerator TypeSentence(string sentence)
        {
            WaitForSeconds wait = new WaitForSeconds(typingDelay);
            
            textDisplay.text = "";
            foreach (char c in sentence)
            {
                textDisplay.text += c;
                yield return wait;
            }
            typingCoroutine = null;
        }
    }
}
