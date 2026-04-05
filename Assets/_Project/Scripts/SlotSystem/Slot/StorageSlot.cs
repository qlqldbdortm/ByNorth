using ByNorth.SlotSystem.Data;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ByNorth.SlotSystem.Slot {
    public class StorageSlot : Slot {
        public override void OnSwap(Slot fromSlot)
        {
            if (fromSlot is QuickSlot) return;
            if (fromSlot is SkillSlot) return;
            (slotData, fromSlot.slotData) = (fromSlot.slotData, slotData);
            if (fromSlot is EquipSlot equipSlot)
            {
                SlotManager.Instance.RemoveSkillSlot(equipSlot.skillIdx);
                equipSlot.skillIdx = -1;
            }
            Refresh();
            fromSlot.Refresh();
        }

        public override void Use()
        {
            MoveInventory(slotData);
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            float timeSinceLastClick = Time.time - lastClickTime;
            if (timeSinceLastClick <= DOUBLE_CLICK_THRESHOLD)
            {
                MoveInventory(slotData);
            }
            lastClickTime = Time.time;
        }

        public void MoveInventory(SlotData data)
        {
            bool result = SlotManager.Instance.AddItem(data);
            if (result)
            {
                ClearSlot();
            }
            else
            {
                Debug.Log("아이템을 넣을 공간이 없습니다.");
            }
        }
    }
}
