using ByNorth.Effect;
using UnityEngine;

namespace ByNorth.Unit.Modifier {
    [CreateAssetMenu(fileName = "Stun", menuName = "Modifier/State/Stun")]
    public class StunModifier : ModifierBase, ICondition {
        [Header("게임상에 나타낼 효과 프리팹")]
        public EffectHandler effect;

        public ConditionType Condition => ConditionType.Stun;
        public float Influence => time;


        public override void Modify(Unit unit)
        {
            if (unit.CurrentHp > 0)
            {
                EffectHandler.Spawn(effect, unit.transform);
            }
        }

        public override void Undo(Unit unit)
        {

        }
    }
}