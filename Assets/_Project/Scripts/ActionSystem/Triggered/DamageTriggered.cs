using ByNorth.SlotSystem;
using ByNorth.SlotSystem.Data.ItemData;
using ByNorth.Unit;
using UnityEngine;

namespace ByNorth.ActionSystem.Triggered
{
    public class DamageTriggered: MonoBehaviour, ITriggered
    {
        public void OnTrigger(ActionExecutor executor, Unit.Unit caster, Unit.Unit hit)
        {
            if (hit == null) return;
            if (caster == hit) return;
            if (caster.EntityType == hit.EntityType) return;
            if (caster.EntityType == EntityType.Npc && hit.EntityType == EntityType.Object) return;

            if (hit.EntityType == EntityType.Player)
            {
                if (SlotManager.Instance.equipmentGroup.slotsList[1].slotData == null)
                {
                    hit.TakeDamage((int)executor.Influence);
                }
                else
                {
                    if (SlotManager.Instance.GetEquipSlot(1).slotData is ArmorData armorData)
                    {
                        hit.TakeDamage((int)(executor.Influence) - (int)(executor.Influence * armorData.damageReduction));
                    }
                }
            }
            else
            {
                hit.TakeDamage((int)executor.Influence);
            }
        }
    }
}