using ByNorth.Effect;
using UnityEngine;

namespace ByNorth.Unit.Modifier {
    [CreateAssetMenu(fileName = "Blood", menuName = "Effect/Blood")]
    public class BloodEffectData : ScriptableObject {
        public EffectHandler effect;

        public void EffectSpawn(Unit unit)
        {
            EffectHandler.Spawn(effect, unit.transform);
        }

        public void VFXEffectSpawn(Unit unit)
        {
            EffectHandler.VFXSpawn(effect, unit.transform);
        }
    }
}