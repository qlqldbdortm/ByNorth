using ByNorth.SlotSystem.Data.ItemData;
using System;
using Unity.VisualScripting;
using UnityEngine;

namespace ByNorth.SlotSystem.Slot {
    public class EquipSlot : Slot {
        public int skillIdx;
        public override void OnSwap(Slot fromSlot)
        {
            if (fromSlot is not InventorySlot && fromSlot is not StorageSlot) return;
            if (fromSlot.slotData is not WeaponData && fromSlot.slotData is not ArmorData) return;
            SlotManager.Instance.RemoveSkillSlot(0);
            if (fromSlot.slotData is WeaponData weaponData)
            {
                skillIdx = SlotManager.Instance.AddSkill(weaponData);
            }
            (slotData, fromSlot.slotData) = (fromSlot.slotData, slotData);
            Refresh();
            fromSlot.Refresh();
        }

        public override void Use()
        {
            // 비무장 상태가 유저에겐 존재할 수 없음.
        }
    }
}
