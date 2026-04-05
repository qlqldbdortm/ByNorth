using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace ByNorth.SlotSystem {
    [JsonObject(MemberSerialization.OptIn)]
    public class SlotGroup: MonoBehaviour
    {
        public GameObject slotRoot;
        [JsonProperty]
        public List<Slot.Slot> slotsList;

        public bool IsVisible => slotRoot.activeSelf;


        public void Show(bool isVisible)
        {
            slotRoot.SetActive(isVisible);
        }
    }
}