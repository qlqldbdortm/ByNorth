using ByNorth.SlotSystem.Data;
using ByNorth.SlotSystem.Data.ItemData;
using ByNorth.SlotSystem.Data.SkillData;
using ByNorth.Unit.Behaviour.Movable;
using TMPro;
using UnityEngine;

namespace ByNorth.SlotSystem.Slot {
    public class QuickSlot : Slot {
        public TextMeshProUGUI counterText;

        public override void OnSwap(Slot fromSlot)
        {
            if (fromSlot is StorageSlot) return;
            if (fromSlot?.slotData is not IUsable) return;

            SlotData temp = slotData;
            slotData = fromSlot.slotData;
            Refresh();
            if (fromSlot is QuickSlot)
            {
                fromSlot.slotData = temp;
                fromSlot.Refresh();
            }
        }

        public override void Refresh()
        {
            base.Refresh();

            if (counterText is null) return;

            counterText.enabled = slotData is not null;

            if (slotData is ItemData)
            {
                int cnt = SlotManager.Instance.GetCountItem(slotData);
                counterText.text = cnt.ToString();
            }
            else if (slotData is SkillData skillData)
            {
                int cost = SlotManager.Instance.GetSkillCost(skillData);
                skillData.IsUsable = true;
                counterText.text = cost.ToString();
            }
        }

        public override void ClearSlot()
        {
            base.ClearSlot();

            counterText.enabled = false;
        }

        public override void Use()
        {
            if (slotData is IUsable usable)
            {
                if (CanUse && usable.IsUsable)
                {
                    if (SlotManager.Instance.GetCountItem(slotData) > 0 || slotData is SkillData)
                    {
                        if ((slotData as SkillData)?.cost > PlayerHandler.Player.Unit.CurrentStamina) return;
                        usable.Use();
                        SlotManager.Instance.CooldownAll(slotData);
                    }
                    else
                    {
                        print("아이템이 없거나 스킬이 아닙니다.");
                    }
                }
            }
        }
    }
}