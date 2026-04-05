using System.Collections.Generic;
using ByNorth.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ByNorth.UI.Player
{
    public class BottomEnterChecker: Singleton<BottomEnterChecker>
    {
        public bool HasEntered
        {
            get
            {
                PointerEventData pointerData = new PointerEventData(eventSystem);
                pointerData.position = Input.mousePosition;

                results.Clear();
                raycaster.Raycast(pointerData, results);

                foreach (var result in results)
                {
                    if (result.gameObject == targetImage.gameObject)
                        return true;
                }

                return false;
            }
        }


        private Image targetImage;
        private GraphicRaycaster raycaster;
        private EventSystem eventSystem;
        private readonly List<RaycastResult> results = new();


        protected override void Awake()
        {
            base.Awake();
            
            targetImage = GetComponent<Image>();
            raycaster = GetComponentInParent<GraphicRaycaster>();
            eventSystem = EventSystem.current;
        }
    }
}