using ByNorth.SlotSystem.Data;

namespace ByNorth.SlotSystem.Slot
{
    public class SkillSlot: Slot
    {
        public override void OnSwap(Slot fromSlot)
        {
            // 아무것도 할 게 없음.
        }

        public override void Use()
        {
            // TODO: 스킬을 Skill창에서 곧바로 사용할 거라면, Use()를 해줘야 함.         
            if (slotData is IUsable usable)
            {
                if (CanUse && usable.IsUsable)
                {
                    usable.Use();
                    
                    SlotManager.Instance.CooldownAll(slotData);
                }
            }
        }
    }
}