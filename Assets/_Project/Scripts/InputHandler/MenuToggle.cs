//using AdvancedRogueLikeandPuzzleSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ByNorth
{
    public class MenuToggle : MonoBehaviour
    {
        public Toggle[] toggles;
        public GameObject[] verticalLayoutGroupPanels;

        void Start()
        {
            for (int i = 0; i < verticalLayoutGroupPanels.Length; i++)
            {
                int index = i;
                toggles[i].onValueChanged.AddListener((isOn) =>
                {
                    for (int j = 0; j < verticalLayoutGroupPanels.Length; j++)
                    {
                        verticalLayoutGroupPanels[j].SetActive(j == index && isOn);
                    }
                });
            }
        }
    }
}
