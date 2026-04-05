using ByNorth.ActionSystem.Triggered;
using ByNorth.Unit;
using ByNorth.Unit.Modifier;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ByNorth.ActionSystem.Triggered {
    public class ModifierTriggered : MonoBehaviour, ITriggered {
        public ModifierBase modifier;
        public void OnTrigger(ActionExecutor executor, Unit.Unit caster, Unit.Unit hit)
        {
            if (hit == null) return;
            if (caster == hit) return;
            if (caster.EntityType == hit.EntityType) return;
            if (caster.EntityType == EntityType.Npc && hit.EntityType == EntityType.Object) return;
            hit.AddModifier(modifier);
        }
    }
}