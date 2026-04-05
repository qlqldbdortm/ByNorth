using ByNorth.SlotSystem.Data;
using ByNorth.SlotSystem.Data.ItemData;


namespace ByNorth.SlotSystem.Slot
{
    public class InventorySlot: Slot
    {
        public override void OnSwap(Slot fromSlot)
        {
            if (fromSlot is QuickSlot) return;
            
            (slotData, fromSlot.slotData) = (fromSlot.slotData, slotData);
            if (fromSlot is EquipSlot equipSlot)
            {
                SlotManager.Instance.RemoveSkillSlot(equipSlot.skillIdx);
                equipSlot.skillIdx = -1;
            }
            Refresh();
            fromSlot.Refresh();
            SlotManager.Instance.RefreshAllQuickSlot();
        }

        public override void Use()
        {
            if (CanUse && slotData is IUsable usable)
            {
                usable.Use();

                SlotManager.Instance.CooldownAll(slotData);
            }
            else if (slotData is WeaponData weaponData)
            {
                if (weaponData.weaponType is WeaponType.Main)
                {
                    SlotManager.Instance.RemoveSkillSlot(0);
                    SlotManager.Instance.GetEquipSlot(0).OnSwap(this);
                }
            }
            else if (slotData is ArmorData armorData)
            {
                SlotManager.Instance.GetEquipSlot(1).OnSwap(this);
            }
        }
    }
}