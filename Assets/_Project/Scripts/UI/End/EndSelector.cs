using ByNorth.Core.GameFlow;
using UnityEngine;

namespace ByNorth.UI.End
{
    public class EndSelector: MonoBehaviour
    {
        public EndSceneManager loseEnding = null;
        public EndSceneManager normalEnding = null;
        public EndSceneManager badEnding = null;
        
        
        void Start()
        {
            if (PlayerInfo.Instance.HasClear)
            {
                if (PlayerInfo.Instance.NormalKarma <= 50)
                {
                    normalEnding.gameObject.SetActive(true);
                }
                else
                {
                    badEnding.gameObject.SetActive(true);
                    if (PlayerInfo.Instance.ChildKarma < 20)
                    {
                        badEnding.skipSentence = 4;
                    }
                }
            }
            else
            {
                loseEnding.gameObject.SetActive(true);
            }
        }
    }
}