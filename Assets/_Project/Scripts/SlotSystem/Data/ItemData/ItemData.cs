using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ByNorth.SlotSystem.Data.ItemData {
    public abstract class ItemData : SlotData {
        public ItemType itemType;
        [TextArea(3, 3)]
        public string description;

        protected virtual void Reset()
        {
            itemType = ItemType.None;
            icon = null;
            itemName = "새 아이템";
            description = "새 아이템의 정보를 입력하세요.";
        }
    }
}




