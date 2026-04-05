using ByNorth.Unit;
using ByNorth.Unit.Modifier;
using UnityEngine;

namespace ByNorth.ActionSystem.Triggered {
    public class BloodTriggered : MonoBehaviour, ITriggered {
        public BloodEffectData bloodEffect;
        public void OnTrigger(ActionExecutor executor, Unit.Unit caster, Unit.Unit hit)
        {
            if (hit == null) return;
            if (caster == hit) return;
            if (caster.EntityType == hit.EntityType) return;
            if (caster.EntityType == EntityType.Npc && hit.EntityType == EntityType.Object) return;

            if ((caster.EntityType == EntityType.Player && hit.EntityType == EntityType.Npc) ||
                (caster.EntityType == EntityType.Npc && hit.EntityType == EntityType.Player))
            {
                bloodEffect.EffectSpawn(hit);
            }
        }
    }
}
